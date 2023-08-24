using System;

namespace NVOS.UI.Models.EventArgs
{
    public class SwitchTileValueChangedEventArgs : System.EventArgs
    {
        public SwitchTile SwitchTile;
        public bool Value;

        public SwitchTileValueChangedEventArgs(SwitchTile switchTile, bool value)
        {
            SwitchTile = switchTile ?? throw new ArgumentNullException(nameof(switchTile));
            Value = value;
        }
    }
}
