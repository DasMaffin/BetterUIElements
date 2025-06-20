namespace Maffin.BetterUI.Editor
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(HorizontalLayoutSizeFitter))]
    public class HorizontalLayoutSizeFitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            HorizontalLayoutSizeFitter setter = (HorizontalLayoutSizeFitter)target;

            if(setter.HasFixableModeError())
            {
                EditorGUILayout.Space();

                // Begin horizontal layout for HelpBox + Button
                EditorGUILayout.BeginHorizontal();

                // Draw the HelpBox, taking up most of the width, with word wrap
                // We use GUILayout.Width or ExpandWidth to control width
                // Let's give it flexible width but a max width to avoid stretching too far
                GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
                EditorGUILayout.HelpBox(setter.GetModeErrorMessage(), MessageType.Error);
                GUILayout.EndVertical();

                // Calculate a square button height based on single line height * 1.5 for padding
                float buttonHeight = EditorGUIUtility.singleLineHeight * 1.5f;

                // Add some horizontal space between HelpBox and Button
                GUILayout.Space(8);

                // Vertically center the button relative to HelpBox
                Rect lastRect = GUILayoutUtility.GetLastRect();
                float helpBoxHeight = lastRect.height > 0 ? lastRect.height : buttonHeight * 2;

                // Use GUILayoutUtility.GetRect to create a fixed size button rect
                Rect buttonRect = GUILayoutUtility.GetRect(buttonHeight, buttonHeight);

                // Adjust Y to vertically center button next to HelpBox
                buttonRect.y += (helpBoxHeight - buttonHeight) / 2;

                if(GUI.Button(buttonRect, "Fix"))
                {
                    Undo.RecordObject(setter, "Fix Responsive Width Mode");
                    setter.FixInvalidMode();
                    EditorUtility.SetDirty(setter);
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
