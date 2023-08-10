using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.UI.Models.EventArgs
{
    public class SwitchTileValueChangedEventArgs : System.EventArgs
    {
        public SwitchTile SwitchTile;
        public bool IsChecked;

        public SwitchTileValueChangedEventArgs(SwitchTile switchTile, bool isChecked)
        {
            SwitchTile = switchTile ?? throw new ArgumentNullException(nameof(switchTile));
            IsChecked = isChecked;
        }
    }
}
