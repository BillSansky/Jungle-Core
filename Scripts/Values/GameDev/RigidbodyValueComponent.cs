using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Rigidbody Value Component", "Component exposing a rigidbody component.", null, "Values/Game Dev")]
    public class RigidbodyValueComponent : ValueComponent<Rigidbody>
    {
        [SerializeField]
        private Rigidbody value;

        public override Rigidbody Value()
        {
            return value;
        }

        public override void SetValue(Rigidbody value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Rigidbody List Component", "Component exposing a list of rigidbodies.", null, "Values/Game Dev")]
    public class RigidbodyListValueComponent : SerializedValueListComponent<Rigidbody>
    {
    }

    [Serializable]
    [JungleClassInfo("Rigidbody Value From Component", "Reads a rigidbody component from a RigidbodyValueComponent.", null, "Values/Game Dev")]
    public class RigidbodyValueFromComponent :
        ValueFromComponent<Rigidbody, RigidbodyValueComponent>, IRigidbodyValue
    {
    }

    [Serializable]
    [JungleClassInfo("Rigidbody List From Component", "Reads rigidbodies from a RigidbodyListValueComponent.", null, "Values/Game Dev")]
    public class RigidbodyListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Rigidbody>, RigidbodyListValueComponent>
    {
    }
}
