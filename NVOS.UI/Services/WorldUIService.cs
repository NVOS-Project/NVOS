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
        private float positionSmoothing;
        private float rotationSmoothing;
        private float walkSpeed;
        private float windowSpawnDistance;
        private float windowBubbleRadius;

        public void Init()
        {
            updateProvider = ServiceLocator.Resolve<UpdateProviderService>();
            IDatabaseService db = ServiceLocator.Resolve<IDatabaseService>();
            DbCollection collection = db.GetCollection("world_ui");

            moveWaitTime = (float)collection.ReadOrDefault("moveWaitTime", 1.25f); 
            positionSmoothing = (float)collection.ReadOrDefault("positionSmoothing", 0.125f);
            rotationSmoothing = (float)collection.ReadOrDefault("rotationSmoothing", 0.125f);
            walkSpeed = (float)collection.ReadOrDefault("walkSpeed", 0.1f);
            windowSpawnDistance = (float)collection.ReadOrDefault("windowSpawnDistance", 0.25f);
            windowBubbleRadius = (float)collection.ReadOrDefault("windowBubbleRadius", 1f);

            worldAnchor = new GameObject("WorldUIAnchor");
            Transform cameraTransform = Camera.main.transform;
            worldAnchor.transform.position = cameraTransform.position;
            worldAnchor.transform.eulerAngles = new Vector3(0f, cameraTransform.rotation.eulerAngles.y, 0f);

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

            Vector3 targetWorldPos = (cameraTransform.forward * windowSpawnDistance) + cameraTransform.position;
            windowObject.transform.localPosition = windowObject.transform.InverseTransformPoint(targetWorldPos);

            Vector3 directionVector = windowObject.transform.localPosition.normalized;
            windowObject.transform.localRotation = Quaternion.LookRotation(directionVector, Vector3.up);

            windows.Add(window);
            window.OnClose += Window_OnClose;

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
                // Constrain the window position to the bubble
                windowObject.transform.localPosition = Vector3.ClampMagnitude(windowObject.transform.localPosition, windowBubbleRadius);
                // Constain the rotation to look at the anchor
                Quaternion rotation = Quaternion.LookRotation(windowObject.transform.localPosition.normalized, Vector3.up);
                windowObject.transform.localEulerAngles = new Vector3(0f, rotation.eulerAngles.y, 0f);
            }

            if (!isMoving)
            {
                if (Vector3.Distance(cameraTransform.position, worldAnchorTransform.position) > windowBubbleRadius)
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
                float position_step = Mathf.Clamp01(Mathf.Max(1f - positionSmoothing, 0.01f) * 10 * Time.deltaTime);
                float rotation_step = Mathf.Clamp01(Mathf.Max(1f - rotationSmoothing, 0.01f) * 10 * Time.deltaTime);
                worldAnchorTransform.position = Vector3.Lerp(worldAnchorTransform.position, cameraTransform.position, position_step);
                worldAnchorTransform.rotation = Quaternion.Lerp(worldAnchorTransform.rotation, Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f), rotation_step);

                if (Vector3.Distance(worldAnchorTransform.position, cameraTransform.position) < 0.01f)
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
    }
}
