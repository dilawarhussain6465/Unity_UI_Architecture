// FullScreenView.cs
using UnityEngine;
namespace com.DilawarHussain.UIArchitecture.Core
{
    public abstract class FullScreenView : UIView
    {
        [Header("FullScreen Settings")]
        [SerializeField] private bool hidePreviousScreen = true;

        protected override void OnShow(object data)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;

            if (hidePreviousScreen && UIManager.Instance.CurrentScreen != null)
            {
                RememberNavigation(UIManager.Instance.CurrentScreen.ScreenType);
            }
        }

        protected override void OnHide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            gameObject.SetActive(false);
        }

        public virtual void SetDefault()
        {
            // Implement default state in derived classes
        }
    }
}