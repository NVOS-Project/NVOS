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

        public Window2D(string title, int tileWidth, int tileHeight) : base(title)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }
    }
}
