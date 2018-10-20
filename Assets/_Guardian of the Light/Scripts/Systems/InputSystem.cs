using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputSystem : MonoBehaviour
{

    public static InputSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<InputSystem>();
            }

            return _instance;
        }
    }

    private static InputSystem _instance;

    #region public vars

    public IObservable<Unit> KeyActionPressed { get; private set; }
    public IObservable<Unit> KeyExtraActionPressed { get; private set; }

    public IObservable<Unit> KeyActionPressedDown { get; private set; }
    public IObservable<Unit> KeyInspectPressedDown { get; private set; }
    public IObservable<Unit> KeyInventoryPressedDown { get; private set; }
    public IObservable<Unit> KeyBackViewPressedDown { get; private set; }

    public IObservable<Unit> KeyUpArrowPressed { get; private set; }
    public IObservable<Unit> KeyDownArrowPressed { get; private set; }
    public IObservable<Unit> KeyLeftArrowPressed { get; private set; }
    public IObservable<Unit> KeyRightArrowPressed { get; private set; }

    public IObservable<Unit> KeyLeftArrowPressedDown { get; private set; }
    public IObservable<Unit> KeyRightArrowPressedDown { get; private set; }

    public IObservable<Unit> KeyReduceSizePressed { get; private set; }
    public IObservable<Unit> KeyIncreaseSizePressed { get; private set; }
    
    public IObservable<Unit> KeyCrouchPressed { get; private set; }
    public IObservable<Unit> KeyJumpPressedDown { get; private set; }
    
    public bool IsInInventory;
        #endregion
    
    #region half private vars

    [Header("Actions")]
    [SerializeField] private KeyCode _actionKey = KeyCode.J;
    [SerializeField] private KeyCode _extraActionKey = KeyCode.L;
    
    [Header("Inventory")]
    [SerializeField] private KeyCode _increaseSizeKey = KeyCode.J;
    [SerializeField] private KeyCode _reduceSizeKey = KeyCode.K;
    [SerializeField] private KeyCode _inventoryKey = KeyCode.I;
    [SerializeField] private KeyCode _inspectViewKey = KeyCode.J;
    [SerializeField] private KeyCode _backViewKey = KeyCode.L;
    
    [Header("Player Controls")]
    [SerializeField] public KeyCode _crouchKey = KeyCode.K;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    
    #endregion


    private void Awake()
    {
        ReferenceInputs();
    }

    private void ReferenceInputs()
    {
        KeyActionPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_actionKey));
        KeyExtraActionPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_extraActionKey));

        KeyActionPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.E));
        
        KeyInspectPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_inspectViewKey));
        KeyInventoryPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_inventoryKey));
        KeyBackViewPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_backViewKey));

        KeyUpArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.W));
        KeyDownArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.S));
        KeyLeftArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.A));
        KeyRightArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.D));

        KeyLeftArrowPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.A));
        KeyRightArrowPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.D));

        KeyReduceSizePressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_reduceSizeKey));
        KeyIncreaseSizePressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_increaseSizeKey));
        
        KeyCrouchPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_crouchKey));
        KeyJumpPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_jumpKey));
    }
}