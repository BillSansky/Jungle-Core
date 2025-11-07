using UnityEditor;
using UnityEngine.UIElements;

namespace Jungle.Actions
{
    /// <summary>
    /// Custom property drawer that explains the Once loop strategy.
    /// </summary>
    [CustomPropertyDrawer(typeof(OnceLoopStrategy))]
    public class OnceLoopStrategyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Builds the inspector UI for the Once loop strategy entry.
        /// </summary>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var helpBox = new HelpBox(
                "This loop strategy executes once and then stops.",
                HelpBoxMessageType.Info
            );

            container.Add(helpBox);

            return container;
        }
    }
}
