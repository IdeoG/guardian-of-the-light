using UniRx;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Player;
using _Guardian_of_the_Light.Scripts.Systems;

public class GameManagerSystem : MonoBehaviour
{
    public static GameManagerSystem Instance { get; private set; }
    
    [Header("Canvases")] 
    [SerializeField] private GameObject _canvasInventoryView;

    [Header("Player")] 
    [SerializeField] public GameObject Player;
    
    private InventoryCanvas _inventoryCanvas;  
    private InputSystem _inputSystem;  
    
    private void Awake()
    {
        Instance = this;

        _inventoryCanvas = _canvasInventoryView.GetComponent<InventoryCanvas>();
        _inputSystem = InputSystem.Instance;
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
        
        _inputSystem.IsUiActive = true;
        _inventoryCanvas.Show();
    }

    private void HideInventoryView()
    {
        _inventoryCanvas.Hide();
        _inputSystem.IsUiActive = false;
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
