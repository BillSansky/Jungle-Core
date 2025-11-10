﻿using System;
using System.Collections.Generic;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values
{
    /// <summary>
    /// Defines the component reference interface.
    /// </summary>
    public interface IComponentReference
    {
        Component C => Component;
        Component Component { get; }
    }
    /// <summary>
    /// Represents a value provider that returns a TComponent.
    /// </summary>
    
    public interface IComponent<out TComponent> : IValue<TComponent>,IComponentReference, IGameObjectReference
        where TComponent : Component
    {
        Component IComponentReference.Component => V;
        GameObject IGameObjectReference.GameObject => ((IValue<TComponent>)this).Value().gameObject;
    }
}