using Maffin.BetterUI;
using UnityEngine;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(HorizontalLayoutGroupPercentage), true)]
    [CanEditMultipleObjects]
    public class HorizontalLayoutGroupPercentageEditor : HorizontalOrVerticalLayoutGroupEditor
    {
        SerializedProperty m_PaddingPercent;
        SerializedProperty m_SpacingPercent;
        SerializedProperty m_ChildAlignment;
        SerializedProperty m_ChildControlWidth;
        SerializedProperty m_ChildControlHeight;
        SerializedProperty m_ChildScaleWidth;
        SerializedProperty m_ChildScaleHeight;
        SerializedProperty m_ChildForceExpandWidth;
        SerializedProperty m_ChildForceExpandHeight;
        SerializedProperty m_ReverseArrangement;

        protected override void OnEnable()
        {
            m_PaddingPercent = serializedObject.FindProperty("paddingPercent");
            m_SpacingPercent = serializedObject.FindProperty("spacingPercent");
            m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
            m_ChildControlWidth = serializedObject.FindProperty("m_ChildControlWidth");
            m_ChildControlHeight = serializedObject.FindProperty("m_ChildControlHeight");
            m_ChildScaleWidth = serializedObject.FindProperty("m_ChildScaleWidth");
            m_ChildScaleHeight = serializedObject.FindProperty("m_ChildScaleHeight");
            m_ChildForceExpandWidth = serializedObject.FindProperty("m_ChildForceExpandWidth");
            m_ChildForceExpandHeight = serializedObject.FindProperty("m_ChildForceExpandHeight");
            m_ReverseArrangement = serializedObject.FindProperty("m_ReverseArrangement");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_PaddingPercent, true);
            EditorGUILayout.PropertyField(m_SpacingPercent, true);
            EditorGUILayout.PropertyField(m_ChildAlignment, true);
            EditorGUILayout.PropertyField(m_ReverseArrangement, true);

            Rect rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Control Child Size"));
            rect.width = Mathf.Max(50, (rect.width - 4) / 3);
            EditorGUIUtility.labelWidth = 50;
            ToggleLeft(rect, m_ChildControlWidth, EditorGUIUtility.TrTextContent("Width"));
            rect.x += rect.width + 2;
            ToggleLeft(rect, m_ChildControlHeight, EditorGUIUtility.TrTextContent("Height"));
            EditorGUIUtility.labelWidth = 0;

            rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Use Child Scale"));
            rect.width = Mathf.Max(50, (rect.width - 4) / 3);
            EditorGUIUtility.labelWidth = 50;
            ToggleLeft(rect, m_ChildScaleWidth, EditorGUIUtility.TrTextContent("Width"));
            rect.x += rect.width + 2;
            ToggleLeft(rect, m_ChildScaleHeight, EditorGUIUtility.TrTextContent("Height"));
            EditorGUIUtility.labelWidth = 0;

            rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, -1, EditorGUIUtility.TrTextContent("Child Force Expand"));
            rect.width = Mathf.Max(50, (rect.width - 4) / 3);
            EditorGUIUtility.labelWidth = 50;
            ToggleLeft(rect, m_ChildForceExpandWidth, EditorGUIUtility.TrTextContent("Width"));
            rect.x += rect.width + 2;
            ToggleLeft(rect, m_ChildForceExpandHeight, EditorGUIUtility.TrTextContent("Height"));
            EditorGUIUtility.labelWidth = 0;

            serializedObject.ApplyModifiedProperties();
        }

        void ToggleLeft(Rect position, SerializedProperty property, GUIContent label)
        {
            bool toggle = property.boolValue;
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            toggle = EditorGUI.ToggleLeft(position, label, toggle);
            EditorGUI.indentLevel = oldIndent;
            if(EditorGUI.EndChangeCheck())
            {
                property.boolValue = property.hasMultipleDifferentValues ? true : !property.boolValue;
            }
            EditorGUI.EndProperty();
        }
    }
}
