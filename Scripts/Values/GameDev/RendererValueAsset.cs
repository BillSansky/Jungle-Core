﻿using Jungle.Values;
using System;
using UnityEngine;

namespace Jungle.Values
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity", fileName = "RendererValueAsset")]
    public class RendererValueAsset : ValueAsset<Renderer>
    {
        [SerializeField]
        private Renderer value;

        public override Renderer Value()
        {
            return value;
        }
    }

    [Serializable]
    public class RendererValueFromAsset : ValueFromAsset<Renderer, RendererValueAsset>, IRendererValue
    {
    }
}
