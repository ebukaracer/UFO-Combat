using System.Collections;
using DG.Tweening;
using Racer.Utilities;
using UnityEngine;

internal class GameUITween : MonoBehaviour
{
    private Tween _tween;

    [SerializeField] private UIImage bulletUI;
    [SerializeField] private UICanvasGroup waveUI;
    [SerializeField] private UIElement[] mainUI;
    [SerializeField] private UIElement menuUI;
    [SerializeField] private UIElement newWaveUI;

    private void Start()
    {
        for (int i = 0; i < mainUI.Length; i++)
            HideUI(mainUI[i]);

        HideWaveUI();
    }

    public void DisplayMainUI(bool value)
    {
        if (value)
            for (int i = 0; i < mainUI.Length; i++)
                DisplayUI(mainUI[i]);
        else
            for (int i = 0; i < mainUI.Length; i++)
                HideUI(mainUI[i]);
    }


    public void DisplayMenuUI(bool value)
    {
        if (value)
            DisplayUI(menuUI);
        else
            HideUI(menuUI);
    }

    public void PlayNewWaveUI()
    {
        newWaveUI.rectTransform.gameObject.ToggleActive(true);

        _tween = newWaveUI.rectTransform.DOScale(newWaveUI.EndValue,
                newWaveUI.Duration)
            .SetEase(newWaveUI.EaseType)
            .SetLoops(-1,
                LoopType.Yoyo);
    }

    public void DisplayWaveUI()
    {
        waveUI.canvasGroup.DOFade(waveUI.EndValue,
                waveUI.Duration)
            .SetEase(waveUI.EaseType)
            .OnComplete(() => Invoke(nameof(HideWaveUI),
                .5f));
    }

    private void HideWaveUI()
    {
        waveUI.canvasGroup.DOFade(waveUI.StartValue, waveUI.Duration);
    }

    public void PlayBulletUI(bool isIncrease)
    {
        StartCoroutine(SmoothBulletUI(isIncrease));

        IEnumerator SmoothBulletUI(bool v)
        {
            var amount = v ? bulletUI.EndValue : bulletUI.StartValue;

            var fill = bulletUI.image;
            var preChangeAmount = bulletUI.image.fillAmount;

            // Dynamic
            float elapsed = 0;

            while (elapsed < bulletUI.Duration)
            {
                elapsed += Time.deltaTime;
                fill.fillAmount = Mathf.Lerp(preChangeAmount, amount, elapsed / bulletUI.Duration);

                yield return 0;
            }

            fill.fillAmount = amount;
        }
    }

    private static void DisplayUI(UIElement uiElement)
    {
        uiElement.rectTransform.DOAnchorPos(uiElement.EndValue,
                uiElement.Duration)
            .SetEase(uiElement.EaseType)
            .SetUpdate(true);
    }

    private static void HideUI(UIElement uiElement)
    {
        uiElement.rectTransform.DOAnchorPos(uiElement.StartValue, uiElement.Duration);
    }

    private void OnDisable()
    {
        if (DOTween.instance)
            _tween?.Kill();
    }
}
