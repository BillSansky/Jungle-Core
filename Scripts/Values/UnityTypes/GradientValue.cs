using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IGradientValue : IValue<Gradient>
    {
    }

    [Serializable]
    public class GradientValue : LocalValue<Gradient>, IGradientValue
    {
    }
}
