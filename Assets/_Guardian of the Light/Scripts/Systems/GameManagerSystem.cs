using UniRx;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class GameManagerSystem : MonoBehaviour
{
    [Header("Player")] [SerializeField] public GameObject Player;
    public static GameManagerSystem Instance { get; private set; }
    
    [Header("Canvases")] [SerializeField] private GameObject _canvasInventoryView;
    [Header("vSyncCount")] [SerializeField] private int _vSyncCount = 3;

    private InventoryCanvas _inventoryCanvas;

    private void Awake()
    {
        Instance = this;
        
        QualitySettings.vSyncCount = _vSyncCount;

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
        Player.GetComponent<ThirdPersonUserControl>().LockInput = true;
        _inventoryCanvas.Show();
    }

    private void HideInventoryView()
    {
        Player.GetComponent<ThirdPersonUserControl>().LockInput = false;
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
