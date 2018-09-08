﻿using UniRx;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameManagerSystem : MonoBehaviour
{
    [Header("Canvases")] [SerializeField] private GameObject _canvasInventoryView;
    [Header("vSyncCount")] [SerializeField] private int _vSyncCount = 3;
    [Header("Player")] [SerializeField] public GameObject Player;

    public static GameManagerSystem Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
        QualitySettings.vSyncCount = _vSyncCount;
    }

    private void Start()
    {
        InputSystem.Instance.KeyInventoryPressedDown
            .Subscribe(_ => OnKeyInventoryPressedDown())
            .AddTo(this);
    }

    private void ShowInventoryView()
    {
        Player.GetComponent<ThirdPersonUserControl>().LockInput = true;
        _canvasInventoryView.SetActive(true);
    }

    private void HideInventoryView()
    {
        Player.GetComponent<ThirdPersonUserControl>().LockInput = false;
        _canvasInventoryView.SetActive(false);
    }

    private void OnKeyInventoryPressedDown()
    {
        var isInventoryActive = _canvasInventoryView.activeSelf;

        if (isInventoryActive)
            HideInventoryView();
        else
            ShowInventoryView();
    }
}