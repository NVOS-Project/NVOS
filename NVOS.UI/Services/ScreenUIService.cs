using NVOS.Core;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NVOS.UI.Services
{
    [ServiceType(ServiceType.Singleton)]
    public class ScreenUIService : IService, IDisposable
    {
        private Canvas canvas;
        private Dictionary<Window2D, Vector2Int> windows;

        private int gridWidth;
        private int gridHeight;
        private float tileWidth;
        private float tileHeight;
        private float hudWidth;
        private float hudHeight;

        private bool[,] grid;

        public bool Init()
        {
            // get values from db??
            gridWidth = 8;
            gridHeight = 5;
            hudWidth = 1.5f;
            hudHeight = 1.2f;
            grid = new bool[gridWidth, gridHeight];

            canvas = new GameObject("HUD").AddComponent<Canvas>();
            Transform canvasTransform = canvas.gameObject.transform;
            canvasTransform.SetParent(Camera.main.transform);
            canvas.gameObject.AddComponent<CanvasRenderer>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvasTransform.localPosition = new Vector3(0, 0, 1);
            canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(1.5f, 1.2f);

            tileWidth = hudWidth / gridWidth;
            tileHeight = hudHeight / gridHeight;

            windows = new Dictionary<Window2D, Vector2Int>();

            return true;
        }

        public void Dispose()
        {
            GameObject.Destroy(canvas.gameObject);
            canvas = null;

            foreach (Window2D window in windows.Keys)
            {
                window.Dispose();
            }
            windows = null;

            grid = null;
        }

        public Window2D CreateWindow(string title, int widthCount, int heightCount, int columnIndex, int rowIndex)
        {
            Window2D window = new Window2D(title, widthCount, heightCount);
            window.GetRootObject().transform.SetParent(canvas.gameObject.transform, false);

            float posX = columnIndex * tileWidth;
            float posY = rowIndex * tileHeight;

            RectTransform windowRect = window.GetRectTransform();
            windowRect.anchorMin = Vector2.zero;
            windowRect.anchorMax = Vector2.zero;
            windowRect.pivot = Vector2.zero;
            windowRect.anchoredPosition = new Vector2(posX, posY);
            windowRect.sizeDelta = new Vector2(tileWidth * widthCount, tileHeight * heightCount);

            bool isGridValid = true;
            for (int i = columnIndex; i < columnIndex + widthCount; i++)
            {
                for (int j = rowIndex; j < rowIndex + heightCount; j++)
                {
                    if (grid[i, j])
                    {
                        isGridValid = false;
                        break;
                    }
                    else
                    {
                        grid[i, j] = true;
                    }
                }

                if (!isGridValid)
                    break;
            }

            if (!isGridValid)
                window.Hide();

            windows.Add(window, new Vector2Int(columnIndex, rowIndex));
            return window;
        }

        public List<Window2D> GetWindows()
        {
            return windows.Keys.ToList();
        }

        public Vector2Int GetWindowPosition(Window2D window)
        {
            return windows[window];
        }
    }
}
