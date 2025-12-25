using UnityEngine;
using UnityEngine.UI;
using com.DilawarHussain.UIArchitecture.Core;
public class HomeScreenView : FullScreenView
{
    [Header("Home Screen Components")]
    [SerializeField] private Button rewardButton;
    [SerializeField] private Button profileButton;
    protected override void OnShow(object data)
    {
        base.OnShow(data);
        rewardButton.onClick.AddListener(OnRewardButtonClicked);
        profileButton.onClick.AddListener(OnProfileButtonClicked);
    }

    protected override void OnHide()
    {
        base.OnHide();
        rewardButton.onClick.RemoveListener(OnRewardButtonClicked);
        profileButton.onClick.RemoveListener(OnProfileButtonClicked);
    }

    private void OnRewardButtonClicked()
    {
        UIManager.Instance.ShowPopup(UIScreenTypes.RewardPopup, UIScreenTypes.Home);
    }
    private void OnProfileButtonClicked() 
    {
        UIManager.Instance.ShowScreen(UIScreenTypes.Profile);
    }
    public override void SetDefault()
    {
        base.SetDefault();
    }
}