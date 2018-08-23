using System;
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

    public void ShowInventoryView()
    {
        _player.GetComponent<ThirdPersonUserControl>().LockInput = true;
        _canvasInventoryView.SetActive(true);
    }

    public void HideInventoryView()
    {
        _player.GetComponent<ThirdPersonUserControl>().LockInput = false;
        _canvasInventoryView.SetActive(false);
    }

    public void ShowInspectView()
    {
        throw new NotImplementedException();
    }

    public void HideInspectView()
    {
        throw new NotImplementedException();
    }

}

public enum HintType
{
    PadlockNoKeyFound = 1
}