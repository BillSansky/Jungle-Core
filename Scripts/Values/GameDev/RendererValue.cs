using Jungle.Values;
using System;
using UnityEngine;

namespace Jungle.Values
{
    public interface IRendererValue : IComponent<Renderer>
    {
    }

    [Serializable]
    public class RendererValue : LocalValue<Renderer>, IRendererValue
    {
        public override bool HasMultipleValues => false;
    }

    [Serializable]
    public class RendererMethodInvokerValue : MethodInvokerValue<Renderer>, IRendererValue
    {
    }
}
