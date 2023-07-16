using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.UI.Models.EventArgs
{
    public class WindowEventArgs : System.EventArgs
    {
        public Window Window;

        public WindowEventArgs(Window window)
        {
            Window = window ?? throw new ArgumentNullException();
        }
    }
}
