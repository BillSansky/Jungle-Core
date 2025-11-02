using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Material Value Component", "Component exposing a material reference.", null, "Values/Game Dev")]
    public class MaterialValueComponent : ValueComponent<Material>
    {
        [SerializeField]
        private Material value;

        public override Material Value()
        {
            return value;
        }

        public override void SetValue(Material value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Material List Component", "Component exposing a list of materials.", null, "Values/Game Dev")]
    public class MaterialListValueComponent : SerializedValueListComponent<Material>
    {
    }

    [Serializable]
    [JungleClassInfo("Material Value From Component", "Reads a material reference from a MaterialValueComponent.", null, "Values/Game Dev")]
    public class MaterialValueFromComponent :
        ValueFromComponent<Material, MaterialValueComponent>, IMaterialValue
    {
    }

    [Serializable]
    [JungleClassInfo("Material List From Component", "Reads materials from a MaterialListValueComponent.", null, "Values/Game Dev")]
    public class MaterialListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Material>, MaterialListValueComponent>
    {
    }
}
