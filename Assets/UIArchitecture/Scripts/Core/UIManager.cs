using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace com.DilawarHussain.UIArchitecture.Core
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("UI Configuration")]
        [SerializeField] private Transform screensParent;
        [SerializeField] private ScreenData[] screensData;
        [SerializeField] private UIScreenTypes defaultScreen;
        [SerializeField] private bool enableScreenHistory = true;



        private Dictionary<UIScreenTypes, UIView> screens = new Dictionary<UIScreenTypes, UIView>();
        private Dictionary<UIScreenTypes, Dictionary<UIScreenTypes, PopupView>> screenPopups =
            new Dictionary<UIScreenTypes, Dictionary<UIScreenTypes, PopupView>>();
        private Stack<UIScreenTypes> screenHistory = new Stack<UIScreenTypes>();
        private GameObject dimBackgroundInstance;
        private List<PopupView> activePopups = new List<PopupView>();

        public UIView CurrentScreen { get; private set; }
        public UnityEvent<UIScreenTypes, object> OnScreenChanged = new UnityEvent<UIScreenTypes, object>();
        public UnityEvent<UIScreenTypes> OnScreenClosed = new UnityEvent<UIScreenTypes>();

        #region Initialization
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }

        private void Initialize()
        {
            CreateAllScreens();
            ShowDefaultScreen();
        }



        private void CreateAllScreens()
        {
            foreach (var data in screensData)
            {
                if (data.screenPrefab == null || data.isPopup) continue;
                CreateScreen(data);

                if (data.screenType != defaultScreen)
                {
                    screens[data.screenType].gameObject.SetActive(false);
                }
            }

            foreach (var data in screensData)
            {
                if (data.screenPrefab == null || !data.isPopup) continue;
                CreatePopup(data);

                screenPopups[data.parentScreen][data.screenType].gameObject.SetActive(false);
            }
        }

        private void CreateScreen(ScreenData data)
        {
            if (screens.ContainsKey(data.screenType))
            {
                Debug.LogError($"Duplicate screen type detected: {data.screenType}. Screen not created.");
                return;
            }

            GameObject screenObj = Instantiate(data.screenPrefab, screensParent);
            screenObj.name = $"{data.screenType}_Screen";

            UIView view = screenObj.GetComponent<UIView>();
            if (view == null)
            {
                Debug.LogWarning($"UIView component missing on screen prefab: {data.screenType}");
                Destroy(screenObj);
                return;
            }

            view.Initialize(data.screenType);
            view.Hide();
            screens[data.screenType] = view;
            screenPopups[data.screenType] = new Dictionary<UIScreenTypes, PopupView>();
        }

        private void CreatePopup(ScreenData data)
        {
            if (!screens.ContainsKey(data.parentScreen))
            {
                Debug.LogWarning($"Parent screen {data.parentScreen} not found for popup {data.screenType}");
                return;
            }

            Transform parentTransform = screens[data.parentScreen].transform;
            GameObject popupObj = Instantiate(data.screenPrefab, parentTransform);
            popupObj.name = $"{data.screenType}_Popup";

            PopupView popup = popupObj.GetComponent<PopupView>();
            if (popup == null)
            {
                Debug.LogWarning($"PopupView component missing on popup prefab: {data.screenType}");
                Destroy(popupObj);
                return;
            }

            popup.Initialize(data.screenType);
            popup.Hide();
            screenPopups[data.parentScreen][data.screenType] = popup;
        }



        private void ShowDefaultScreen()
        {
            if (screens.ContainsKey(defaultScreen))
            {
                ShowScreen(defaultScreen);
            }
            else
            {
                Debug.LogError($"Default screen {defaultScreen} not found in registered screens");
            }
        }


        #endregion

        #region Screen Management
        public void ShowScreen(UIScreenTypes screenType, object data = null, bool rememberInHistory = true)
        {
            if (!screens.TryGetValue(screenType, out UIView screen))
            {
                Debug.LogWarning($"Screen {screenType} not found");
                return;
            }

            if (!screen.gameObject.activeSelf)
            {
                screen.gameObject.SetActive(true);
            }

            if (CurrentScreen != null)
            {
                if (rememberInHistory && enableScreenHistory)
                {
                    screenHistory.Push(CurrentScreen.ScreenType);
                }
                CurrentScreen.Hide();
            }

            CurrentScreen = screen;
            CurrentScreen.Show(data);
            OnScreenChanged?.Invoke(screenType, data);
            screen.transform.SetAsLastSibling();
        }

        public void CloseCurrentScreen()
        {
            if (CurrentScreen != null)
            {
                var screenType = CurrentScreen.ScreenType;
                CurrentScreen.Hide();
                CurrentScreen = null;
                OnScreenClosed?.Invoke(screenType);
            }
        }

        public void CloseScreen(UIScreenTypes screenType)
        {
            if (screens.TryGetValue(screenType, out UIView screen))
            {
                if (CurrentScreen == screen)
                {
                    CloseCurrentScreen();
                }
                else
                {
                    screen.Hide();
                    OnScreenClosed?.Invoke(screenType);
                }
            }
        }

        public void GoBack()
        {
            if (screenHistory.Count > 0)
            {
                ShowScreen(screenHistory.Pop(), rememberInHistory: false);
            }
            else
            {
                Debug.Log("No screens in history to go back to");
            }
        }

        public void ClearHistory()
        {
            screenHistory.Clear();
        }
        #endregion

        #region Popup Management
        public void ShowPopup(UIScreenTypes popupType, UIScreenTypes parentScreenType,
                         object data = null, bool useDim = true)
        {
            if (!screenPopups.TryGetValue(parentScreenType, out var popups) ||
                !popups.TryGetValue(popupType, out PopupView popup))
            {
                Debug.LogWarning($"Popup {popupType} not found under parent {parentScreenType}");
                return;
            }

            // Ensure the popup is active before showing
            if (!popup.gameObject.activeSelf)
            {
                popup.gameObject.SetActive(true);
            }

            popup.Show(data);
            activePopups.Add(popup);

            // Bring parent screen and popup to front
            if (screens.TryGetValue(parentScreenType, out UIView parentScreen))
            {
                parentScreen.transform.SetAsLastSibling();
            }
            popup.transform.SetAsLastSibling();


        }

        public void ClosePopup(PopupView popup)
        {
            if (activePopups.Contains(popup))
            {
                popup.Hide();
                activePopups.Remove(popup);
                popup.gameObject.SetActive(false); // Deactivate the popup

                if (activePopups.Count == 0 && dimBackgroundInstance != null)
                {
                    dimBackgroundInstance.SetActive(false);
                }
            }
        }

        public void CloseAllPopups()
        {
            foreach (var popup in activePopups)
            {
                popup.Hide();
            }
            activePopups.Clear();

            if (dimBackgroundInstance != null)
            {
                dimBackgroundInstance.SetActive(false);
            }
        }
        #endregion

        #region Utility Methods
        public T GetScreen<T>(UIScreenTypes screenType) where T : UIView
        {
            return screens.TryGetValue(screenType, out UIView screen) ? (T)screen : null;
        }

        public T GetPopup<T>(UIScreenTypes popupType, UIScreenTypes parentScreenType) where T : PopupView
        {
            if (screenPopups.TryGetValue(parentScreenType, out var popups) &&
                popups.TryGetValue(popupType, out PopupView popup))
            {
                return (T)popup;
            }
            return null;
        }

        public bool IsScreenActive(UIScreenTypes screenType)
        {
            return screens.TryGetValue(screenType, out UIView screen) && screen.IsActive;
        }

        public bool IsPopupActive(UIScreenTypes popupType)
        {
            foreach (var popup in activePopups)
            {
                if (popup.ScreenType == popupType)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion


    }
}