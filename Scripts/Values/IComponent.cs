﻿using System;
using System.Collections.Generic;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values
{
    public interface IComponent<out TComponent> : IValue<TComponent>, IGameObjectReference
        where TComponent : Component
    {
        GameObject IGameObjectReference.GameObject => ((IValue<TComponent>)this).Value().gameObject;
    }
}