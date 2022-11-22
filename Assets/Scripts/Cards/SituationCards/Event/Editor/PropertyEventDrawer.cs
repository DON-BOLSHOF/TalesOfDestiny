using Cards.SituationCards.Event.PropertyEvents;
using UnityEditor;
using UnityEngine;
using Utils.Editor;

namespace Cards.SituationCards.Event.Editor
{
    [CustomPropertyDrawer(typeof(PropertyEvent))]
    public class PropertyEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var rect = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label);
            EditorGUI.indentLevel += 1;

            var modeProperty = property.FindPropertyRelative("_mode");

            if (property.isExpanded)
            {
                int level = 1;
                var unitRect = new Rect(position.min.x, position.min.y + EditorGUIUtility.singleLineHeight * level++,
                    position.size.x, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(unitRect, modeProperty);

                if (modeProperty.GetEnum(out PropertyEvent.PropertyMode mode))
                {
                    switch (mode)
                    {
                        case PropertyEvent.PropertyMode.Bound:
                            var subProp = property.FindPropertyRelative("_data");
                            SetNewField(subProp.FindPropertyRelative("_food"), position, ref level);
                            SetNewField(subProp.FindPropertyRelative("_coin"), position, ref level);
                            SetNewField(subProp.FindPropertyRelative("_prestige"), position, ref level);
                            break;
                        case PropertyEvent.PropertyMode.External:
                            SetNewField(property.FindPropertyRelative("_def"), position, ref level);
                            break;
                    }
                }
            }

            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }
        
        private void SetNewField(SerializedProperty property, Rect position, ref int level)
        {
            var nameRect = new Rect(position.min.x,
                position.min.y + EditorGUIUtility.singleLineHeight * level++,
                position.size.x, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(nameRect, property);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;

            if (property.isExpanded)
            {
                var modeProperty = property.FindPropertyRelative("_mode");

                if (modeProperty.GetEnum(out PropertyEvent.PropertyMode mode))
                {
                    switch (mode)
                    {
                        case PropertyEvent.PropertyMode.Bound:
                            totalLines+=4;
                            break;
                        case PropertyEvent.PropertyMode.External:
                            totalLines+=2;
                            break;
                    }
                }
            }

            return EditorGUIUtility.singleLineHeight * totalLines +
                   EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
        }
    }
}