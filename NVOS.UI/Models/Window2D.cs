using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVOS.UI.Models
{
    public class Window2D : Window
    {
        private int tileWidth;
        private int tileHeight;
        
        public int TileWidth { get { return tileWidth; } }
        public int TileHeight { get { return tileHeight; } }

        public Window2D(string name, int tileWidth, int tileHeight) : base(name)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }
    }
}
