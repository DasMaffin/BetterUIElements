namespace Maffin.PropertyDrawer
{
    using Maffin.BetterUI;
    using Maffin.ENums;
    using UnityEditor;
    using UnityEngine;

    [CustomPropertyDrawer(typeof(PercentSpace))]
    public class PercentSpaceDrawer : PropertyDrawer
    {
        private readonly GUIContent screenToggleContent = new GUIContent("", "Selected: use screen space.\nUnselected: use local (parent) space.");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty valueProp = property.FindPropertyRelative("value");
            SerializedProperty spaceProp = property.FindPropertyRelative("space");

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float valueWidth = position.width * 0.8f;
            float toggleWidth = position.width * 0.2f;

            Rect valueRect = new Rect(position.x, position.y, valueWidth - 4, position.height);
            Rect toggleRect = new Rect(position.x + valueWidth + 2, position.y, toggleWidth - 2, position.height);

            EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
            bool isScreen = spaceProp.enumValueIndex == (int)ESpace.Screen;
            bool newIsScreen = EditorGUI.Toggle(toggleRect, screenToggleContent, isScreen);

            if(newIsScreen != isScreen)
            {
                spaceProp.enumValueIndex = newIsScreen ? (int)ESpace.Screen : (int)ESpace.Parent;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}