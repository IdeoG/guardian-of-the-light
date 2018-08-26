﻿using System;
using UniRx;
using UnityEngine;

public class InventoryCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _basicView;
    [SerializeField] private GameObject _inspectView;


    private IDisposable _keyInspectItemPressedDown;


    private void OnKeyInspectItemPressedDown()
    {
        var isInspectViewActive = _inspectView.activeSelf;

        if (!isInspectViewActive)
        {
            var item = _basicView.GetComponent<BasicView>().GetCurrentItem();

//            _basicView.SetActive(false);
//            _inspectView.SetActive(true);
            
            _basicView.GetComponent<FadeEffect>().FadeOut();
            _inspectView.GetComponent<FadeEffect>().FadeIn();
            
            _inspectView.GetComponent<InspectView>().SetItem(item);
        }
        else
        {
//            _basicView.SetActive(true);
            _basicView.GetComponent<FadeEffect>().FadeIn();
            _inspectView.GetComponent<FadeEffect>().FadeOut();
//            _inspectView.SetActive(false);
        }
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