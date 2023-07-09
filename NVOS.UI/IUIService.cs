using NVOS.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.UI
{
    public interface IUIService
    {
        Window CreateWindow(string title);
    }
}
