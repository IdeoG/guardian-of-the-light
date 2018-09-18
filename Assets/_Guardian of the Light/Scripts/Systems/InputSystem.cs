using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [Header("Actions")]
    [SerializeField] private KeyCode _actionKey = KeyCode.J;
    [SerializeField] private KeyCode _extraActionKey = KeyCode.L;
    
    [Header("Inventory")]
    [SerializeField] private KeyCode _increaseSizeKey = KeyCode.J;
    [SerializeField] private KeyCode _reduceSizeKey = KeyCode.I;
    [SerializeField] private KeyCode _inventoryKey = KeyCode.L;
    [SerializeField] private KeyCode _inspectViewKey = KeyCode.J;
    
    [Header("Player Controls")]
    [SerializeField] private KeyCode _crouchKey = KeyCode.K;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;

    public bool IsInInventory;
    public static InputSystem Instance { get; private set; }

    public IObservable<Unit> KeyActionPressed { get; private set; }
    public IObservable<Unit> KeyExtraActionPressed { get; private set; }

    public IObservable<Unit> KeyActionPressedDown { get; private set; }
    public IObservable<Unit> KeyInspectPressedDown { get; private set; }
    public IObservable<Unit> KeyInventoryPressedDown { get; private set; }

    public IObservable<Unit> KeyUpArrowPressed { get; private set; }
    public IObservable<Unit> KeyDownArrowPressed { get; private set; }
    public IObservable<Unit> KeyLeftArrowPressed { get; private set; }
    public IObservable<Unit> KeyRightArrowPressed { get; private set; }

    public IObservable<Unit> KeyLeftArrowPressedDown { get; private set; }
    public IObservable<Unit> KeyRightArrowPressedDown { get; private set; }

    public IObservable<Unit> KeyReduceSizePressed { get; private set; }
    public IObservable<Unit> KeyIncreaseSizePressed { get; private set; }
    
    public IObservable<Unit> KeyCrouchPressedDown { get; private set; }
    public IObservable<Unit> KeyJumpPressedDown { get; private set; }



    private void Awake()
    {
        Instance = this;

        KeyActionPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_actionKey));
        KeyExtraActionPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_extraActionKey));

        KeyActionPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.E));
        
        KeyInspectPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_inspectViewKey));
        KeyInventoryPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_inventoryKey));

        KeyUpArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.W));
        KeyDownArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.S));
        KeyLeftArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.A));
        KeyRightArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.D));

        KeyLeftArrowPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.A));
        KeyRightArrowPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.D));

        KeyReduceSizePressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_reduceSizeKey));
        KeyIncreaseSizePressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_increaseSizeKey));
        
        KeyCrouchPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKey(_crouchKey));
        KeyJumpPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKey(_jumpKey));
    }
}