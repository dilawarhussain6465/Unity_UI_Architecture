using UnityEngine;
using UnityEngine.UI;
using TMPro;
using com.DilawarHussain.UIArchitecture.Core;
public class ProfileScreenView : FullScreenView
{
    [Header("Profile Screen Components")]
    [SerializeField] private Button closeButton;
    [SerializeField] private TextMeshProUGUI userNameText;

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        userNameText.text = "Unity Developer"; // Test purpose Name

        closeButton.onClick.AddListener(OnCloseButtonClicked);
    }

    protected override void OnHide()
    {
        base.OnHide();
        closeButton.onClick.RemoveListener(OnCloseButtonClicked);
    }

    private void OnCloseButtonClicked()
    {
        UIManager.Instance.GoBack();
    }

    public override void SetDefault()
    {
        base.SetDefault();
        userNameText.text = string.Empty;
    }
}