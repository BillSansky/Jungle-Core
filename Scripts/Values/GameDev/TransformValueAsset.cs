using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform value", fileName = "TransformLocalValue")]
    [JungleClassInfo("Transform Value Asset", "ScriptableObject storing a transform component.", null, "Values/Game Dev")]
    public class TransformValueAsset : ValueAsset<Transform>
    {
        [SerializeField]
        private Transform value;

        public override Transform Value()
        {
            return value;
        }

        public override void SetValue(Transform value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform list value", fileName = "TransformListValue")]
    [JungleClassInfo("Transform List Asset", "ScriptableObject storing a list of transforms.", null, "Values/Game Dev")]
    public class TransformListValueAsset : SerializedValueListAsset<Transform>
    {
    }

    [Serializable]
    [JungleClassInfo("Transform Value From Asset", "Reads a transform component from a TransformValueAsset.", null, "Values/Game Dev")]
    public class TransformValueFromAsset :
        ValueFromAsset<Transform, TransformValueAsset>, ITransformValue
    {
    }

    [Serializable]
    [JungleClassInfo("Transform List From Asset", "Reads transforms from a TransformListValueAsset.", null, "Values/Game Dev")]
    public class TransformListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Transform>, TransformListValueAsset>
    {
    }
}
