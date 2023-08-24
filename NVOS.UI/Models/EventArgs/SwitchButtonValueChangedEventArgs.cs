using System;

namespace NVOS.UI.Models.EventArgs
{
    public class SwitchButtonValueChangedEventArgs : System.EventArgs
    {
        public SwitchButton SwitchButton;
        public bool Value;

        public SwitchButtonValueChangedEventArgs(SwitchButton switchButton, bool value)
        {
            SwitchButton = switchButton ?? throw new ArgumentNullException(nameof(switchButton));
            Value = value;
        }
    }
}
