using UnityEngine;
using UnityEngine.UI;
namespace com.DilawarHussain.UIArchitecture.Core
{
    public abstract class PopupView : UIView
    {
        [Header("Popup Settings")]
        [SerializeField] private bool closeOnOutsideClick = true;
        [SerializeField] private float showAnimationDuration = 0.3f;

        private RectTransform rectTransform;
        private Vector2 originalSize;
        private Coroutine currentAnimation;

        protected override void OnShow(object data)
        {
            rectTransform = GetComponent<RectTransform>();
            originalSize = rectTransform.sizeDelta;

            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            // Start fade in animation
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(FadeAnimation(0, 1, showAnimationDuration));



            if (closeOnOutsideClick)
            {
                RegisterBackdropClick();
            }
        }

        protected override void OnHide()
        {
            if (currentAnimation != null)
            {
                StopCoroutine(currentAnimation);
            }
            currentAnimation = StartCoroutine(FadeAnimation(1, 0, showAnimationDuration, () =>
            {
                gameObject.SetActive(false);
            }));
        }

        private System.Collections.IEnumerator FadeAnimation(float startAlpha, float endAlpha, float duration, System.Action onComplete = null)
        {
            float elapsed = 0;
            while (elapsed < duration)
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = endAlpha;
            onComplete?.Invoke();
            currentAnimation = null;
        }

        private void RegisterBackdropClick()
        {
            // Create a full-screen transparent button for backdrop clicks
            GameObject backdrop = new GameObject("Backdrop");
            backdrop.transform.SetParent(transform, false);

            RectTransform backdropRect = backdrop.AddComponent<RectTransform>();
            backdropRect.anchorMin = Vector2.zero;
            backdropRect.anchorMax = Vector2.one;
            backdropRect.sizeDelta = Vector2.zero;
            backdropRect.SetAsFirstSibling();

            Image backdropImage = backdrop.AddComponent<Image>();
            backdropImage.color = Color.clear;

            Button backdropButton = backdrop.AddComponent<Button>();
            backdropButton.onClick.AddListener(Close);
        }

        public void Close()
        {
            UIManager.Instance.ClosePopup(this);
        }

        public override void SetDefault()
        {
            base.SetDefault();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = originalSize;
            }
        }
    }
}