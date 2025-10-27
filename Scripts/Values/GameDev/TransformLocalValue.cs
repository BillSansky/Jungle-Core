using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ITransformValue : IValue<Transform>
    {
        public Vector3 Position => Value().position;
        public Vector3 LocalScale => Value().localScale;
        public Quaternion Rotation => Value().rotation;
        
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
