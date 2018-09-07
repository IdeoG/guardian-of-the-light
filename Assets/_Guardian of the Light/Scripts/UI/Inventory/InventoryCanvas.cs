using System;
using UniRx;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _basicView;
    [SerializeField] private GameObject _inspectView;

    private IDisposable _keyInspectItemPressedDown;


    private void OnKeyInspectItemPressedDown()
    {
        // BUG: Fast double click on Inventory Button leads to Missing Reference on GameObject. Block double click, then fade effect is running.
        var isInspectViewActive = _inspectView.activeSelf;

        if (isInspectViewActive)
        {
            HideInspectView();
        }
        else
        {
            ShowInspectView();
        }
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

        _basicView.GetComponent<FadeEffect>().FadeIn();
        _inspectView.SetActive(false);
    }

    private void OnDisable()
    {
        _keyInspectItemPressedDown.Dispose();
    }

}