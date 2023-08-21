using NVOS.Core;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NVOS.Unity
{
    [ServiceType(ServiceType.Singleton)]
    public class UpdateProviderService : IService, IDisposable
    {
        private bool isDisposed;
        private GameObject tickerObject;
        private UpdateProvider tickerScript;

        public event EventHandler OnUpdate;
        public event EventHandler OnFixedUpdate;
        public event EventHandler OnLateUpdate;

        public void Init()
        {
            tickerObject = new GameObject();
            tickerScript = tickerObject.AddComponent<UpdateProvider>();
            tickerScript.OnUpdate += TickerScript_OnUpdate;
            tickerScript.OnFixedUpdate += TickerScript_OnFixedUpdate;
            tickerScript.OnLateUpdate += TickerScript_OnLateUpdate;
        }

        private void TickerScript_OnUpdate(object sender, EventArgs e)
        {
            OnUpdate?.Invoke(this, EventArgs.Empty);
        }

        private void TickerScript_OnFixedUpdate(object sender, EventArgs e)
        {
            OnFixedUpdate?.Invoke(this, EventArgs.Empty);
        }

        private void TickerScript_OnLateUpdate(object sender, EventArgs e)
        {
            OnLateUpdate?.Invoke(this, EventArgs.Empty);
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            tickerScript.OnUpdate -= TickerScript_OnUpdate;
            tickerScript.OnFixedUpdate -= TickerScript_OnFixedUpdate;
            tickerScript.OnLateUpdate -= TickerScript_OnLateUpdate;

            GameObject.Destroy(tickerObject);
            tickerObject = null;
            tickerScript = null;
            isDisposed = true;
        }
    }
}
