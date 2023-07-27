using NVOS.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NVOS.UI
{
    public class ScreenUIService : IUIService<Window2D>
    {
        public ScreenUIService() { }

        private Canvas canvas;
        private Dictionary<Window2D, Vector2Int> windows = new Dictionary<Window2D, Vector2Int>();

        private int gridWidth = 8;
        private int gridHeight = 5;
        private float tileWidth;
        private float tileHeight;

        private bool[,] grid = new bool[8, 5];


        public Window2D CreateWindow(string title, int widthCount, int heightCount, int columnIndex, int rowIndex)
        {
            Window2D window = new Window2D(title, widthCount, heightCount);
            window.GetRootObject().transform.SetParent(canvas.gameObject.transform);

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

        public void Update()
        {
            if (!canvas)
            {
                canvas = new GameObject("HUD").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.planeDistance = float.MinValue;

                float screenWidth = Camera.main.pixelWidth;
                float screenHeight = Camera.main.pixelHeight;

                tileWidth = screenWidth / gridWidth;
                tileHeight = screenHeight / gridHeight;
            }
        }
    }
}
