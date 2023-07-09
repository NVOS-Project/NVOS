﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public abstract class Control : IDisposable
    {
        private string name;

        private float positionOffsetX = 0;
        private float positionOffsetY = 0;
        private float positionScaleX = 0;
        private float positionScaleY = 0;

        private float sizeOffsetX = 0;
        private float sizeOffsetY = 0;
        private float sizeScaleX = 0;
        private float sizeScaleY = 0;

        private float minWidth = 0;
        private float minHeight = 0;
        private float preferredWidth = 0;
        private float preferredHeight = 0;
        private float flexibleWidth = 0;
        private float flexibleHeight = 0;
        private int layoutPriority = 1;


        private bool isTransformDirty = false;
        private bool isVisible = true;

        private RectTransform rectTransform;
        private LayoutElement layoutElement;
        protected GameObject root;

        public string Name 
        { 
            get 
            { 
                return name; 
            } 
            set 
            {
                root.name = value;
                name = value;
            } 
        }

        public float PositionOffsetX
        {
            get
            {
                return positionOffsetX;
            }
            set
            {
                positionOffsetX = value;
                isTransformDirty = true;
            }
        }

        public float PositionOffsetY
        {
            get
            {
                return positionOffsetY;
            }
            set
            {
                positionOffsetY = value;
                isTransformDirty = true;
            }
        }

        public float PositionScaleX
        {
            get
            {
                return positionScaleX;
            }
            set
            {
                positionScaleX = value;
                isTransformDirty = true;
            }
        }

        public float PositionScaleY
        {
            get
            {
                return positionScaleY;
            }
            set
            {
                positionScaleY = value;
                isTransformDirty = true;
            }
        }

        public float SizeOffsetX
        {
            get
            {
                return sizeOffsetX;
            }
            set
            {
                sizeOffsetX = value;
                isTransformDirty = true;
            }
        }

        public float SizeOffsetY
        {
            get
            {
                return sizeOffsetY;
            }
            set
            {
                sizeOffsetY = value;
                isTransformDirty = true;
            }
        }

        public float SizeScaleX
        {
            get
            {
                return sizeScaleX;
            }
            set
            {
                sizeScaleX = value;
                isTransformDirty = true;
            }
        }

        public float SizeScaleY
        {
            get
            {
                return sizeScaleY;
            }
            set
            {
                sizeScaleY = value;
                isTransformDirty = true;
            }
        }

        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    root.SetActive(value);
                }
            }
        }

        public float MinWidth
        {
            get
            {
                return minWidth;
            }
            set
            {
                layoutElement.minWidth = value;
                minWidth = value;
            }
        }

        public float MinHeight
        {
            get
            {
                return minHeight;
            }
            set
            {
                layoutElement.minHeight = value;
                minHeight = value;
            }
        }

        public float PreferredWidth
        {
            get
            {
                return preferredWidth;
            }
            set
            {
                layoutElement.preferredWidth = value;
                preferredWidth = value;
            }
        }

        public float PreferredHeight
        {
            get
            {
                return preferredHeight;
            }
            set
            {
                layoutElement.preferredHeight = value;
                preferredHeight = value;
            }
        }

        public float FlexibleWidth
        {
            get
            {
                return flexibleWidth;
            }
            set
            {
                layoutElement.flexibleWidth = value;
                flexibleWidth = value;
            }
        }

        public float FlexibleHeight
        {
            get
            {
                return flexibleHeight;
            }
            set
            {
                layoutElement.flexibleHeight = value;
                flexibleHeight = value;
            }
        }

        public int LayoutPriority
        {
            get
            {
                return layoutPriority;
            }
            set
            {
                layoutElement.layoutPriority = value;
                layoutPriority = value;
            }
        }

        public List<Control> controls = new List<Control>();

        public Control()
        {
            root = new GameObject("Control");
            rectTransform = root.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            layoutElement = root.AddComponent<LayoutElement>();
        }

        public Control(string name)
        {
            root = new GameObject(name);
            rectTransform = root.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            layoutElement = root.AddComponent<LayoutElement>();
        }  

        protected virtual void UpdateDirtyTransform()
        {
            isTransformDirty = false;
            rectTransform.anchorMin = new Vector2(positionScaleX, 1f - positionScaleY - sizeScaleY);
            rectTransform.anchorMax = new Vector2(positionScaleX + sizeScaleX, 1f - positionScaleY);
            rectTransform.anchoredPosition = new Vector2(positionOffsetX, -positionOffsetY);
            rectTransform.sizeDelta = new Vector2(sizeOffsetX, sizeOffsetY);
        }

        public virtual void Update()
        {
            if (isTransformDirty)
                UpdateDirtyTransform();

            foreach (Control child in controls)
            {
                if (child.IsVisible)
                    child.Update();
            }
        }

        public void AddChild(Control child)
        {
            isTransformDirty = true;
            child.root.transform.SetParent(root.transform);
            controls.Add(child);
        }

        public GameObject GetRootObject()
        {
            return root;
        }

        public void Dispose()
        {
            foreach (Control child in controls)
                child.Dispose();
            
            rectTransform = null;
            GameObject.Destroy(root);
        }
    }
}
