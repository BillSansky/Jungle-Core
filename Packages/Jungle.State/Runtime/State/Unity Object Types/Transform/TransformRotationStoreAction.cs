using System;
using Jungle.Attributes;
using Jungle.Utility;

namespace Jungle.Actions
{
    /// <summary>
    /// Stores only the world rotation of the target transforms.
    /// </summary>
    [Serializable]
    [JungleClassInfo(
        "Transform Rotation Store Action",
        "Captures the world rotation of the target transforms.",
        "d_RotateTool Icon",
        "Actions/State")]
    public class TransformRotationStoreAction : TransformStateStoreActionBase<TransformRotationState>
    {
    }
}
