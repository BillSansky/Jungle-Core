using Jungle.Physics;
using UnityEditor;
using UnityEngine;

namespace Jungle.Values.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(CollisionLayerMatrix))]
    public class CollisionLayerMatrixDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
            {
                return EditorGUIUtility.singleLineHeight;
            }

            return EditorGUIUtility.singleLineHeight * (CollisionLayerMatrix.LayerCount + 1);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = EditorGUIUtility.singleLineHeight;
            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
            if (!property.isExpanded)
            {
                return;
            }

            EditorGUI.indentLevel++;
            SerializedProperty collisionsProperty = property.FindPropertyRelative("collisions");
            EnsureCollisionArray(collisionsProperty);

            string[] layerNames = BuildLayerNames();
            float lineHeight = EditorGUIUtility.singleLineHeight;
            position.y += lineHeight;

            for (int layer = 0; layer < CollisionLayerMatrix.LayerCount; layer++)
            {
                string labelText = layerNames[layer];
                Rect rowRect = new Rect(position.x, position.y, position.width, lineHeight);

                int currentMask = GetLayerMask(collisionsProperty, layer);

                EditorGUI.BeginChangeCheck();
                int newMask = EditorGUI.MaskField(rowRect, labelText, currentMask, layerNames);
                if (EditorGUI.EndChangeCheck())
                {
                    ApplyMask(collisionsProperty, layer, newMask);
                }

                position.y += lineHeight;
            }

            EditorGUI.indentLevel--;
        }

        private static void ApplyMask(SerializedProperty collisionsProperty, int layer, int mask)
        {
            for (int targetLayer = 0; targetLayer < CollisionLayerMatrix.LayerCount; targetLayer++)
            {
                bool collides = (mask & (1 << targetLayer)) != 0;
                SetCollision(collisionsProperty, layer, targetLayer, collides);
                SetCollision(collisionsProperty, targetLayer, layer, collides);
            }
        }

        private static int GetLayerMask(SerializedProperty collisionsProperty, int layer)
        {
            int mask = 0;
            for (int targetLayer = 0; targetLayer < CollisionLayerMatrix.LayerCount; targetLayer++)
            {
                if (GetCollision(collisionsProperty, layer, targetLayer))
                {
                    mask |= 1 << targetLayer;
                }
            }

            return mask;
        }

        private static bool GetCollision(SerializedProperty collisionsProperty, int layerA, int layerB)
        {
            int index = GetIndex(layerA, layerB);
            return collisionsProperty.GetArrayElementAtIndex(index).boolValue;
        }

        private static void SetCollision(SerializedProperty collisionsProperty, int layerA, int layerB, bool collides)
        {
            int index = GetIndex(layerA, layerB);
            collisionsProperty.GetArrayElementAtIndex(index).boolValue = collides;
        }

        private static int GetIndex(int layerA, int layerB)
        {
            return layerA * CollisionLayerMatrix.LayerCount + layerB;
        }

        private static void EnsureCollisionArray(SerializedProperty collisionsProperty)
        {
            int requiredSize = CollisionLayerMatrix.LayerCount * CollisionLayerMatrix.LayerCount;
            if (collisionsProperty.arraySize != requiredSize)
            {
                collisionsProperty.arraySize = requiredSize;
                for (int layer = 0; layer < CollisionLayerMatrix.LayerCount; layer++)
                {
                    SetCollision(collisionsProperty, layer, layer, true);
                }
            }
        }

        private static string[] BuildLayerNames()
        {
            string[] names = new string[CollisionLayerMatrix.LayerCount];
            for (int layer = 0; layer < CollisionLayerMatrix.LayerCount; layer++)
            {
                string layerName = LayerMask.LayerToName(layer);
                names[layer] = string.IsNullOrEmpty(layerName) ? $"Layer {layer}" : layerName;
            }

            return names;
        }
    }
}
