using System;

namespace NVOS.UI.Models.EventArgs
{
    public class ToggleValueChangedEventArgs : System.EventArgs
    {
        public Toggle Toggle;
        public bool Value;

        public ToggleValueChangedEventArgs(Toggle toggle, bool value)
        {
            Toggle = toggle ?? throw new ArgumentNullException(nameof(toggle));
            Value = value;
        }
    }
}
