using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
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

    public bool IsInInventory;
    
    [SerializeField] private KeyCode _actionKey = KeyCode.J;
    [SerializeField] private KeyCode _extraActionKey = KeyCode.L;
    
    [SerializeField] private KeyCode _reduceSizeKey = KeyCode.J;
    [SerializeField] private KeyCode _increaseSizeKey = KeyCode.L;


    
    private void Awake()
    {
        Instance = this;

        KeyActionPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_actionKey));
        KeyExtraActionPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_extraActionKey));

        KeyActionPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.E));
        KeyInspectPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.O));
        KeyInventoryPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.I));

        KeyUpArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.W));
        KeyDownArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.S));
        KeyLeftArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.A));
        KeyRightArrowPressed = this.UpdateAsObservable().Where(_ => Input.GetKey(KeyCode.D));

        KeyLeftArrowPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.A));
        KeyRightArrowPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(KeyCode.D));
        
        KeyReduceSizePressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_reduceSizeKey));
        KeyIncreaseSizePressed = this.UpdateAsObservable().Where(_ => Input.GetKey(_increaseSizeKey));
    }
}