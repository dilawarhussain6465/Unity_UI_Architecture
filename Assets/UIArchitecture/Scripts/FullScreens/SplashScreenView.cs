using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SplashScreenView : FullScreenView
{
    [Header("Splash Screen Components")]
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private float loadingDuration = 3f;

    private Coroutine loadingCoroutine;
    private Coroutine dotsCoroutine;

    protected override void OnShow(object data)
    {
        base.OnShow(data);
        SetDefault();
        StartLoadingAnimation();
    }

    protected override void OnHide()
    {
        base.OnHide();
        if (loadingCoroutine != null) StopCoroutine(loadingCoroutine);
        if (dotsCoroutine != null) StopCoroutine(dotsCoroutine);
    }

    private void StartLoadingAnimation()
    {
        loadingCoroutine = StartCoroutine(LoadingAnimation());
        dotsCoroutine = StartCoroutine(AnimateLoadingDots());
    }

    private IEnumerator LoadingAnimation()
    {
        float elapsedTime = 0f;
        while (elapsedTime < loadingDuration)
        {
            float progress = elapsedTime / loadingDuration;
            loadingSlider.value = progress;
            percentageText.text = Mathf.RoundToInt(progress * 100) + "%";
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values
        loadingSlider.value = 1f;
        percentageText.text = "100%";

        // Transition to home screen
        UIManager.Instance.ShowScreen(UIScreenTypes.Home);
    }

    private IEnumerator AnimateLoadingDots()
    {
        int dotCount = 0;
        string baseText = "Loading";

        while (true)
        {
            dotCount = (dotCount + 1) % 4; // Cycle through 0-3 dots
            loadingText.text = baseText + new string('.', dotCount);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void SetDefault()
    {
        base.SetDefault();
        loadingSlider.value = 0f;
        percentageText.text = "0%";
        loadingText.text = "Loading";
    }
}