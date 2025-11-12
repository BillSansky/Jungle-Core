using System;
using Jungle.Attributes;
using Jungle.Utility;

namespace Jungle.Actions
{
    /// <summary>
    /// Stores only the world position of the target transforms.
    /// </summary>
    [Serializable]
    [JungleClassInfo(
        "Transform Position Store Action",
        "Captures the world position of the target transforms.",
        "d_GridAxis Icon",
        "Actions/Transform")]
    public class TransformPositionStoreAction : TransformStateStoreActionBase<TransformPositionState>
    {
    }
}
