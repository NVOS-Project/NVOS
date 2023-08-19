using System;

namespace NVOS.UI.Models.EventArgs
{
    public class WindowEventArgs : System.EventArgs
    {
        public Window Window;

        public WindowEventArgs(Window window)
        {
            Window = window ?? throw new ArgumentNullException(nameof(window));
        }
    }
}
