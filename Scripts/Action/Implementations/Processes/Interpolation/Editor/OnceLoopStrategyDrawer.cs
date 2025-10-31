using UnityEditor;
using UnityEngine.UIElements;

namespace Jungle.Actions
{
    /// <summary>
    /// Renders the inspector UI for configuring the single-run loop strategy.
    /// </summary>
    [CustomPropertyDrawer(typeof(OnceLoopStrategy))]
    public class OnceLoopStrategyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Builds the inspector UI for the once-only loop strategy, explaining its behavior.
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
