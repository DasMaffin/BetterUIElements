using Maffin.ENums;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Maffin.BetterUI
{
    /// <summary>
    /// HorizontalLayoutGroup with spacing as a percentage of the screen width.
    /// </summary>
    [AddComponentMenu("Layout/Horizontal Layout Group (Percent spacing)")]
    public class HorizontalLayoutGroupPercentage : HorizontalLayoutGroup
    {
        [SerializeField, Range(0f, 100f), Tooltip("Spacing as percentage of screen width.")]
        private float spacingPercent = 0f;

        [SerializeField] private RectOffsetPercentage paddingPercent = new RectOffsetPercentage();

        public float SpacingPercent
        {
            get => spacingPercent;
            set => spacingPercent = Mathf.Clamp(value, 0f, 100f);
        }

        public override void CalculateLayoutInputHorizontal()
        {
            padding = paddingPercent.PercentToOffset();
            base.CalculateLayoutInputHorizontal();

            float pixelSpacing = Screen.width * (SpacingPercent / 100f);

            spacing = pixelSpacing;
        }
    }
}
