using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace com.DilawarHussain.UIArchitecture.Core
{
    public abstract class UIView : MonoBehaviour
    {
        [Header("Base View Settings")]
        [SerializeField] protected CanvasGroup canvasGroup;
        //[SerializeField] protected UIScreenTypes screenType;
        [SerializeField] protected bool showLastPanelOnHide;
        public UIScreenTypes ScreenType { get; private set; }

        public bool IsActive { get; protected set; }

        protected Stack<UIScreenTypes> navigationHistory = new Stack<UIScreenTypes>();

        public UnityEvent OnShowEvent = new UnityEvent();
        public UnityEvent OnHideEvent = new UnityEvent();

        public virtual void Initialize(UIScreenTypes screenType, object data = null)
        {
            this.ScreenType = screenType;
            gameObject.SetActive(false);
        }

        protected virtual void OnDestroy()
        {
            OnShowEvent.RemoveAllListeners();
            OnHideEvent.RemoveAllListeners();
        }


        public void Show(object data = null)
        {
            if (IsActive) return;

            gameObject.SetActive(true);
            OnShow(data);
            IsActive = true;
            OnShowEvent?.Invoke();
        }

        public void Hide()
        {
            if (!IsActive) return;

            OnHide();
            IsActive = false;
            OnHideEvent?.Invoke();

            if (!showLastPanelOnHide) return;

            if (navigationHistory.Count > 0)
            {
                UIManager.Instance.ShowScreen(navigationHistory.Pop());
            }
        }
        public virtual void SetDefault()
        {
            // Base implementation can be empty
            // Derived classes will override this
        }

        protected abstract void OnShow(object data);
        protected abstract void OnHide();

        public void RememberNavigation(UIScreenTypes screenType)
        {
            navigationHistory.Push(screenType);
        }

        public void ClearNavigationHistory()
        {
            navigationHistory.Clear();
        }
    }
}