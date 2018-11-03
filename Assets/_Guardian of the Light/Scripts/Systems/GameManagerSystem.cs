using UniRx;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Player;
using _Guardian_of_the_Light.Scripts.Systems;

public class GameManagerSystem : MonoBehaviour
{
    [Header("Canvases")] 
    [SerializeField] private GameObject _canvasInventoryView;

    [Header("Player")] 
    [SerializeField] public GameObject Player;
    
    public static GameManagerSystem Instance { get; private set; }
    private InventoryCanvas _inventoryCanvas;

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
        _inventoryCanvas.Show();
    }

    private void HideInventoryView()
    {
        _inventoryCanvas.Hide();
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
