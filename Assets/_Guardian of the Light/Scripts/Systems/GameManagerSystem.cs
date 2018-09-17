using UniRx;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameManagerSystem : MonoBehaviour
{
    [Header("Canvases")] [SerializeField] private GameObject _canvasInventoryView;

    private InventoryCanvas _inventoryCanvas;

    [Header("vSyncCount")] [SerializeField]
    private VSyncType _vSyncCount = VSyncType.EveryVSync60Fps;

    [Header("Player")] [SerializeField] public GameObject Player;
    public static GameManagerSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        QualitySettings.vSyncCount = (int) _vSyncCount;

        _inventoryCanvas = _canvasInventoryView.GetComponent<InventoryCanvas>();
    }

    private void Start()
    {
        InputSystem.Instance.KeyInventoryPressedDown
            .Subscribe(_ => OnKeyInventoryPressedDown())
            .AddTo(this);
    }

    private void ShowInventoryView()
    {
        if (InventorySystem.Instance.GetTookItems().Count == 0) return;

        Player.GetComponent<ThirdPersonUserControl>().LockInput = true;
        InputSystem.Instance.IsInInventory = true;
        _inventoryCanvas.Show();
    }

    private void HideInventoryView()
    {
        Player.GetComponent<ThirdPersonUserControl>().LockInput = false;
        InputSystem.Instance.IsInInventory = false;
        _inventoryCanvas.Hide();
    }

    private void OnKeyInventoryPressedDown()
    {
        if (_inventoryCanvas.IsInspectViewActive) return;

        var isInventoryActive = _canvasInventoryView.activeSelf;

        if (isInventoryActive)
            HideInventoryView();
        else
            ShowInventoryView();
    }
}

public enum VSyncType
{
    EveryVSync60Fps = 1,
    EverySecondVSync30Fps = 2,
    NoVSync = 0
}