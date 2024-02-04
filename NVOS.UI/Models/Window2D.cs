using UnityEngine;

namespace NVOS.UI.Models
{
    public class Window2D : Window
    {
        protected Canvas canvas;
        public Window2D(string name, float width, float height) : base(name)
        {
            canvas = root.AddComponent<Canvas>();
            root.AddComponent<CanvasRenderer>();
            root.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            canvas.sortingOrder = 999;
            rectTransform.sizeDelta = new Vector2(width, height);
        }
    }
}
