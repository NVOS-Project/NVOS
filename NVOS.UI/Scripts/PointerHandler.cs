using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NVOS.UI.Scripts
{
    public class PointerHandler : MonoBehaviour, IPointerUpHandler
    {
        public event EventHandler PointerUp;

#if IL2CPP_BUILD
        public PointerHandler(IntPtr ptr) : base(ptr) { }
#endif

        public void OnPointerUp(PointerEventData eventData)
        {
            PointerUp?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
