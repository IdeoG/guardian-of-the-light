using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.Systems
{
    public class InputSystem : MonoBehaviour
    {
        public static InputSystem Instance
        {
            get
            {
                if (_instance == null) 
                    _instance = FindObjectOfType<InputSystem>();

                return _instance;
            }
        }
        
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

        public IObservable<Unit> KeyUpArrowPressedDown { get; private set; }
        public IObservable<Unit> KeyDownArrowPressedDown { get; private set; }
        public IObservable<Unit> KeyLeftArrowPressedDown { get; private set; }
        public IObservable<Unit> KeyRightArrowPressedDown { get; private set; }

        public IObservable<Unit> KeyReduceSizePressed { get; private set; }
        public IObservable<Unit> KeyIncreaseSizePressed { get; private set; }

        public IObservable<bool> KeyCrouchPressed { get; private set; }
        public IObservable<Unit> KeyJumpPressedDown { get; private set; }

        public IObservable<Unit> KeyYesHintPressedDown { get; private set; }
        public IObservable<Unit> KeyNoHintPressedDown { get; private set; }
        public IObservable<Unit> KeySkipHintPressedDown { get; private set; }
        public IObservable<Unit> KeyConfirmHintPressedDown { get; private set; }
        public IObservable<Unit> KeyExitHintPressedDown { get; private set; }
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
        [SerializeField] private KeyCode _confirmHintKey;
        [SerializeField] private KeyCode _exitHintKey;
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

        private static InputSystem _instance;

    
        private void Awake()
        {   
            ReferencePlayerInputs();
            ReferenceInventoryInputs();
            ReferenceHintsInputs();
        }

        private void ReferencePlayerInputs()
        {
            KeyActionPressed = this.UpdateAsObservable().Where(_ => CanMove()).Where(_ => Input.GetKey(_actionKey));
            KeyExtraActionPressed = this.UpdateAsObservable().Where(_ => CanMove()).Where(_ => Input.GetKey(_extraActionKey));

            KeyActionPressedDown = this.UpdateAsObservable().Where(_ => CanMove()).Where(_ => Input.GetKeyDown(_actionKey));

            KeyCrouchPressed = this.UpdateAsObservable().Where(_ => CanJumpAndCrouch()).Select(_ => Input.GetKey(_crouchKey));
            KeyJumpPressedDown = this.UpdateAsObservable().Where(_ => CanJumpAndCrouch()).Where(_ => Input.GetKeyDown(_jumpKey));
        }

        private void ReferenceInventoryInputs()
        {
            KeyInventoryPressedDown = this.UpdateAsObservable().Where(_ => CanOpenInventory()).Where(_ => Input.GetKeyDown(_inventoryKey));
        
            KeyInspectPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(_inspectViewKey));
            KeyBackViewPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(_backViewKey));

            KeyUpArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(KeyCode.W));
            KeyDownArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(KeyCode.S));
            KeyLeftArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(KeyCode.A));
            KeyRightArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(KeyCode.D));

            KeyUpArrowPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(KeyCode.W));
            KeyDownArrowPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(KeyCode.S));
            KeyLeftArrowPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(KeyCode.A));
            KeyRightArrowPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKeyDown(KeyCode.D));
        
            KeyReduceSizePressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(_reduceSizeKey));
            KeyIncreaseSizePressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetKey(_increaseSizeKey));
        }

        private void ReferenceHintsInputs()
        {
            KeyYesHintPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_yesHintKey));
            KeyNoHintPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_noHintKey));
            KeySkipHintPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_skipHintKey));
            KeyConfirmHintPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_confirmHintKey));
            KeyExitHintPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_exitHintKey));
            KeyTemporaryButtonPressedDown = this.UpdateAsObservable().Where(_ => Input.GetKeyDown(_temporaryButtonHintKey));
        }

        public bool CanMove()
        {
            return !IsUiActive && !IsAnimationPlaying;
        }

        private bool CanJumpAndCrouch()
        {
            return !IsUiActive && !IsPlayerInCollider && !IsAnimationPlaying;
        }

        private bool CanOpenInventory()
        {
            return !IsUiActive && !IsAnimationPlaying;
        }
    }
}