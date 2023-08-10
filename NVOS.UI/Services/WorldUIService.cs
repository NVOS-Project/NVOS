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
            gameTickProvider.OnLateUpdate -= GameTickProvider_OnLateUpdate;
            gameTickProvider = null;
            GameObject.Destroy(tickerObject);
            tickerObject = null;

            foreach (Window3D window in windows)
            {
                window.Dispose();
            }
            windows = null;
        }

        public Window3D CreateWindow(string name, float width, float height)
        {
            if (windows.Where(x => x.Name == name).Count() > 0)
                throw new Exception($"Window of name '{name}' already exists!");

            Window3D window = new Window3D(name, width, height);
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

                windowObject.transform.eulerAngles = new Vector3(0f, windowObject.transform.eulerAngles.y, 0f);
            }
        }
    }
}
