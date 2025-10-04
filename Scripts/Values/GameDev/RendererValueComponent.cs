using Jungle.Values;
using System;
using UnityEngine;

namespace Jungle.Values
{
    public class RendererValueComponent : ValueComponent<Renderer>
    {
        [SerializeField]
        private Renderer value;

        public override Renderer Value()
        {
            return value;
        }
    }

    [Serializable]
    public class RendererValueFromComponent : ValueFromComponent<Renderer, RendererValueComponent>, IRendererValue
    {
    }
}
