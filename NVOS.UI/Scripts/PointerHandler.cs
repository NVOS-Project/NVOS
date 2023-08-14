using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NVOS.UI.Scripts
{
    public class PointerHandler : MonoBehaviour, IPointerUpHandler
    {
        public event EventHandler PointerUp;

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
