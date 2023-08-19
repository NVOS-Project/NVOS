using NVOS.UI.Models.EventArgs;
using NVOS.UI.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class VerticalSlider : Control
    {
        private Slider slider;
        private Panel backgroundPanel;
        private Panel fillArea;
        private Panel fill;
        private Panel handleArea;
        private Panel handle;

        private Color backgroundColor;
        private Color fillColor;
        private Color handleColor;
        private Slider.Direction direction;

        private float value;
        private float minValue;
        private float maxValue;

        private bool wholeNumbers;
        private bool interactable;

        public event EventHandler<SliderValueChangedEventArgs> OnValueChanged;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                backgroundPanel.BackgroundColor = value;
            }
        }

        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                fillColor = value;
                fill.BackgroundColor = value;
            }
        }

        public Color HandleColor
        {
            get
            {
                return handleColor;
            }
            set
            {
                handleColor = value;
                handle.BackgroundColor = value;
            }
        }

        public Slider.Direction Direction
        {
            get
            {
                return direction;
            }
            set
            {
                if (value != Slider.Direction.BottomToTop && value != Slider.Direction.TopToBottom)
                    throw new Exception("Vertical slider only accepts a BottomToTop/TopToBottom direction!");

                direction = value;
                slider.direction = value;
            }
        }

        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                slider.value = value;
            }
        }

        public float MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
                slider.minValue = value;
            }
        }

        public float MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
                slider.maxValue = value;
            }
        }

        public bool WholeNumbers
        {
            get
            {
                return wholeNumbers;
            }
            set
            {
                wholeNumbers = value;
                slider.wholeNumbers = value;
            }
        }

        public bool Interactable
        {
            get
            {
                return interactable;
            }
            set
            {
                interactable = value;
                slider.interactable = value;
            }
        }

        public VerticalSlider() : this("Slider") { }

        public VerticalSlider(string name) : base(name)
        {
            PointerHandler pointerHandler = root.AddComponent<PointerHandler>();
            pointerHandler.PointerUp += PointerHandler_PointerUp;

            slider = root.AddComponent<Slider>();
            slider.direction = Slider.Direction.BottomToTop;

            backgroundPanel = new Panel("Background");
            AddChild(backgroundPanel);
            backgroundPanel.BackgroundColor = Color.gray;
            backgroundColor = Color.gray;
            backgroundPanel.SizeScaleX = 1f;
            backgroundPanel.SizeScaleY = 1f;

            fillArea = new Panel("Fill Area");
            AddChild(fillArea);
            fillArea.BackgroundColor = Color.clear;
            fillArea.SizeOffsetY = SizeOffsetY;

            fill = new Panel("Fill");
            fillArea.AddChild(fill);
            fill.BackgroundColor = Color.black;
            fillColor = Color.black;
            fill.SizeOffsetX = SizeOffsetX;

            handleArea = new Panel("Handle Area");
            AddChild(handleArea);
            handleArea.BackgroundColor = Color.clear;
            handleArea.SizeOffsetY = SizeOffsetY;

            handle = new Panel("Handle");
            handleArea.AddChild(handle);
            handle.BackgroundColor = Color.black;
            handleColor = Color.black;
            handle.SizeOffsetX = SizeOffsetX * 1.5f;
            handle.SizeOffsetY = SizeOffsetX * 1.5f;
            handle.PositionOffsetX = (SizeOffsetX - handle.SizeOffsetX) / 2;
            handle.PositionOffsetY = handle.SizeOffsetX / -2;

            slider.fillRect = fill.GetRootObject().GetComponent<RectTransform>();
            slider.handleRect = handle.GetRootObject().GetComponent<RectTransform>();
            slider.targetGraphic = handle.GetRootObject().GetComponent<Image>();

            interactable = true;
        }

        private void PointerHandler_PointerUp(object sender, System.EventArgs e)
        {
            value = slider.value;
            OnValueChanged?.Invoke(this, new SliderValueChangedEventArgs(value));
        }

        protected override void UpdateDirtyTransform()
        {
            base.UpdateDirtyTransform();
            value = slider.value;
            fillArea.SizeOffsetY = Height;
            fill.SizeOffsetX = Width;
            handleArea.SizeOffsetY = Height;
            handle.SizeOffsetX = Width * 1.5f;
            handle.SizeOffsetY = Width * 1.5f;
            handle.PositionOffsetX = (Width - handle.SizeOffsetX) / 2;
            handle.PositionOffsetY = handle.SizeOffsetX / -2;
        }
    }
}
