/*using UnityEditor;
using Utils.Editor;

namespace Cards.SituationCards.Editor
{
    [CustomEditor(typeof(Situation))]
    public class SituationEditor : UnityEditor.Editor
    {
        private SerializedProperty _modeProperty;

        private void OnEnable()
        {
            _modeProperty = serializedObject.FindProperty("_mode");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_modeProperty);
            Situation.Mode mode;

            if (_modeProperty.GetEnum(out mode))
            {
                switch (mode)
                {
                    case Situation.Mode.Bound:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_bound"));
                        break;
                    case Situation.Mode.External:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("_external"));
                        break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}*/