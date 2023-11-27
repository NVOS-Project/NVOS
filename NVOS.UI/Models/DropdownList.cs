using NVOS.UI.Models.EventArgs;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NVOS.UI.Models
{
    public class DropdownList : Control
    {
        public SwitchButton DropdownSwitch { get; }
        private VerticalLayoutPanel dropdownPanel;
        private ScrollView dropdownScrollView;

        private float listHeight;
        private Color listColor;

        public float ListHeight
        {
            get
            {
                return listHeight;
            }
            set
            {
                listHeight = value;
                dropdownScrollView.SizeOffsetY = value;
            }
        }

        public Color ListColor
        {
            get
            {
                return listColor;
            }
            set
            {
                listColor = value;
                dropdownPanel.BackgroundColor = value;
            }
        }

        public DropdownList() : this("Dropdown")
        { }

        public DropdownList(string name) : base(name)
        {
            DropdownSwitch = new SwitchButton("Dropdown Switch");
            DropdownSwitch.SizeScaleX = 1f;
            DropdownSwitch.SizeScaleY = 1f;
            DropdownSwitch.OnValueChanged += DropdownSwitch_OnValueChanged;
            DropdownSwitch.Label.FontSize = 1f;
            AddChild(DropdownSwitch);

            dropdownPanel = new VerticalLayoutPanel("Dropdown Panel");
            dropdownPanel.VerticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize;
            dropdownPanel.ControlChildWidth = true;
            dropdownPanel.ControlChildHeight = false;
            dropdownPanel.SizeScaleX = 1f;
            dropdownPanel.BackgroundColor = Color.white;
            listColor = Color.white;

            dropdownScrollView = new ScrollView("Dropdown ScrollView", dropdownPanel);
            dropdownScrollView.PositionScaleY = 1f;
            dropdownScrollView.SizeScaleX = 1f;
            dropdownScrollView.SizeOffsetY = 1f;
            listHeight = 1f;
            AddChild(dropdownScrollView);
            dropdownScrollView.IsVisible = false;
        }

        public void AddListItem(Control item)
        {
            dropdownPanel.AddChild(item);
        }

        public void ClearListItems(Control item)
        {
            foreach (Control control in dropdownPanel.controls)
            {
                dropdownPanel.controls.Remove(control);
                control.Dispose();
            }
        }

        private void DropdownSwitch_OnValueChanged(object sender, SwitchButtonValueChangedEventArgs e)
        {
            dropdownScrollView.IsVisible = e.Value;
        }
    }
}
