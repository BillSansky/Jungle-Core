using System;
using System.Reflection;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Reads a TComponent from a configured component member.
    /// </summary>
    [Serializable]
    public class ComponentClassMembersValue<TComponent> : ClassMembersValue<TComponent>
        where TComponent : Component
    {
        /// <summary>
        /// Specifies how component references are searched when resolving the value.
        /// </summary>
        public enum LookupMode
        {
            Direct,
            FindOnObject,
            FindOnChildren,
            FindOnParent
        }

        [SerializeField]
        private LookupMode lookupMode = LookupMode.Direct;

        protected override void InitializeAction()
        {
            if (lookupMode == LookupMode.Direct)
            {
                base.InitializeAction();
                return;
            }

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
                if (TryCreateResolvedFunc(methodInfo.ReturnType, () => methodInfo.Invoke(component, null), out var func))
                {
                    cachedFunc = func;
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

            MethodInfo getter = propertyInfo.GetGetMethod(true);

            if (getter == null || getter.GetParameters().Length != 0)
            {
                Debug.LogWarning($"ClassMembersValue: Property '{memberName}' does not have a parameterless getter");
                return;
            }

            if (TryCreateResolvedFunc(propertyInfo.PropertyType, () => getter.Invoke(component, null), out var propertyFunc))
            {
                cachedFunc = propertyFunc;
            }
        }

        private bool TryCreateResolvedFunc(Type memberReturnType, Func<object> valueProvider, out Func<TComponent> func)
        {
            func = null;

            if (typeof(TComponent).IsAssignableFrom(memberReturnType))
            {
                func = () => valueProvider() as TComponent;
                return true;
            }

            if (typeof(Component).IsAssignableFrom(memberReturnType))
            {
                func = () => ResolveFromComponent(valueProvider() as Component);
                return true;
            }

            if (typeof(GameObject).IsAssignableFrom(memberReturnType))
            {
                func = () => ResolveFromGameObject(valueProvider() as GameObject);
                return true;
            }

            Debug.LogWarning(
                $"ClassMembersValue: Member '{memberName}' must return {typeof(TComponent).Name}, Component, or GameObject when using lookup mode {lookupMode}, but returns {memberReturnType.Name}");
            return false;
        }

        private TComponent ResolveFromComponent(Component source)
        {
            if (source == null)
            {
                return null;
            }

            switch (lookupMode)
            {
                case LookupMode.FindOnChildren:
                    return source.GetComponentInChildren<TComponent>();
                case LookupMode.FindOnParent:
                    return source.GetComponentInParent<TComponent>();
                case LookupMode.FindOnObject:
                    return source.GetComponent<TComponent>();
                default:
                    return source as TComponent;
            }
        }

        private TComponent ResolveFromGameObject(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return null;
            }

            switch (lookupMode)
            {
                case LookupMode.FindOnChildren:
                    return gameObject.GetComponentInChildren<TComponent>();
                case LookupMode.FindOnParent:
                    return gameObject.GetComponentInParent<TComponent>();
                default:
                    return gameObject.GetComponent<TComponent>();
            }
        }
    }
}
