using NVOS.UI.Models.Enums;
using System;

namespace NVOS.UI.Models.EventArgs
{
    public class WindowStateChangedEventArgs : System.EventArgs
    {
        public Window Window;
        public WindowState State;

        public WindowStateChangedEventArgs(Window window, WindowState state)
        {
            Window = window ?? throw new ArgumentNullException(nameof(window));
            State = state;
        }
    }
}
