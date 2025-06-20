namespace Maffin.BetterUI
{
    using Maffin.ENums;
    using UnityEngine;

    [System.Serializable]
    public class PercentSpace
    {
        public float value;
        public ESpace space = ESpace.Screen;
    }

    [System.Serializable]
    public class RectOffsetPercentage : RectOffset
    {
        [SerializeField] public PercentSpace leftPercent;
        [SerializeField] public PercentSpace rightPercent;
        [SerializeField] public PercentSpace topPercent;
        [SerializeField] public PercentSpace bottomPercent;

        public RectOffset PercentToOffset()
        {
            left = Mathf.RoundToInt(Screen.width * (leftPercent.value / 100f));
            right = Mathf.RoundToInt(Screen.width * (rightPercent.value / 100f));
            top = Mathf.RoundToInt(Screen.height * (topPercent.value / 100f));
            bottom = Mathf.RoundToInt(Screen.height * (bottomPercent.value / 100f));
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
            return this;
        }
    }
}
