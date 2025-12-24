using System;
using UnityEngine;
namespace com.DilawarHussain.UIArchitecture.Core
{
    [Serializable]
    public class ScreenData
    {
        public UIScreenTypes screenType;
        public GameObject screenPrefab;
        public bool isPopup;
        public UIScreenTypes parentScreen;
        public bool showByDefault;
    }
}