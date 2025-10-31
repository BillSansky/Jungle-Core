using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector3IntValue : IValue<Vector3Int>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector3Int Value", "Stores a 3D integer vector locally on the owner.", null, "Values/Unity Types", true)]
    public class Vector3IntValue : LocalValue<Vector3Int>, IVector3IntValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Vector3Int Member Value", "Returns a 3D integer vector from a component field, property, or method.", null, "Values/Unity Types")]
    public class Vector3IntClassMembersValue : ClassMembersValue<Vector3Int>, IVector3IntValue
    {
    }
}
