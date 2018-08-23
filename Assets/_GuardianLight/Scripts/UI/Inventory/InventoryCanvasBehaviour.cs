using System;
using UniRx;
using UnityEngine;

public class InventoryCanvasBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _basicView;
    [SerializeField] private GameObject _inspectView;


    private IDisposable _keyInspectItemPressedDown;


    private void OnKeyInspectItemPressedDown()
    {
        var isInpectViewActive = _inspectView.activeSelf;

        if (!isInpectViewActive)
        {
            var item = _basicView.GetComponent<BasicViewBehaviour>().GetCurrentItem();

            _basicView.SetActive(false);
            _inspectView.SetActive(true);
            
            _inspectView.GetComponent<InspectViewBehaviour>().SetItem(item);
        }
        else
        {
            _basicView.SetActive(true);
            _inspectView.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _keyInspectItemPressedDown = InputSystem.Instance.KeyInspectPressedDown
            .Subscribe(_ => OnKeyInspectItemPressedDown())
            .AddTo(this);
        
        _basicView.SetActive(true);
        _inspectView.SetActive(false);

    }

    private void OnDisable()
    {
        _keyInspectItemPressedDown.Dispose();
    }
}