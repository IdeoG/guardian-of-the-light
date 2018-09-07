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
    [Header("Полотна")] [SerializeField] private GameObject _canvasInventoryView;

    [Header("Fixed frame rate")] [SerializeField]
    private int _frameRate;

    [Header("Герой")] [SerializeField] private GameObject _player;

    public static GameManagerSystem Instance { get; private set; }


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