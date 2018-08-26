using System;
using UniRx;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameManagerSystem : MonoBehaviour
{
    [Header("Player")] [SerializeField] private GameObject _player;

    [Header("Canvases")] [SerializeField] private GameObject _canvasInspectView;
    [SerializeField] private GameObject _canvasInventoryView;
    [SerializeField] private GameObject _canvasPadlockNoKeyHint;

    [Header("Fixed frame rate")] [SerializeField]
    private int _frameRate;


    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = _frameRate;
    }

    private void Start()
    {
        InputSystem.Instance.KeyInventoryPressedDown
            .Subscribe(_ => OnKeyInventoryPressedDown())
            .AddTo(this);
    }

    private void ShowPadlockNoKeyFound()
    {
        _canvasPadlockNoKeyHint.SetActive(true);
        Invoke("HideHint", 2f);
    }

    private void ShowInventoryView()
    {
        _player.GetComponent<ThirdPersonUserControl>().LockInput = true;
        _canvasInventoryView.SetActive(true);
    }

    private void HideInventoryView()
    {
        _player.GetComponent<ThirdPersonUserControl>().LockInput = false;
        _canvasInventoryView.SetActive(false);
    }

    private void OnKeyInventoryPressedDown()
    {
        var isInventoryActive = !_canvasInventoryView.activeSelf;

        if (isInventoryActive)
            ShowInventoryView();
        else
            HideInventoryView();
    }
    
    public void ShowHint(HintType hintType)
    {
        switch (hintType)
        {
            case HintType.PadlockNoKeyFound:
                ShowPadlockNoKeyFound();
                break;
            default:
                throw new ArgumentOutOfRangeException("hintType", hintType, null);
        }
    }

    public void HideHint()
    {
        _canvasPadlockNoKeyHint.SetActive(false);
    }

    
    public static GameManagerSystem Instance { get; private set; }
}

public enum HintType
{
    PadlockNoKeyFound = 1
}