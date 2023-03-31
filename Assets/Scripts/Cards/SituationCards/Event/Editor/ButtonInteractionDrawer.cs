using System;
using System.Linq;
using System.Reflection;
using Cards.SituationCards.Event.ArmyEvents;
using Cards.SituationCards.Event.PropertyEvents;
using UnityEditor;
using UnityEngine;
using Utils.Editor;

namespace Cards.SituationCards.Event.Editor
{
    //На будущее пользуйся фреймворком https://github.com/dbrizov/NaughtyAttributes, всю эту дичь со скрытиями
    //можно будет скрыть в пару атрибутов!!!
    [CustomPropertyDrawer(typeof(ButtonInteraction))]
    public class ButtonInteractionDrawer : PropertyDrawer
    {
        private readonly Assembly _assembly = Assembly.GetAssembly(typeof(ArmyEvent));

        private EventType GetFlagEnumValueFromIndex(int index)
        {
            return (EventType)index;
        }

        private Type GetType(SerializedProperty property)
        {
            var typeName = property.type;
            var ass = Assembly.LoadFrom(_assembly.CodeBase);

            return ass.GetType($"Cards.SituationCards.Event.{typeName}s.{typeName}");
        }

        private int GetSubFieldLength(SerializedProperty property)
        {
            if (property.isArray && property.arraySize <= 0)
                return 0;

            property = property.isArray ? property.GetArrayElementAtIndex(0) : property;

            var type = GetType(property);
            var size = type.GetFields(BindingFlags.NonPublic |
                                      BindingFlags.Instance).Length;

            if (property.type.Contains("Event"))
                size += 1;

            return size;
        }

        private void SetNewField(SerializedProperty property, Rect position, ref int level)
        {
            var nameRect = new Rect(position.min.x,
                position.min.y + EditorGUIUtility.singleLineHeight * level++,
                position.size.x, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(nameRect, property);

            if (property.isExpanded)
            {
                level++;

                var subPropertySize = GetSubFieldLength(property);

                if (property.isArray)
                {
                    level += property.arraySize;

                    for (int i = 0; i < property.arraySize; i++)
                    {
                        if (property.GetArrayElementAtIndex(i).isExpanded)
                        {
                            level += subPropertySize;
                        }
                    }

                    level++;
                }
            }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var rect = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label);
            EditorGUI.indentLevel += 1;

            var type = property.FindPropertyRelative("_type");

            if (property.isExpanded)
            {
                int level = 1;
                var unitRect = new Rect(position.min.x, position.min.y + EditorGUIUtility.singleLineHeight * level++,
                    position.size.x, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(unitRect, type);

                var flagEnum = GetFlagEnumValueFromIndex(type.intValue);

                if ((flagEnum & EventType.ArmyVisitor) == EventType.ArmyVisitor)
                {
                    SetNewField(property.FindPropertyRelative("_armyEvents"), position, ref level);
                }

                if ((flagEnum & EventType.PropertyVisitor) == EventType.PropertyVisitor)
                {
                    SetNewField(property.FindPropertyRelative("_propertyEvents"), position, ref level);
                }
                
                if ((flagEnum & EventType.Continue) == EventType.Continue)
                {
                    SetNewField(property.FindPropertyRelative("_futureSituation"), position, ref level);
                }
            }

            EditorGUI.indentLevel -= 1;
            EditorGUI.EndProperty();
        }

        private void ExpandHeight(SerializedProperty subProperty, ref int totalLines)
        {
            totalLines += subProperty.arraySize + 1;

            var subPropertySize = GetSubFieldLength(subProperty);
            if (subProperty.isArray)
            {
                for (var i = 0; i < subProperty.arraySize; i++)
                {
                    var tempProp = subProperty.GetArrayElementAtIndex(i);
                    if (!tempProp.isExpanded) continue;
                    if (IsExternalModeProperty(tempProp))
                    {
                        totalLines += 2;
                    }
                    else
                    {
                        totalLines += subPropertySize;
                    }
                }

                totalLines++;
            }
            else
            {
                if (subProperty.isExpanded)
                    totalLines += subPropertySize;
            }
        }

        private bool IsExternalModeProperty(SerializedProperty subProperty)
        {
            var modeProperty = subProperty.FindPropertyRelative("_mode");

            if (modeProperty == null) return false;
            return modeProperty.GetEnum(out PropertyEvent.PropertyMode mode) &&
                   mode == PropertyEvent.PropertyMode.External;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int totalLines = 1;

            if (property.isExpanded)
            {
                totalLines++;

                var type = property.FindPropertyRelative("_type");
                var flagEnum = GetFlagEnumValueFromIndex(type.intValue);
                totalLines += Enum
                    .GetValues(typeof(EventType))
                    .Cast<Enum>()
                    .Count(flagEnum.HasFlag) - 1;

                var subProperty = property.FindPropertyRelative("_armyEvents");
                if ((flagEnum & EventType.ArmyVisitor) == EventType.ArmyVisitor &&
                    subProperty.isExpanded)
                {
                    ExpandHeight(subProperty, ref totalLines);
                }

                subProperty = property.FindPropertyRelative("_propertyEvents");
                if ((flagEnum & EventType.PropertyVisitor) == EventType.PropertyVisitor &&
                    subProperty.isExpanded)
                {
                    ExpandHeight(subProperty, ref totalLines);
                }
            }

            return EditorGUIUtility.singleLineHeight * totalLines +
                   EditorGUIUtility.standardVerticalSpacing * (totalLines - 1);
        }
    }
}