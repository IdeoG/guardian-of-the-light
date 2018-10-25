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

    public IObservable<bool> KeyCrouchPressed { get; private set; }
    public IObservable<Unit> KeyJumpPressedDown { get; private set; }

    public IObservable<Unit> KeyYesHintPressedDown { get; private set; }
    public IObservable<Unit> KeyNoHintPressedDown { get; private set; }
    public IObservable<Unit> KeySkipHintPressedDown { get; private set; }
    public IObservable<Unit> KeyTemporaryButtonPressedDown { get; private set; }
    
    public bool IsUiActive;
    public bool IsPlayerInCollider;
    public bool IsAnimationPlaying;

    #endregion

    #region half private vars

    [Header("Hints")] 
    [SerializeField] private KeyCode _yesHintKey;
    [SerializeField] private KeyCode _noHintKey;
    [SerializeField] private KeyCode _skipHintKey;
    [SerializeField] private KeyCode _temporaryButtonHintKey;
    
    
    [Header("Inventory")] 
    [SerializeField] private KeyCode _increaseSizeKey = KeyCode.J;
    [SerializeField] private KeyCode _reduceSizeKey = KeyCode.K;
    [SerializeField] private KeyCode _inventoryKey = KeyCode.I;
    [SerializeField] private KeyCode _inspectViewKey = KeyCode.J;
    [SerializeField] private KeyCode _backViewKey = KeyCode.L;

    [Header("Player Controls")] 
    [SerializeField] private KeyCode _crouchKey = KeyCode.K;
    [SerializeField] private KeyCode _jumpKey = KeyCode.L;
    [SerializeField] private KeyCode _actionKey = KeyCode.J;
    [SerializeField] private KeyCode _extraActionKey = KeyCode.L;

    #endregion


    private void Awake()
    {
        ReferencePlayerInputs();
        ReferenceInventoryInputs();
        ReferenceHintInputs();
    }

    private void ReferencePlayerInputs()
    {
        KeyActionPressed = this.UpdateAsObservable().Where(_ => !IsUiActive && !IsAnimationPlaying)
            .Where(_ => Input.GetKey(_actionKey));
        KeyExtraActionPressed = this.UpdateAsObservable().Where(_ => !IsUiActive && !IsAnimationPlaying)
            .Where(_ => Input.GetKey(_extraActionKey));

        KeyActionPressedDown = this.UpdateAsObservable().Where(_ => !IsUiActive && !IsAnimationPlaying)
            .Where(_ => Input.GetKeyDown(_actionKey));

        KeyCrouchPressed = this.UpdateAsObservable().Where(_ => !(IsUiActive || IsPlayerInCollider) && !IsAnimationPlaying)
            .Select(_ => Input.GetKey(_crouchKey));
        KeyJumpPressedDown = this.UpdateAsObservable().Where(_ => !(IsUiActive || IsPlayerInCollider) && !IsAnimationPlaying)
            .Where(_ => Input.GetKeyDown(_jumpKey));
    }

    private void ReferenceInventoryInputs()
    {
        KeyInventoryPressedDown = this.UpdateAsObservable().Where(_ => !IsUiActive && !IsAnimationPlaying).Where(_ => Input.GetKeyDown(_inventoryKey));
        KeyInspectPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(_inspectViewKey));
        KeyBackViewPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(_backViewKey));

        KeyUpArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(KeyCode.W));
        KeyDownArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(KeyCode.S));
        KeyLeftArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(KeyCode.A));
        KeyRightArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(KeyCode.D));

        KeyLeftArrowPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(KeyCode.A));
        KeyRightArrowPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(KeyCode.D));
        
        KeyReduceSizePressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(_reduceSizeKey));
        KeyIncreaseSizePressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(_increaseSizeKey));
    }

    private void ReferenceHintInputs()
    {
        KeyYesHintPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_yesHintKey));
        KeyNoHintPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_noHintKey));
        KeySkipHintPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_skipHintKey));
        KeyTemporaryButtonPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_temporaryButtonHintKey));
    }
}