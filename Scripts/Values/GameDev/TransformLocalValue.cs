using System;
using System.Collections.Generic;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ITransformValue : IComponent<Transform>
    {
      
        public Vector3 Position => ((IValue<Transform>)this).Value().position;
        public Vector3 LocalScale => ((IValue<Transform>)this).Value().localScale;
        public Quaternion Rotation => ((IValue<Transform>)this).Value().rotation;
        
    }

    [Serializable]
    public class TransformLocalValue : LocalValue<Transform>, ITransformValue
    {
        public override bool HasMultipleValues => false;

       
    }

    [Serializable]
    public class TransformMethodInvokerValue : MethodInvokerValue<Transform>, ITransformValue
    {
    }
}
