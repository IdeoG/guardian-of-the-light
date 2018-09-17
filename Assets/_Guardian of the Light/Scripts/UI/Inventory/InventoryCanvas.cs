using System;
using DG.Tweening;
using UniRx;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _basicView;

    [SerializeField] private float _canvasDisappearanceDuration;
    [SerializeField] private GameObject _inspectView;

    private IDisposable _keyInspectItemPressedDown;
    public bool IsInspectViewActive => _inspectView.activeInHierarchy;

    public void Show()
    {
        gameObject.SetActive(true);

        _basicView.GetComponent<FadeEffect>().FadeIn();
        _inspectView.SetActive(false);
    }

    public void Hide()
    {
        var canvasGroup = _basicView.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, _canvasDisappearanceDuration).OnComplete(() =>
        {
            canvasGroup.gameObject.SetActive(false);
            gameObject.SetActive(false);
        });
    }

    private void OnKeyInspectItemPressedDown()
    {
        if (IsInspectViewActive)
            HideInspectView();
        else
            ShowInspectView();
    }

    private void HideInspectView()
    {
        _basicView.GetComponent<FadeEffect>().FadeIn();
        _inspectView.GetComponent<FadeEffect>().FadeOut();
        _inspectView.GetComponent<InspectView>().PlayOnDisableAnimation();
    }

    private void ShowInspectView()
    {
        var item = _basicView.GetComponent<BasicView>().GetCurrentItem();

        _basicView.GetComponent<FadeEffect>().FadeOut();
        _inspectView.GetComponent<FadeEffect>().FadeIn();

        _inspectView.GetComponent<InspectView>().SetItem(item);
        _inspectView.GetComponent<InspectView>().PlayOnEnableAnimation();
    }

    private void OnEnable()
    {
        _keyInspectItemPressedDown = InputSystem.Instance.KeyInspectPressedDown
            .Subscribe(_ => OnKeyInspectItemPressedDown())
            .AddTo(this);
    }

    private void OnDisable()
    {
        _keyInspectItemPressedDown.Dispose();
    }
}