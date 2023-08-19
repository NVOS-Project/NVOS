using System;

namespace NVOS.UI.Models.EventArgs
{
    public class SwitchButtonValueChangedEventArgs : System.EventArgs
    {
        public SwitchButton SwitchButton;
        public bool IsOn;

        public SwitchButtonValueChangedEventArgs(SwitchButton switchButton, bool isOn)
        {
            SwitchButton = switchButton ?? throw new ArgumentNullException(nameof(switchButton));
            IsOn = isOn;
        }
    }
}
