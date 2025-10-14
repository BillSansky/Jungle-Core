using UnityEditor;
using UnityEngine.UIElements;

namespace Jungle.Actions
{
    [CustomPropertyDrawer(typeof(OnceLoopStrategy))]
    public class OnceLoopStrategyDrawer : PropertyDrawer
    {
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
