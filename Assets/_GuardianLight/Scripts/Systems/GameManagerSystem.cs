using System;
using UnityEngine;

/**
 * TODO: GameManagerSystem
 * 1. Сделать одно полотно для подсказок
 * 2. Создать класс, который хранит карту из текста подсказки и ссылки на картинку
 * 3. В окне подсказки, текст должен содержать имя ключа и подсказку типа "Этим ключом можно что-то открыть"
 */
public class GameManagerSystem : MonoBehaviour
{
    [SerializeField] private GameObject _canvasPadlockNoKeyHint;
    [SerializeField] private GameObject _canvasInventoryView;
    [SerializeField] private GameObject _canvasInspectView;
    
    public static GameManagerSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
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
        _canvasInventoryView.SetActive(true);
    }

    public void HideInventoryView()
    {
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
    
    
    private void ShowPadlockNoKeyFound()
    {
        _canvasPadlockNoKeyHint.SetActive(true);
        Invoke("HideHint", 2f);
    }

}

public enum HintType
{
    PadlockNoKeyFound = 1
}