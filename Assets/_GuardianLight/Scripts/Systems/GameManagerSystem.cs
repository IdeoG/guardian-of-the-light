using System;
using UniRx;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

/**
 * TODO: GameManagerSystem
 * 1. Сделать одно полотно для подсказок
 * 2. Создать класс, который хранит карту из текста подсказки и ссылки на картинку
 * 3. В окне подсказки, текст должен содержать имя ключа и подсказку типа "Этим ключом можно что-то открыть"
 */
public class GameManagerSystem : MonoBehaviour
{
    [Header("Герой")] [SerializeField] private GameObject _player;
    
    [Header("Полотна")]
    [SerializeField] private GameObject _canvasInspectView;
    [SerializeField] private GameObject _canvasInventoryView;
    [SerializeField] private GameObject _canvasPadlockNoKeyHint;

    public static GameManagerSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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

}

public enum HintType
{
    PadlockNoKeyFound = 1
}