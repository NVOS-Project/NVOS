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
        private GameObject tickerObject;
        private GameTickProvider gameTickProvider;

        public bool Init()
        {
            tickerObject = new GameObject("WorldUITicker");
            gameTickProvider = tickerObject.AddComponent<GameTickProvider>();
            gameTickProvider.OnLateUpdate += GameTickProvider_OnLateUpdate;

            windows = new List<Window3D>();

            return true;
        }

        public void Dispose()
        {
            foreach (Window3D window in windows)
            {
                window.Dispose();
            }
            windows = null;
        }

        public Window3D CreateWindow(string title, float width, float height)
        {
            Window3D window = new Window3D(title, width, height);
            Transform cameraTransform = Camera.main.transform;
            GameObject windowObject = window.GetRootObject();

            Vector3 windowPosition = (cameraTransform.forward * 0.5f) + cameraTransform.position;
            windowObject.transform.position = windowPosition;

            Vector3 directionVector = (windowObject.transform.position - cameraTransform.position).normalized;
            windowObject.transform.rotation = Quaternion.LookRotation(directionVector, Vector3.up);

            windows.Add(window);
            window.OnClose += Window_OnClose;

            return window;
        }

        public List<Window3D> GetWindows()
        {
            return windows;
        }

        private void Window_OnClose(object sender, WindowEventArgs e)
        {
            windows.Remove((Window3D)e.Window);
        }

        private void GameTickProvider_OnLateUpdate(object sender, EventArgs e)
        {
            foreach (Window3D window in windows)
            {
                window.Update();

                GameObject windowObject = window.GetRootObject();
                Vector3 windowPosition = windowObject.transform.position;

                float distance = Vector3.Distance(windowPosition, Vector3.zero);

                if (distance > 3f)
                {
                    Vector3 centerVector = windowPosition.normalized * 3f;
                    windowObject.transform.position = centerVector;
                }

                Vector3 windowVector = windowPosition.normalized;
                windowObject.transform.rotation = Quaternion.LookRotation(windowVector, Vector3.up);
                windowObject.transform.eulerAngles = new Vector3(0f, windowObject.transform.eulerAngles.y, windowObject.transform.eulerAngles.z);
            }
        }
    }
}
