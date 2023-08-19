using System;
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
