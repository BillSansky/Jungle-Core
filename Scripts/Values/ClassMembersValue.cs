using System;
using System.Reflection;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Jungle.Values
{
    /// <summary>
    /// Resolves values by invoking a parameterless method or property on a component.
    /// </summary>
    [Serializable]
    public class ClassMembersValue<T> : IValue<T>
    {
        [SerializeReference] private IComponentReference component;
        [SerializeField] private string memberName;

        private Func<T> cachedFunc;
        private bool isInitialized;

        public Component Component => component.C;

        public string MethodName
        {
            get => memberName;
            set
            {
                memberName = value;
                isInitialized = false;
                cachedFunc = null;
            }
        }

        public bool HasMultipleValues => false;
        /// <summary>
        /// Returns the resolved member value using <see cref="Value"/>, boxed as an object.
        /// </summary>
        public object GetValue()
        {
            return Value();
        }
        /// <summary>
        /// Lazily resolves and invokes the configured member on the component.
        /// </summary>
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
        /// <summary>
        /// Builds the cached delegate that reads the requested method or property.
        /// </summary>
        private void InitializeAction()
        {
            isInitialized = true;
            cachedFunc = null;

            if (component == null)
            {
                Debug.LogWarning("ClassMembersValue: Ref is null");
                return;
            }

            if (string.IsNullOrEmpty(memberName))
            {
                Debug.LogWarning("ClassMembersValue: Method name is empty");
                return;
            }

            Type componentType = component.GetType();
            MethodInfo methodInfo = componentType.GetMethod(
                memberName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null
            );

            if (methodInfo != null)
            {
                if (methodInfo.ReturnType != typeof(T))
                {
                    Debug.LogWarning($"ClassMembersValue: Method '{memberName}' must return type {typeof(T).Name}, but returns {methodInfo.ReturnType.Name}");
                    return;
                }

                try
                {
                    cachedFunc = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), component, methodInfo);
                }
                catch (Exception e)
                {
                    Debug.LogError($"ClassMembersValue: Failed to create delegate for method '{memberName}': {e.Message}");
                }

                return;
            }

            PropertyInfo propertyInfo = componentType.GetProperty(
                memberName,
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
            );

            if (propertyInfo == null)
            {
                Debug.LogWarning($"ClassMembersValue: Member '{memberName}' not found on {componentType.Name}");
                return;
            }

            if (propertyInfo.PropertyType != typeof(T))
            {
                Debug.LogWarning($"ClassMembersValue: Property '{memberName}' must return type {typeof(T).Name}, but returns {propertyInfo.PropertyType.Name}");
                return;
            }

            MethodInfo getter = propertyInfo.GetGetMethod(true);

            if (getter == null || getter.GetParameters().Length != 0)
            {
                Debug.LogWarning($"ClassMembersValue: Property '{memberName}' does not have a parameterless getter");
                return;
            }

            try
            {
                cachedFunc = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), component, getter);
            }
            catch (Exception e)
            {
                Debug.LogError($"ClassMembersValue: Failed to create delegate for property '{memberName}': {e.Message}");
            }
        }
        /// <summary>
        /// Clears the cached delegate so the member lookup runs again on next access.
        /// </summary>
        public void ResetCache()
        {
            isInitialized = false;
            cachedFunc = null;
        }
    }
}
