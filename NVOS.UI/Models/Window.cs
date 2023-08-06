using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NVOS.UI.Models.Enums;
using NVOS.UI.Models.EventArgs;
using UnityEngine.XR.Interaction.Toolkit.UI;

namespace NVOS.UI.Models
{
    public abstract class Window : IDisposable
    {
        protected Panel content;
        protected GameObject root;
        protected RectTransform rectTransform;
        private WindowState state;

        public event EventHandler<WindowStateChangedEventArgs> OnWindowStateChanged;
        public event EventHandler<WindowEventArgs> OnClose;

        public WindowState State { get { return state; } }

        public Window(string title)
        {
            root = new GameObject(title);
            rectTransform = root.AddComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = new Vector2(1f, 1f);

            root.AddComponent<VerticalLayoutGroup>();

            content = new Panel("Content");
            content.GetRootObject().transform.SetParent(root.transform, false);
            content.BackgroundColor = Color.white;

            root.AddComponent<TrackedDeviceGraphicRaycaster>();
        }

        private void SetVisible(bool visible)
        {
            if (visible)
                state = WindowState.Normal;
            else
                state = WindowState.Hidden;

            root.SetActive(visible);
        }
        
        public virtual void Update()
        {
            foreach (Control child in content.controls)
            {
                if (child.IsVisible)
                    child.Update();
            }
        }

        public void Show()
        {
            if (state == WindowState.Normal)
                throw new InvalidOperationException("Window is already open!");

            SetVisible(true);
            OnWindowStateChanged?.Invoke(this, new WindowStateChangedEventArgs(this, state));
        }

        public void Hide()
        {
            if (state == WindowState.Hidden)
                throw new InvalidOperationException("Window is already minimized!");

            SetVisible(false);
            OnWindowStateChanged?.Invoke(this, new WindowStateChangedEventArgs(this, state));
        }

        public void Close()
        {
            OnClose?.Invoke(this, new WindowEventArgs(this));
            Dispose();
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
