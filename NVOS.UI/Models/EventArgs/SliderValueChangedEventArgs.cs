using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.UI.Models.EventArgs
{
    public class SliderValueChangedEventArgs : System.EventArgs
    {
        public float Value;

        public SliderValueChangedEventArgs(float value)
        {
            Value = value;
        }
    }
}
