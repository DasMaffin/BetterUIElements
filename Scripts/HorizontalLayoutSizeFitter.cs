namespace Maffin.BetterUI
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;
    using Maffin.ENums;

    [AddComponentMenu("Layout/Horizontal Layout Size Fitter", 141)]
    [ExecuteAlways]
    [RequireComponent(typeof(RectTransform))]
    public partial class HorizontalLayoutSizeFitter : UIBehaviour
    {
        [UnityEngine.Range(0, 100)]
        public float widthPercent = 100f;

        [Tooltip("Choose whether to base the width on screen size or parent RectTransform.")]
        public ESpace mode = ESpace.Screen;

        private RectTransform rectTransform;
        
        [SerializeField, HideInInspector] private string _errorMessage = "";
        [SerializeField, HideInInspector] private bool _hasFixableModeError = false;

        public bool HasFixableModeError() => _hasFixableModeError;

        public string GetModeErrorMessage() => _errorMessage;

        protected override void OnEnable()
        {
            base.OnEnable();
            rectTransform = GetComponent<RectTransform>(); 
            ValidateMode();
            UpdateWidth();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            rectTransform = GetComponent<RectTransform>();
            ValidateMode();
            UpdateWidth();
        }

        void Update()
        {
            if(!Application.isPlaying)
            {
                ValidateMode();
                UpdateWidth();
            }
        }
#endif

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            UpdateWidth();
        }

        private void ValidateMode()
        {
            _errorMessage = ""; 
            _hasFixableModeError = false;

            if(mode == ESpace.Parent && rectTransform.parent is RectTransform parentRect)
            {
                var fitter = parentRect.GetComponent<ContentSizeFitter>();
                if(fitter != null && fitter.horizontalFit != ContentSizeFitter.FitMode.Unconstrained)
                {
                    _errorMessage = $"Cannot use 'Parent' mode: parent has a ContentSizeFitter with Horizontal Fit set to {fitter.horizontalFit}. Has to be Unconstrained!";
                    _hasFixableModeError = true;
                }
            }
        }

        public void FixInvalidMode()
        {
            mode = ESpace.Screen;
            _errorMessage = "";
            _hasFixableModeError = false;
            Debug.Log($"[ResponsiveWidthSetter] Mode switched to Screen to fix layout conflict.", this);
            UpdateWidth();
        }

        private void UpdateWidth()
        {
            if(rectTransform == null) return;

            float referenceWidth = 0f;

            switch(mode)
            {
                case ESpace.Screen:
                    referenceWidth = Screen.width;
                    break;
                case ESpace.Parent:
                    if(rectTransform.parent is RectTransform parentRect)
                        referenceWidth = parentRect.rect.width;
                    break;
            }

            float newWidth = referenceWidth * (widthPercent / 100f);
            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = newWidth;
            rectTransform.sizeDelta = sizeDelta;
        }
    }
}
