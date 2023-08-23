using NVOS.Core;
using NVOS.Core.Database;
using NVOS.Core.Services;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.UI.Models;
using NVOS.UI.Models.EventArgs;
using NVOS.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NVOS.UI.Services
{
    [ServiceType(ServiceType.Singleton)]
    [ServiceDependency(typeof(UpdateProviderService))]
    public class WorldUIService : IService, IDisposable
    {
        private UpdateProviderService updateProvider;
        private List<Window3D> windows;
        private GameObject worldAnchor;
        private bool isDisposed;

        private bool isMoving;
        private float moveWaitTime;
        private float moveWaitTimePassed;
        private float windowSpeed;
        private float rotationSpeed;
        private float walkSpeed;
        private float windowSpawnDistance;
        private float windowBubbleRadius;

        public void Init()
        {
            updateProvider = ServiceLocator.Resolve<UpdateProviderService>();
            IDatabaseService db = ServiceLocator.Resolve<IDatabaseService>();
            DbCollection collection = db.GetCollection("world_ui");

            moveWaitTime = (float)collection.ReadOrDefault("moveWaitTime", 2.5f); 
            windowSpeed = (float)collection.ReadOrDefault("windowSpeed", 2f); // 2 meters per second
            rotationSpeed = (float)collection.ReadOrDefault("rotationSpeed", 360f); // 360 degrees per second
            walkSpeed = (float)collection.ReadOrDefault("walkSpeed", 0.7f);
            windowSpawnDistance = (float)collection.ReadOrDefault("windowSpawnDistance", 0.5f);
            windowBubbleRadius = (float)collection.ReadOrDefault("windowBubbleRadius", 1f);

            worldAnchor = new GameObject("WorldUIAnchor");
            Transform cameraTransform = Camera.main.transform;
            worldAnchor.transform.position = cameraTransform.position;
            worldAnchor.transform.rotation = Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f);

            windows = new List<Window3D>();
            updateProvider.OnLateUpdate += UpdateProvider_OnLateUpdate;
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            updateProvider.OnLateUpdate -= UpdateProvider_OnLateUpdate;
            updateProvider = null;

            foreach (Window3D window in windows)
            {
                window.Dispose();
            }
            windows = null;

            GameObject.Destroy(worldAnchor);
            worldAnchor = null;
            isDisposed = true;
        }

        public Window3D CreateWindow(string name, float width, float height)
        {
            Window3D window = new Window3D(name, width, height);

            Transform cameraTransform = Camera.main.transform;
            GameObject windowObject = window.GetRootObject();
            windowObject.transform.SetParent(worldAnchor.transform, true);

            Vector3 windowPosition = (cameraTransform.forward * windowSpawnDistance) + cameraTransform.position;
            windowObject.transform.position = windowPosition;

            Vector3 directionVector = (windowObject.transform.position - cameraTransform.position).normalized;
            windowObject.transform.rotation = Quaternion.LookRotation(directionVector, Vector3.up);

            windows.Add(window);
            window.OnClose += Window_OnClose;
            window.OnWindowStateChanged += Window_OnWindowStateChanged;

            return window;
        }

        public List<Window3D> GetWindows()
        {
            return windows;
        }

        public IEnumerable<Window3D> GetWindowsWithName(string name)
        {
            return windows.Where(x => x.Name == name);
        }

        private void Window_OnClose(object sender, WindowEventArgs e)
        {
            windows.Remove((Window3D)e.Window);
        }

        private void UpdateProvider_OnLateUpdate(object sender, EventArgs e)
        {
            Transform worldAnchorTransform = worldAnchor.transform;
            Transform cameraTransform = Camera.main.transform;

            foreach (Window3D window in windows)
            {
                window.Update();

                GameObject windowObject = window.GetRootObject();
                Vector3 windowPosition = windowObject.transform.position;

                float distance = Vector3.Distance(windowPosition, worldAnchorTransform.position);

                if (distance > windowBubbleRadius)
                {
                    Vector3 vector = (windowPosition - worldAnchorTransform.position).normalized * windowBubbleRadius;
                    windowObject.transform.position = vector;
                }

                windowObject.transform.eulerAngles = new Vector3(0f, windowObject.transform.eulerAngles.y, 0f);
            }

            if (!isMoving)
            {
                if (Vector3.Distance(cameraTransform.position, worldAnchorTransform.position) > 3f)
                {
                    isMoving = true;
                    moveWaitTimePassed = 0f;

                    foreach (Window3D window in windows)
                    {
                        if (!window.IsLocked)
                            window.GetRootObject().SetActive(false);
                    }
                }
            }

            if (isMoving)
            {
                float position_step = windowSpeed * Time.deltaTime;
                float rotation_step = rotationSpeed * Time.deltaTime;
                worldAnchorTransform.position = Vector3.MoveTowards(worldAnchorTransform.position, cameraTransform.position, position_step);
                worldAnchorTransform.rotation = Quaternion.RotateTowards(worldAnchorTransform.rotation, Quaternion.Euler(0f, worldAnchorTransform.rotation.eulerAngles.y, 0f), rotation_step);

                if (Vector3.Distance(worldAnchorTransform.position, cameraTransform.position) < 1f)
                {
                    if (Camera.main.velocity.magnitude < walkSpeed)
                        moveWaitTimePassed += Time.deltaTime;
                    else
                        moveWaitTimePassed = 0f;
                }

                if (moveWaitTimePassed > moveWaitTime)
                {
                    isMoving = false;
                    moveWaitTimePassed = 0f;

                    foreach (Window3D window in windows)
                    {
                        if (!window.IsLocked)
                            window.GetRootObject().SetActive(true);
                    }
                }
            }
        }

        private void Window_OnWindowStateChanged(object sender, WindowStateChangedEventArgs e)
        {
            if (e.State == Models.Enums.WindowState.Normal)
            {
                Window3D window = (Window3D)e.Window;

                Transform cameraTransform = Camera.main.transform;
                GameObject windowObject = window.GetRootObject();

                Vector3 windowPosition = (cameraTransform.forward * windowSpawnDistance) + cameraTransform.position;
                windowObject.transform.position = windowPosition;

                Vector3 directionVector = (windowObject.transform.position - cameraTransform.position).normalized;
                windowObject.transform.rotation = Quaternion.LookRotation(directionVector, Vector3.up);
            }
        }
    }
}
