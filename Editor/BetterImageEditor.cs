using System.Linq;
using UnityEngine;
using UnityEditor.AnimatedValues;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    /// <summary>
    /// Editor class used to edit UI Sprites.
    /// </summary>

    [CustomEditor(typeof(Image), true)]
    [CanEditMultipleObjects]
    /// <summary>
    ///   Custom Editor for the Image Component.
    ///   Extend this class to write a custom editor for a component derived from Image.
    /// </summary>
    public class BetterImageEditor : ImageEditor
    {
        SerializedProperty m_FillMethod;
        SerializedProperty m_FillOrigin;
        SerializedProperty m_FillAmount;
        SerializedProperty m_FillClockwise;
        SerializedProperty m_Type;
        SerializedProperty m_FillCenter;
        SerializedProperty m_Sprite;
        SerializedProperty m_PreserveAspect;
        SerializedProperty m_UseSpriteMesh;
        SerializedProperty m_PixelsPerUnitMultiplier; 
        SerializedProperty m_UseCornerRadiusPercent;
        SerializedProperty m_cornerRadius;
        SerializedProperty m_cornerRadiusPercent;
        SerializedProperty m_cornerSegments;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_Sprite = serializedObject.FindProperty("m_Sprite");
            m_Type = serializedObject.FindProperty("m_Type");
            m_FillCenter = serializedObject.FindProperty("m_FillCenter");
            m_FillMethod = serializedObject.FindProperty("m_FillMethod");
            m_FillOrigin = serializedObject.FindProperty("m_FillOrigin");
            m_FillClockwise = serializedObject.FindProperty("m_FillClockwise");
            m_FillAmount = serializedObject.FindProperty("m_FillAmount");
            m_PreserveAspect = serializedObject.FindProperty("m_PreserveAspect");
            m_UseSpriteMesh = serializedObject.FindProperty("m_UseSpriteMesh");
            m_PixelsPerUnitMultiplier = serializedObject.FindProperty("m_PixelsPerUnitMultiplier");
            m_UseCornerRadiusPercent = serializedObject.FindProperty("useCornerRadiusPercent");
            m_cornerRadius = serializedObject.FindProperty("cornerRadius");
            m_cornerRadiusPercent = serializedObject.FindProperty("cornerRadiusPercent");
            m_cornerSegments = serializedObject.FindProperty("segments");

            SetShowNativeSize(true);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SpriteGUI(); 
            CornerGUI();
            AppearanceControlsGUI();
            RaycastControlsGUI();
            MaskableControlsGUI();

            EditorGUILayout.EndFadeGroup();

            SetShowNativeSize(false);
            if(EditorGUILayout.BeginFadeGroup(m_ShowNativeSize.faded))
            {
                EditorGUI.indentLevel++;

                if((Image.Type)m_Type.enumValueIndex == Image.Type.Simple)
                    EditorGUILayout.PropertyField(m_UseSpriteMesh);

                EditorGUILayout.PropertyField(m_PreserveAspect);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndFadeGroup();
            NativeSizeButtonGUI();

            serializedObject.ApplyModifiedProperties();
        }

        void SetShowNativeSize(bool instant)
        {
            Image.Type type = (Image.Type)m_Type.enumValueIndex;
            bool showNativeSize = (type == Image.Type.Simple || type == Image.Type.Filled) && m_Sprite.objectReferenceValue != null;
            base.SetShowNativeSize(showNativeSize, instant);
        }

        internal void CornerGUI()
        {
            //Toggle for using percentages
            EditorGUILayout.PropertyField(m_UseCornerRadiusPercent, new GUIContent("Use percentages"));

            if(!m_UseCornerRadiusPercent.boolValue)
            {
                EditorGUILayout.PropertyField(m_cornerRadius, true);
            }
            else
            {
                EditorGUILayout.PropertyField(m_cornerRadiusPercent, true);
            }
            EditorGUILayout.PropertyField(m_cornerSegments, true);
        }
    }
}
