using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.UI.Models.EventArgs
{
    public class ToggleValueChangedEventArgs
    {
        public Toggle Toggle;
        public bool IsChecked;

        public ToggleValueChangedEventArgs(Toggle toggle, bool isChecked)
        {
            Toggle = toggle ?? throw new ArgumentNullException();
        }
    }
}
