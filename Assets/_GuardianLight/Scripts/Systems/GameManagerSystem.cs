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

    private void ShowPadlockNoKeyFound()
    {
        _canvasPadlockNoKeyHint.SetActive(true);
        Invoke("HideHint", 2f);
    }

    public void HideHint()
    {
        _canvasPadlockNoKeyHint.SetActive(false);
    }

    public static GameManagerSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}

public enum HintType
{
    PadlockNoKeyFound = 1
}