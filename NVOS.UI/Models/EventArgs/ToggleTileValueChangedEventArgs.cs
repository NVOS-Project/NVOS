using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.UI.Models.EventArgs
{
    public class ToggleTileValueChangedEventArgs
    {
        public ToggleTile ToggleTile;
        public bool IsChecked;

        public ToggleTileValueChangedEventArgs(ToggleTile toggleTile, bool isChecked)
        {
            ToggleTile = toggleTile ?? throw new ArgumentNullException();
            IsChecked = isChecked;
        }
    }
}
