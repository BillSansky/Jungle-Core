using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IRectValue : IValue<Rect>
    {
    }

    [Serializable]
    public class RectValue : LocalValue<Rect>, IRectValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
