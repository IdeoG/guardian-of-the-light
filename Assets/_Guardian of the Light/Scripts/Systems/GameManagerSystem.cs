using UniRx;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameManagerSystem : MonoBehaviour
{
    [Header("Canvases")] [SerializeField] private GameObject _canvasInventoryView;

    private InventoryCanvas _inventoryCanvas;


    [Header("Player")] [SerializeField] public GameObject Player;
    public static GameManagerSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        _inventoryCanvas = _canvasInventoryView.GetComponent<InventoryCanvas>();
    }

    private void Start()
    {
        InputSystem.Instance.KeyInventoryPressedDown
            .Subscribe(_ => OnKeyInventoryPressedDown())
            .AddTo(this);
        
        InputSystem.Instance.KeyBackViewPressedDown
            .Subscribe(_ => OnKeyBackViewPressedDown())
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
        _inventoryCanvas.Hide();
        InputSystem.Instance.IsInInventory = false;
        Player.GetComponent<ThirdPersonUserControl>().LockInput = false;
    }

    private void OnKeyInventoryPressedDown()
    {
        if (_inventoryCanvas.IsInspectViewActive) return;
        if (_canvasInventoryView.activeSelf) return;

        ShowInventoryView();
    }

    private void OnKeyBackViewPressedDown()
    {
        if (_inventoryCanvas.IsInspectViewActive) return;
        if (!_canvasInventoryView.activeSelf) return;

        HideInventoryView();
    }
}
