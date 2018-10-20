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
    private IDisposable _keyBackViewPressedDown;
    
    public bool IsInspectViewActive => _inspectView.activeInHierarchy;

    public void Show()
    {
        InputSystem.Instance.IsUiActive = true;
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
            
            InputSystem.Instance.IsUiActive = false;
        });
    }

    private void HideInspectView()
    {
        _basicView.GetComponent<FadeEffect>().FadeIn();
        _inspectView.GetComponent<FadeEffect>().FadeOut();
        _inspectView.GetComponent<InspectView>().PlayOnDisableAnimation();
        
        _keyBackViewPressedDown?.Dispose();
        
        _keyInspectItemPressedDown = InputSystem.Instance.KeyInspectPressedDown
            .Subscribe(_ => ShowInspectView())
            .AddTo(this);
    }

    private void ShowInspectView()
    { 
        var item = _basicView.GetComponent<BasicView>().GetCurrentItem();

        _basicView.GetComponent<FadeEffect>().FadeOut();
        _inspectView.GetComponent<FadeEffect>().FadeIn();

        _inspectView.GetComponent<InspectView>().SetItem(item);
        _inspectView.GetComponent<InspectView>().PlayOnEnableAnimation();
        
        _keyInspectItemPressedDown?.Dispose();
        
        _keyBackViewPressedDown = InputSystem.Instance.KeyBackViewPressedDown
            .Subscribe(_ => HideInspectView())
            .AddTo(this);
    }

    private void OnEnable()
    {
        _keyInspectItemPressedDown = InputSystem.Instance.KeyInspectPressedDown
            .Subscribe(_ => ShowInspectView())
            .AddTo(this);
    }

    private void OnDisable()
    {
        _keyInspectItemPressedDown?.Dispose();
        _keyBackViewPressedDown?.Dispose();
    }
}