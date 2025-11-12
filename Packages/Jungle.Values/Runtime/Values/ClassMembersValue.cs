using System;
using System.Reflection;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;
using Component = UnityEngine.Component;

namespace Jungle.Values
{
    
    
    
    
    /// <summary>
    /// Invokes a component member to produce a value.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Class Member Value", "Invokes a component member to produce a value.", null, "Core")]
    public class ClassMembersValue<T> : IValue<T>
    {
        [SerializeReference] protected IComponentValue component= new ComponentLocalValue();
        [SerializeField] protected string memberName;

        protected Func<T> cachedFunc;
        protected bool isInitialized;
       
        /// <summary>
        /// Gets the method name.
        /// </summary>

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
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public bool HasMultipleValues => false;
        /// <summary>
        /// Retrieves the value.
        /// </summary>

        public object GetValue()
        {
            return Value();
        }
        /// <summary>
        /// Gets the value produced by this provider.
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

        protected virtual void InitializeAction()
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
        /// Clears any cached member lookup state.
        /// </summary>

        public void ResetCache()
        {
            isInitialized = false;
            cachedFunc = null;
        }
    }
}
