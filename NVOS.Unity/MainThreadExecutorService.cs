using NVOS.Core;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using NVOS.Unity.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NVOS.Unity
{
    [ServiceType(ServiceType.Singleton)]
    public class MainThreadExecutorService : IService, IDisposable
    {
        private bool isDisposed;
        private GameObject executorObject;
        private MainThreadExecutor executorScript;

        public void Init()
        {
            executorObject = new GameObject();
            executorScript = executorObject.AddComponent<MainThreadExecutor>();
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            GameObject.Destroy(executorObject);
            executorObject = null;
            executorScript = null;
            isDisposed = true;
        }

        public void Execute(IEnumerator action)
        {
            executorScript.Execute(action);
        }

        public void Execute(Action action)
        {
            executorScript.Execute(action);
        }

        public Task ExecuteAsync(Action action)
        {
            return executorScript.ExecuteAsync(action);
        }
    }
}
