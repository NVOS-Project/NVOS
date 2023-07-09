using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NVOS.UI.Models.Enums;

namespace NVOS.UI.Models
{
    public abstract class Window : IDisposable
    {
        private Outline outline;
        protected Panel content;
        protected GameObject root;
        protected RectTransform rectTransform;

        private float outlineThickness;
        private float width;
        private float height;
        private bool renderOutline;

        public float OutlineThickness
        {
            get
            {
                return outlineThickness;
            }
            set
            {
                outline.effectDistance = new Vector2(value, value);
                outlineThickness = value;
            }
        }

        public bool RenderOutline
        {
            get
            {
                return renderOutline;
            }
            set
            {
                if (value)
                {
                    outline.effectColor = Color.clear;
                }
                else
                {
                    outline.effectColor = Color.black;
                }
                renderOutline = value;
            }
        }

        public float Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y);
            }
        }

        public float Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, value);
            }
        }

        public Window() : this("Window") { } 

        public Window(string name)
        {
            root = new GameObject(name);
            rectTransform = root.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(1f, 1f);
            width = 1f;
            height = 1f;

            root.AddComponent<VerticalLayoutGroup>();
            root.AddComponent<Image>();

            outline = root.AddComponent<Outline>();
            outline.effectDistance = new Vector2(0.01f, 0.01f);
            outlineThickness = 0.01f;
            outline.effectColor = Color.black;
            renderOutline = true;

            content = new Panel("Content");
            content.GetRootObject().transform.SetParent(root.transform);
            content.BackgroundColor = Color.white;
        }
        
        public void Update()
        {
            foreach (Control child in content.controls)
            {
                if (child.IsVisible)
                    child.Update();
            }
        }

        public void Dispose()
        {
            foreach (Control child in content.controls)
            {
                child.Dispose();
            }
            content.Dispose();
            content = null;
            rectTransform = null;
            GameObject.Destroy(root);
            root = null;
        }

        public Panel GetContent()
        {
            return content;
        }

        public GameObject GetRootObject()
        {
            return root;
        }

        public RectTransform GetRectTransform()
        {
            return rectTransform;
        }
    }
}
