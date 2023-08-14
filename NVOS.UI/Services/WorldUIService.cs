using NVOS.Core;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.UI.Models;
using NVOS.UI.Models.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NVOS.UI.Services
{
    [ServiceType(ServiceType.Singleton)]
    public class WorldUIService : IService, IDisposable
    {
        private List<Window3D> windows;
        private GameObject worldAnchor;

        private GameObject tickerObject;
        private GameTickProvider gameTickProvider;

        private bool isMoving;
        private float moveWaitTime;
        private float moveWaitTimePassed;
        private float windowSpeed;
        private float walkSpeed;
        private float windowSpawnDistance;
        private float windowBubbleRadius;

        public bool Init()
        {
            worldAnchor = new GameObject("WorldUIAnchor");
            worldAnchor.transform.position = Vector3.zero;
            tickerObject = new GameObject("WorldUITicker");
            moveWaitTime = 5f;
            windowSpeed = 2f;
            walkSpeed = 0.7f;
            windowSpawnDistance = 0.5f;
            windowBubbleRadius = 3f;
            gameTickProvider = tickerObject.AddComponent<GameTickProvider>();
            gameTickProvider.OnLateUpdate += GameTickProvider_OnLateUpdate;

            windows = new List<Window3D>();

            return true;
        }

        public void Dispose()
        {
            gameTickProvider.OnLateUpdate -= GameTickProvider_OnLateUpdate;
            gameTickProvider = null;
            GameObject.Destroy(tickerObject);
            tickerObject = null;

            foreach (Window3D window in windows)
            {
                window.Dispose();
            }
            windows = null;

            GameObject.Destroy(worldAnchor);
            worldAnchor = null;
        }

        public Window3D CreateWindow(string name, float width, float height)
        {
            if (windows.Where(x => x.Name == name).Count() > 0)
                throw new Exception($"Window of name '{name}' already exists!");

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

        public Window3D GetWindowByName(string name)
        {
            Window3D window = windows.Where(x => x.Name == name).FirstOrDefault();
            return window;
        }

        private void Window_OnClose(object sender, WindowEventArgs e)
        {
            windows.Remove((Window3D)e.Window);
        }

        private void GameTickProvider_OnLateUpdate(object sender, EventArgs e)
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
                float step = windowSpeed * Time.deltaTime;
                worldAnchorTransform.position = Vector3.MoveTowards(worldAnchorTransform.position, cameraTransform.position, step);
                worldAnchorTransform.position = new Vector3(worldAnchorTransform.position.x, 0, worldAnchorTransform.position.z);
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
