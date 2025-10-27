using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ITransformValue : IComponent<Transform>
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
}
