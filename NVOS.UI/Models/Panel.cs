using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NVOS.UI.Models
{
    public class Panel : Control
    {
        private Image backgroundImage;
        private Color backgroundColor;

        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundImage.color = value;
                backgroundColor = value;
            }
        }

        public Panel() : this("Panel") { }

        public Panel(string name) : base(name) 
        {
            backgroundImage = root.AddComponent<Image>();
            backgroundImage.color = Color.clear;
            backgroundColor = Color.clear;
        }
    }
}
