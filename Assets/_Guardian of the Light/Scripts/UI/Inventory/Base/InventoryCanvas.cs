using System;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Systems;

public class InventoryCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _basicView;

    [SerializeField] private float _canvasDisappearanceDuration;
    [SerializeField] private GameObject _inspectView;

    private IDisposable _keyInspectItemPressedDown;
    private IDisposable _keyBackViewPressedDown;

    private float? _freeLookMaxSpeed;
    
    public bool IsInspectViewActive => _inspectView.activeInHierarchy;

    public void Show()
    {
        InputSystem.Instance.IsUiActive.Value = true;
        _freeLookMaxSpeed = FindObjectOfType<CinemachineFreeLook>()?.m_XAxis.m_MaxSpeed;
        if (_freeLookMaxSpeed != null) 
            FindObjectOfType<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = 0f;
        
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
            
            InputSystem.Instance.IsUiActive.Value = false;
            if (_freeLookMaxSpeed != null) 
                FindObjectOfType<CinemachineFreeLook>().m_XAxis.m_MaxSpeed = (float) _freeLookMaxSpeed;
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