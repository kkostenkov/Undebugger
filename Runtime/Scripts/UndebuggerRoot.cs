#if (UNITY_EDITOR || DEBUG || UNDEBUGGER) && !UNDEBUGGER_DISABLE
#define UNDEBUGGER_ENABLED
#endif

using System.Runtime.CompilerServices;
using UnityEngine;

namespace Undebugger
{
    public static class UndebuggerRoot
    {
        public const string Version = "1.0.0";

#if UNDEBUGGER_ENABLED

        public static GameObject Object;
        public static Transform Transform;

        public static void Initialize()
        {
            GameObject.Destroy(UndebuggerRoot.Object);
            UndebuggerRoot.Transform = null;
            
            Object = new GameObject("Undebugger");
            Object.hideFlags = HideFlags.NotEditable;
            Transform = Object.transform;
            GameObject.DontDestroyOnLoad(Object);
        }

        public static void Destroy()
        {
            GameObject.Destroy(UndebuggerRoot.Object);
            UndebuggerRoot.Transform = null;

        }

        public static T CreateServiceInstance<T>(string name)
            where T : MonoBehaviour
        {
            var serviceObject = new GameObject(name);
            serviceObject.hideFlags = HideFlags.NotEditable;
            serviceObject.transform.SetParent(Transform);

            return serviceObject.AddComponent<T>();
        }

#endif

    }
}


