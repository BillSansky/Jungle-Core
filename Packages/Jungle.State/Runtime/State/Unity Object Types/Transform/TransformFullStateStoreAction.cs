using System;
using Jungle.Attributes;
using Jungle.Utility;

namespace Jungle.Actions
{
    /// <summary>
    /// Stores full transform state including position, rotation, and scale for later restoration.
    /// </summary>
    [Serializable]
    [JungleClassInfo(
        "Transform State Store Action",
        "Captures the position, rotation, and scale of the target transforms so they can be restored later.",
        "d_Transform Icon",
        "Actions/Transform")]
    public class TransformFullStateStoreAction : TransformStateStoreActionBase<TransformFullState>
    {
    }
}
