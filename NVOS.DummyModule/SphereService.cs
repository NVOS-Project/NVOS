using NVOS.Core;
using NVOS.Core.Services.Attributes;
using NVOS.Core.Services.Enums;
using System;
using UnityEngine;

namespace NVOS.DummyModule
{
    [ServiceType(ServiceType.Singleton)]
    public class SphereService : IService, IDisposable
    {
        private GameObject gameObject;

        public bool Init()
        {
            gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gameObject.transform.position = new Vector3(0, 0, 5);
            return true;
        }

        public void Dispose()
        {
            if (gameObject == null)
                return;

            GameObject.Destroy(gameObject);
            gameObject = null;
        }
    }
}
