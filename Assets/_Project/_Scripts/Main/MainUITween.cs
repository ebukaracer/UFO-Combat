using DG.Tweening;
using System;
using Racer.Utilities;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
internal class UIElement : TweenProperties
{
    public RectTransform rectTransform;
}

[Serializable]
internal class UICanvasGroup : TweenProperties2
{
    public CanvasGroup canvasGroup;
}

[Serializable]
internal class UIImage : TweenProperties2
{
    public Image image;
}

internal class MainUITween : MonoBehaviour
{
    private GameObject _userUIGo;
    private GameObject _inputUIGo;
    private GameObject _nextIcoGo;

    [SerializeField] private UIElement userUI;
    [SerializeField] private UIElement infoUI;
    [SerializeField] private UIElement inputFieldUI;
    [SerializeField] private UIElement infoIco;
    [SerializeField] private UIElement nextIcon;
    [SerializeField] private UIElement titleIcon;

    [SerializeField] private UICanvasGroup startupUICg;
    [SerializeField] private UICanvasGroup infoUICg;

    private void Awake()
    {
        _userUIGo = userUI.rectTransform.gameObject;
        _inputUIGo = inputFieldUI.rectTransform.gameObject;
        _nextIcoGo = nextIcon.rectTransform.gameObject;

        userUI.rectTransform.localScale = userUI.StartValue;
        inputFieldUI.rectTransform.localScale = inputFieldUI.StartValue;
        // _nextIcoGo.ToggleActive(false);
    }

    public void DisplayUserInputUI(bool value)
    {
        if (value)
        {
            startupUICg.canvasGroup.DOFade(startupUICg.StartValue, 0);
            startupUICg.canvasGroup.interactable = false;

            infoUICg.canvasGroup.DOFade(infoUICg.StartValue, 0);
            infoUICg.canvasGroup.interactable = false;

            _inputUIGo.ToggleActive(true);

            inputFieldUI.rectTransform.DOScale(inputFieldUI.EndValue, inputFieldUI.Duration)
                .SetEase(inputFieldUI.EaseType);
        }
        else
        {
            inputFieldUI.rectTransform.DOScale(inputFieldUI.StartValue, inputFieldUI.Duration)
                .SetEase(inputFieldUI.EaseType)
                .OnComplete(() =>
            {
                startupUICg.canvasGroup.DOFade(startupUICg.EndValue, startupUICg.Duration);
                startupUICg.canvasGroup.interactable = true;

                infoUICg.canvasGroup.DOFade(infoUICg.EndValue, infoUICg.Duration);
                infoUICg.canvasGroup.interactable = true;

                _inputUIGo.ToggleActive(true);
            });
        }
    }
    public void DisplayUserUI(bool value)
    {
        if (value)
        {
            _userUIGo.ToggleActive(true);
            userUI.rectTransform.DOScale(userUI.EndValue, userUI.Duration).SetEase(userUI.EaseType);
        }
        else
        {
            userUI.rectTransform.DOScale(userUI.StartValue, userUI.Duration).SetEase(userUI.EaseType)
                  .OnComplete(() => _userUIGo.ToggleActive(false));
        }
    }

    public void DisplayInfoUI(bool value)
    {
        RotateInfoIcon(value);
        DisplayNextIcon(value);

        if (value)
            infoUI.rectTransform.DOAnchorPos(infoUI.EndValue, infoUI.Duration).SetEase(infoUI.EaseType);
        else
            infoUI.rectTransform.DOAnchorPos(infoUI.StartValue, infoUI.Duration).SetEase(infoUI.EaseType);
    }

    public void DisplayTitleIcon()
    {
        titleIcon.rectTransform.DOAnchorPos(titleIcon.EndValue, titleIcon.Duration).SetEase(titleIcon.EaseType);
    }

    private void RotateInfoIcon(bool value)
    {
        // Using the y-vector in place of z for rotation.
        if (value)
            infoIco.rectTransform.DOLocalRotate(new Vector3(infoIco.EndValue.x,
                        0,
                        infoIco.EndValue.y),
                    infoIco.Duration)
                .SetEase(infoIco.EaseType);
        else
            infoIco.rectTransform.DOLocalRotate(new Vector3(infoIco.StartValue.x,
                        0,
                        infoIco.StartValue.y),
                    infoIco.Duration)
                .SetEase(infoIco.EaseType);
    }

    private void DisplayNextIcon(bool value)
    {
        if (value)
        {
            _nextIcoGo.ToggleActive(true);
            nextIcon.rectTransform.DOScale(nextIcon.EndValue, nextIcon.Duration).SetEase(nextIcon.EaseType);
        }
        else
        {
            nextIcon.rectTransform.DOScale(nextIcon.StartValue, nextIcon.Duration).SetEase(nextIcon.EaseType)
                .OnComplete(() => _nextIcoGo.ToggleActive(false));
        }
    }
}
