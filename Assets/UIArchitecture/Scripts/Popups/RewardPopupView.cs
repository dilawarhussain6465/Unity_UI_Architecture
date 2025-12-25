using UnityEngine;
using UnityEngine.UI;
using TMPro;
using com.DilawarHussain.UIArchitecture.Core;
public class RewardPopupView : PopupView
{
    [Header("Reward Popup Components")]
    [SerializeField] private Button closeButton;
    [SerializeField] private Button claimButton;
    [SerializeField] private TextMeshProUGUI rewardText;

    protected override void OnShow(object data)
    {
        base.OnShow(data);

        closeButton.onClick.AddListener(OnCloseButtonClicked);
        claimButton.onClick.AddListener(OnClaimButtonClicked);

        // Example reward text - could be customized with data parameter
        rewardText.text = "You've earned 10 coins!";
    }

    protected override void OnHide()
    {
        base.OnHide();
        closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        claimButton.onClick.RemoveListener(OnClaimButtonClicked);
    }

    private void OnCloseButtonClicked()
    {
        Close();
    }

    private void OnClaimButtonClicked()
    {
        Debug.Log("Reward claimed!");
        Close();
    }

    public override void SetDefault()
    {
        base.SetDefault();
        rewardText.text = string.Empty;
    }
}