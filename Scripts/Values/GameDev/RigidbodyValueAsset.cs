using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Rigidbody value", fileName = "RigidbodyValue")]
    [JungleClassInfo("Rigidbody Value Asset", "ScriptableObject storing a rigidbody component.", null, "Values/Game Dev")]
    public class RigidbodyValueAsset : ValueAsset<Rigidbody>
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Rigidbody list value", fileName = "RigidbodyListValue")]
    [JungleClassInfo("Rigidbody List Asset", "ScriptableObject storing a list of rigidbodies.", null, "Values/Game Dev")]
    public class RigidbodyListValueAsset : SerializedValueListAsset<Rigidbody>
    {
    }

    [Serializable]
    [JungleClassInfo("Rigidbody Value From Asset", "Reads a rigidbody component from a RigidbodyValueAsset.", null, "Values/Game Dev")]
    public class RigidbodyValueFromAsset :
        ValueFromAsset<Rigidbody, RigidbodyValueAsset>, IRigidbodyValue
    {
    }

    [Serializable]
    [JungleClassInfo("Rigidbody List From Asset", "Reads rigidbodies from a RigidbodyListValueAsset.", null, "Values/Game Dev")]
    public class RigidbodyListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Rigidbody>, RigidbodyListValueAsset>
    {
    }
}
