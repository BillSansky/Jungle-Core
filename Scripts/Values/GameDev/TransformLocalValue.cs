using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ITransformValue : IValue<Transform>
    {
        
    }

    [Serializable]
    public class TransformLocalValue : LocalValue<Transform>, ITransformValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
