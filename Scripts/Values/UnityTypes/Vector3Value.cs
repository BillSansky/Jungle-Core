using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector3Value : IValue<Vector3>
    {
    }

    [Serializable]
    public class Vector3Value : LocalValue<Vector3>, IVector3Value
    {
    }
}
