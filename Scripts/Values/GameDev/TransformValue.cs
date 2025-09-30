using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ITransformValue : IValue<Transform>
    {
    }

    [Serializable]
    public class TransformValue : LocalValue<Transform>, ITransformValue
    {
    }
}
