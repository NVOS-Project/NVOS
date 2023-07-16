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
    public class WorldUIService
    {
        public List<Window3D> windows = new List<Window3D>();

        public Window3D CreateWindow(string title)
        {
            Window3D window = new Window3D(title);
            windows.Add(window);
            window.OnWindowStateChanged += Window_OnWindowStateChanged;

            return window;
        }

        private void Window_OnWindowStateChanged(object sender, WindowStateChangedEventArgs e)
        {

        }
    }
}
