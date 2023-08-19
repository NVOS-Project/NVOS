using System;

namespace NVOS.UI.Models.EventArgs
{
    public class ToggleValueChangedEventArgs : System.EventArgs
    {
        public Toggle Toggle;
        public bool IsChecked;

        public ToggleValueChangedEventArgs(Toggle toggle, bool isChecked)
        {
            Toggle = toggle ?? throw new ArgumentNullException(nameof(toggle));
            IsChecked = isChecked;
        }
    }
}
