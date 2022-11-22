using Cards.SituationCards.Event.ArmyEvents;
using UnityEditor;
using UnityEngine;

namespace Cards.SituationCards.Event.Editor
{
    [CustomPropertyDrawer(typeof(ArmyEvent))]
    public class ArmyEventDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var rect = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label);
            EditorGUI.indentLevel += 1;

            var type = property.FindPropertyRelative("_id");

            if (property.isExpanded)
            {
                int level = 1;
                var unitRect = new Rect(position.min.x, position.min.y + EditorGUIUtility.singleLineHeight * level++,
                    position.size.x, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(unitRect, type);
            }

            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;

            if (property.isExpanded)
            {
                totalLines++;
            }

            return EditorGUIUtility.singleLineHeight * totalLines +
                   EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
        }
    }
}