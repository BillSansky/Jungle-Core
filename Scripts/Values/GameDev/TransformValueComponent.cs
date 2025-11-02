using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Transform Value Component", "Component exposing a transform component.", null, "Values/Game Dev")]
    public class TransformValueComponent : ValueComponent<Transform>
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

    [JungleClassInfo("Transform List Component", "Component exposing a list of transforms.", null, "Values/Game Dev")]
    public class TransformListValueComponent : SerializedValueListComponent<Transform>
    {
    }

    [Serializable]
    [JungleClassInfo("Transform Value From Component", "Reads a transform component from a TransformValueComponent.", null, "Values/Game Dev")]
    public class TransformValueFromComponent :
        ValueFromComponent<Transform, TransformValueComponent>, ITransformValue
    {
    }

    [Serializable]
    [JungleClassInfo("Transform List From Component", "Reads transforms from a TransformListValueComponent.", null, "Values/Game Dev")]
    public class TransformListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Transform>, TransformListValueComponent>
    {
    }
}
