using System;
using System.Collections.Generic;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values
{
    public interface IComponent<out TComponent> : IValue<TComponent>
        where TComponent : Component
    {
       
        TComponent Ref => ((IValue<TComponent>)this).Value();
        
        IEnumerable<TComponent> Refs => ((IValue<TComponent>)this).Values;
        
        bool HasMultipleRefs => ((IValue<TComponent>)this).HasMultipleValues;
    }
}