using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NVOS.UI.Scripts
{
    public class PointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event EventHandler<System.EventArgs> PointerEnter;
        public event EventHandler<System.EventArgs> PointerExit;

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            PointerEnter?.Invoke(this, System.EventArgs.Empty);
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            PointerExit?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
