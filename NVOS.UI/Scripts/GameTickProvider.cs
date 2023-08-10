using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NVOS.UI
{
    public class GameTickProvider : MonoBehaviour
    {
        public event EventHandler OnUpdate;
        public event EventHandler OnFixedUpdate;
        public event EventHandler OnLateUpdate;

        void Update()
        {
            OnUpdate?.Invoke(this, EventArgs.Empty);
        }

        void FixedUpdate()
        {
            OnFixedUpdate?.Invoke(this, EventArgs.Empty);
        }

        void LateUpdate()
        {
            OnLateUpdate?.Invoke(this, EventArgs.Empty);
        }
    }
}
