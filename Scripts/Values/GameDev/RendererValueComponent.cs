using Jungle.Values;
using System;
using System.Collections.Generic;
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

        public override void SetValue(Renderer value)
        {
            this.value = value;
        }
    }

    public class RendererListValueComponent : SerializedValueListComponent<Renderer>
    {
    }

    [Serializable]
    public class RendererValueFromComponent : ValueFromComponent<Renderer, RendererValueComponent>, IRendererValue
    {
    }

    [Serializable]
    public class RendererListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Renderer>, RendererListValueComponent>
    {
    }
}
