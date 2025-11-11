using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Lookup strategy for finding components on GameObjects.
    /// </summary>
    public enum ComponentLookupStrategy
    {
        /// <summary>
        /// Get component directly on the GameObject.
        /// </summary>
        OnObject,
        /// <summary>
        /// Get component on the GameObject or any of its children.
        /// </summary>
        InChildren,
        /// <summary>
        /// Get component on the GameObject's parent.
        /// </summary>
        InParent
    }

    /// <summary>
    /// Base abstract class that retrieves a component from a GameObject using a specified lookup strategy.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to retrieve.</typeparam>
    /// <typeparam name="TInterface">Interface that this value implements.</typeparam>
    [Serializable]
    public abstract class BaseComponentLookupValue<TComponent, TInterface> : IValue<TComponent>
        where TComponent : Component
        where TInterface : IValue<TComponent>
    {
        [SerializeReference]
        [JungleClassSelection(typeof(IGameObjectValue))]
        protected IGameObjectValue targetObject = new GameObjectValue();

        [SerializeField]
        protected ComponentLookupStrategy lookupStrategy = ComponentLookupStrategy.OnObject;

        protected TComponent cachedComponent;
        protected readonly List<TComponent> cachedComponents = new();
        protected bool isInitialized;

        /// <summary>
        /// Gets the target GameObject provider.
        /// </summary>
        public IGameObjectValue TargetObject
        {
            get => targetObject;
            set
            {
                targetObject = value;
                ResetCache();
            }
        }

        /// <summary>
        /// Gets or sets the lookup strategy.
        /// </summary>
        public ComponentLookupStrategy LookupStrategy
        {
            get => lookupStrategy;
            set
            {
                lookupStrategy = value;
                ResetCache();
            }
        }

        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public virtual bool HasMultipleValues
        {
            get
            {
                if (!isInitialized)
                {
                    InitializeComponent();
                }

                return cachedComponents.Count > 1;
            }
        }

        /// <summary>
        /// Retrieves all component values.
        /// </summary>
        public virtual IEnumerable<TComponent> Values
        {
            get
            {
                if (!isInitialized)
                {
                    InitializeComponent();
                }

                return cachedComponents;
            }
        }

        /// <summary>
        /// Retrieves the component value.
        /// </summary>
        public TComponent Value()
        {
            if (!isInitialized)
            {
                InitializeComponent();
            }

            return cachedComponent;
        }

        /// <summary>
        /// Initializes and caches the component based on the lookup strategy.
        /// </summary>
        protected virtual void InitializeComponent()
        {
            isInitialized = true;
            cachedComponent = null;
            cachedComponents.Clear();

            if (targetObject == null)
            {
                Debug.LogWarning($"{GetType().Name}: Target object provider is null");
                return;
            }

            int providedObjects = 0;
            int nullReferences = 0;

            foreach (GameObject gameObject in targetObject.Values)
            {
                providedObjects++;

                if (gameObject == null)
                {
                    nullReferences++;
                    continue;
                }

                TComponent component = lookupStrategy switch
                {
                    ComponentLookupStrategy.OnObject => gameObject.GetComponent<TComponent>(),
                    ComponentLookupStrategy.InChildren => gameObject.GetComponentInChildren<TComponent>(),
                    ComponentLookupStrategy.InParent => gameObject.GetComponentInParent<TComponent>(),
                    _ => null
                };

                if (component != null)
                {
                    cachedComponents.Add(component);

                    if (cachedComponent == null)
                    {
                        cachedComponent = component;
                    }
                }
            }

            if (cachedComponent != null)
            {
                return;
            }

            if (providedObjects == 0)
            {
                Debug.LogWarning($"{GetType().Name}: Target object provider returned no GameObjects");
                return;
            }

            if (nullReferences == providedObjects)
            {
                Debug.LogWarning($"{GetType().Name}: Target object provider returned only null GameObject references");
                return;
            }

            Debug.LogWarning($"{GetType().Name}: Component of type {typeof(TComponent).Name} not found using strategy {lookupStrategy} across {providedObjects - nullReferences} GameObjects");
        }

        /// <summary>
        /// Clears the cached component reference.
        /// </summary>
        public void ResetCache()
        {
            isInitialized = false;
            cachedComponent = null;
            cachedComponents.Clear();
        }
    }
}
