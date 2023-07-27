using NVOS.UI.Models;
using NVOS.UI.Models.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NVOS.UI
{
    public class WorldUIService : IUIService<Window3D>
    {
        public WorldUIService() { }

        public List<Window3D> windows = new List<Window3D>();

        public Window3D CreateWindow(string title, float width, float height)
        {
            Window3D window = new Window3D(title, width, height);
            Transform cameraTransform = Camera.main.transform;
            Vector3 windowPosition = (cameraTransform.forward * 2f) + cameraTransform.position;
            window.GetRootObject().transform.position = windowPosition;
            windows.Add(window);

            return window;
        }


        public List<Window3D> GetWindows()
        {
            return windows;
        }

        public void Update()
        {
            foreach (Window3D window in windows)
            {
                window.Update();

                Vector3 cameraPosition = Camera.main.transform.position;
                GameObject windowObject = window.GetRootObject();
                Vector3 windowPosition = windowObject.transform.position; 

                float distance = Vector3.Distance(windowPosition, cameraPosition);

                if (distance > 3f)
                {
                    Vector3 centerVector = windowPosition - cameraPosition;
                    centerVector = centerVector.normalized * 3f;
                    windowObject.transform.position = cameraPosition + centerVector;
                }

                Quaternion rotation = Quaternion.LookRotation(Vector3.zero);
                windowObject.transform.rotation = rotation;
            }
        }
    }
}
