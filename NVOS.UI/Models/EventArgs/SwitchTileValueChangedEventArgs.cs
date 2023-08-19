using System;

namespace NVOS.UI.Models.EventArgs
{
    public class SwitchTileValueChangedEventArgs : System.EventArgs
    {
        public SwitchTile SwitchTile;
        public bool IsOn;

        public SwitchTileValueChangedEventArgs(SwitchTile switchTile, bool isOn)
        {
            SwitchTile = switchTile ?? throw new ArgumentNullException(nameof(switchTile));
            IsOn = isOn;
        }
    }
}
