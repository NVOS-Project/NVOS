using NVOS.UI.Models.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class Toggle : Control
    {
        private Panel check;
        private Panel uncheck;
        private Image backgroundImage;
        private UnityEngine.UI.Toggle toggle;

        private Color backgroundColor;
        private Color checkColor;
        private Color uncheckColor;

        public event EventHandler<ToggleValueChangedEventArgs> OnValueChanged;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                backgroundImage.color = value;
            }
        }

        public Color CheckColor
        {
            get
            {
                return checkColor;
            }
            set
            {
                checkColor = value;
                check.BackgroundColor = value;
            }
        }

        public Color UncheckColor
        {
            get
            {
                return uncheckColor;
            }
            set
            {
                uncheckColor = value;
                uncheck.BackgroundColor = value;
            }
        }

        public Toggle() : this("Toggle") { }

        public Toggle(string name) : base(name)
        {
            SizeOffsetX = 0.1f;
            SizeOffsetY = 0.1f;

            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.gray;
            backgroundColor = Color.gray;

            uncheck = new Panel("Uncheck");
            AddChild(uncheck);
            uncheck.BackgroundColor = Color.white;
            uncheckColor = Color.white;

            uncheck.SizeScaleX = 0.7f;
            uncheck.SizeScaleY = 0.7f;
            uncheck.PositionScaleX = 0.15f;
            uncheck.PositionScaleY = 0.15f;

            check = new Panel("Check");
            AddChild(check);
            check.BackgroundColor = Color.black;
            checkColor = Color.black;

            check.SizeScaleX = 0.7f;
            check.SizeScaleY = 0.7f;
            check.PositionScaleX = 0.15f;
            check.PositionScaleY = 0.15f;

            toggle = root.AddComponent<UnityEngine.UI.Toggle>();
            toggle.image = uncheck.GetRootObject().GetComponent<Image>();
            toggle.graphic = check.GetRootObject().GetComponent<Image>();
            toggle.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<bool>(HandleClick));
        }

        private void HandleClick(bool isChecked)
        {
            OnValueChanged?.Invoke(this, new ToggleValueChangedEventArgs(this, isChecked)); 
        }
    }
}
