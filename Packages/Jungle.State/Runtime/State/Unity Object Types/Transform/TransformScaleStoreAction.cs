using System;
using Jungle.Attributes;
using Jungle.Utility;

namespace Jungle.Actions
{
    /// <summary>
    /// Stores only the local scale of the target transforms.
    /// </summary>
    [Serializable]
    [JungleClassInfo(
        "Transform Scale Store Action",
        "Captures the local scale of the target transforms.",
        "d_ScaleTool Icon",
        "Actions/State")]
    public class TransformScaleStoreAction : TransformStateStoreActionBase<TransformScaleState>
    {
    }
}
