using System;
using System.Reflection;
using UnityEngine;

namespace Jungle.Values
{
    [Serializable]
    public class MethodInvokerValue<T> : IValue<T>
    {
        [SerializeField] private Component component;
        [SerializeField] private string methodName;

        private Func<T> cachedFunc;
        private bool isInitialized;

        public Component Component
        {
            get => component;
            set
            {
                component = value;
                isInitialized = false;
                cachedFunc = null;
            }
        }

        public string MethodName
        {
            get => methodName;
            set
            {
                methodName = value;
                isInitialized = false;
                cachedFunc = null;
            }
        }

        public bool HasMultipleValues => false;

        public object GetValue()
        {
            return Value();
        }

        public T Value()
        {
            if (!isInitialized)
            {
                InitializeAction();
            }

            if (cachedFunc != null)
            {
                return cachedFunc.Invoke();
            }

            return default(T);
        }

        private void InitializeAction()
        {
            isInitialized = true;
            cachedFunc = null;

            if (component == null)
            {
                Debug.LogWarning("MethodInvokerValue: Ref is null");
                return;
            }

            if (string.IsNullOrEmpty(methodName))
            {
                Debug.LogWarning("MethodInvokerValue: Method name is empty");
                return;
            }

            Type componentType = component.GetType();
            MethodInfo methodInfo = componentType.GetMethod(
                methodName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null
            );

            if (methodInfo == null)
            {
                Debug.LogWarning($"MethodInvokerValue: Method '{methodName}' not found on {componentType.Name}");
                return;
            }

            if (methodInfo.ReturnType != typeof(T))
            {
                Debug.LogWarning($"MethodInvokerValue: Method '{methodName}' must return type {typeof(T).Name}, but returns {methodInfo.ReturnType.Name}");
                return;
            }

            try
            {
                cachedFunc = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), component, methodInfo);
            }
            catch (Exception e)
            {
                Debug.LogError($"MethodInvokerValue: Failed to create delegate for method '{methodName}': {e.Message}");
            }
        }

        public void ResetCache()
        {
            isInitialized = false;
            cachedFunc = null;
        }
    }
}
