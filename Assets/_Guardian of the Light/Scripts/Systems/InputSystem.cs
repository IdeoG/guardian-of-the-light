using System;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
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
        public IObservable<Unit> KeyUsePressedDown { get; private set; }

        public IObservable<Unit> KeyUpArrowPressed { get; private set; }
        public IObservable<Unit> KeyDownArrowPressed { get; private set; }
        public IObservable<Unit> KeyLeftArrowPressed { get; private set; }
        public IObservable<Unit> KeyRightArrowPressed { get; private set; }


        public IObservable<Unit> KeyReduceSizePressed { get; private set; }
        public IObservable<Unit> KeyIncreaseSizePressed { get; private set; }

        public IObservable<bool> KeyCrouchPressed { get; private set; }
        public IObservable<Unit> KeyJumpPressedDown { get; private set; }
        public IObservable<float> KeyRunPressedDown { get; private set; }
        
        public IObservable<float> HorizontalAxis { get; private set; }
        public IObservable<float> VerticalAxis { get; private set; }
        public IObservable<float> RightStickX { get; private set; }
        public IObservable<float> RightStickY { get; private set; }

        public IObservable<Unit> KeyYesHintPressedDown { get; private set; }
        public IObservable<Unit> KeyNoHintPressedDown { get; private set; }
        public IObservable<Unit> KeySkipHintPressedDown { get; private set; }
        public IObservable<Unit> KeyConfirmHintPressedDown { get; private set; }
        public IObservable<Unit> KeyExitHintPressedDown { get; private set; }
    
        public bool IsUiActive;
        public bool IsPlayerInCollider;
        public bool IsAnimationPlaying;

        #endregion

        #region half private vars

        [Header("Hints")] 
        [SerializeField] private MultiPlatformKeyCode _yesHintKey;
        [SerializeField] private MultiPlatformKeyCode _noHintKey;
        [SerializeField] private MultiPlatformKeyCode _skipHintKey;
        [SerializeField] private MultiPlatformKeyCode _confirmHintKey;
        [SerializeField] private MultiPlatformKeyCode _exitHintKey;
    
        [Header("Inventory")] 
        [SerializeField] private MultiPlatformKeyCode _inventoryKey;
        [SerializeField] private MultiPlatformKeyCode _useKey;
        [SerializeField] private MultiPlatformKeyCode _backViewKey;
        [SerializeField] private MultiPlatformKeyCode _inspectViewKey;
        [SerializeField] private MultiPlatformKeyCode _increaseSizeKey;
        [SerializeField] private MultiPlatformKeyCode _reduceSizeKey;

        [Header("Player Controls")] 
        [SerializeField] private MultiPlatformKeyCode _crouchKey;
        [SerializeField] private MultiPlatformKeyCode _jumpKey;
        [SerializeField] private MultiPlatformKeyCode _actionKey;
        [SerializeField] private MultiPlatformKeyCode _extraActionKey;

        #endregion

        #region PrivateVars

        private static InputSystem _instance;

        #endregion

    
        private void Awake()
        {   
            ReferencePlayerInputs();
            ReferenceInventoryInputs();
            ReferenceHintsInputs();
        }

        private void ReferencePlayerInputs()
        {
            KeyActionPressed = this.UpdateAsObservable().Where(_ => CanMove()).Where(_ => _actionKey.GetKey());
            KeyExtraActionPressed = this.UpdateAsObservable().Where(_ => CanMove()).Where(_ => _extraActionKey.GetKey());

            KeyActionPressedDown = this.UpdateAsObservable().Where(_ => CanMove()).Where(_ => _actionKey.GetKeyDown());

            KeyCrouchPressed = this.UpdateAsObservable().Where(_ => CanJumpAndCrouch()).Select(_ => _crouchKey.GetKey());
            KeyJumpPressedDown = this.UpdateAsObservable().Where(_ => CanJumpAndCrouch()).Where(_ => _jumpKey.GetKeyDown());
            
            KeyRunPressedDown = this.UpdateAsObservable().Where(_ => CanMove()).Select(_ => Input.GetAxis("PlayerRun"));
            
            HorizontalAxis = this.UpdateAsObservable().Select(_ => Input.GetAxis("Horizontal"));
            VerticalAxis = this.UpdateAsObservable().Select(_ => Input.GetAxis("Vertical"));
            
            RightStickX = this.UpdateAsObservable().Select(_ => Input.GetAxis("Right Stick X"));
            RightStickY = this.UpdateAsObservable().Select(_ => Input.GetAxis("Right Stick Y"));
        }

        private void ReferenceInventoryInputs()
        {
            KeyInventoryPressedDown = this.UpdateAsObservable().Where(_ => CanOpenInventory()).Where(_ => _inventoryKey.GetKeyDown());
        
            KeyInspectPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => _inspectViewKey.GetKeyDown());
            KeyBackViewPressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => _backViewKey.GetKeyDown());
            
            KeyUsePressedDown = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => _useKey.GetKeyDown());

            KeyUpArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetAxis("Vertical") > 0.2f);
            KeyDownArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetAxis("Vertical") < -0.2f);
            KeyLeftArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetAxis("Horizontal") < -0.2f);
            KeyRightArrowPressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => Input.GetAxis("Horizontal") > 0.2f);
        
            KeyReduceSizePressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => _reduceSizeKey.GetKey());
            KeyIncreaseSizePressed = this.UpdateAsObservable().Where(_ => IsUiActive).Where(_ => _increaseSizeKey.GetKey());
        }

        private void ReferenceHintsInputs()
        {
            KeyYesHintPressedDown = this.UpdateAsObservable().Where(_ => _yesHintKey.GetKeyDown());
            KeyNoHintPressedDown = this.UpdateAsObservable().Where(_ => _noHintKey.GetKeyDown());
            KeySkipHintPressedDown = this.UpdateAsObservable().Where(_ => _skipHintKey.GetKeyDown());
            KeyConfirmHintPressedDown = this.UpdateAsObservable().Where(_ => _confirmHintKey.GetKeyDown());
            KeyExitHintPressedDown = this.UpdateAsObservable().Where(_ => _exitHintKey.GetKeyDown());
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

    [Serializable]
    public class MultiPlatformKeyCode
    {
        [SerializeField] private KeyCode _pc;
        [SerializeField] private KeyCode _xbox;
        [SerializeField] private KeyCode _ps4;

        public bool GetKeyDown()
        {
            return Input.GetKeyDown(_pc) || Input.GetKeyDown(_xbox);
        }
        
        public bool GetKey()
        {
            return Input.GetKey(_pc) || Input.GetKey(_xbox);
        }
    }
    
    [CustomPropertyDrawer (typeof (MultiPlatformKeyCode))]
    public class MultiPlatformKeyCodeDrawer : PropertyDrawer {    
    
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty (position, label, property);
            
            position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);
            var buttonWidth = position.width / 3;
            var pcRect = new Rect (position.x, position.y, buttonWidth - 10, position.height);
            var xboxRect = new Rect (position.x + buttonWidth, position.y, buttonWidth - 10, position.height);
            var ps4Rect = new Rect (position.x + buttonWidth * 2, position.y, buttonWidth - 10, position.height);
            var indent = EditorGUI.indentLevel;
            
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField (pcRect, property.FindPropertyRelative ("_pc"), GUIContent.none);
            EditorGUI.PropertyField (xboxRect, property.FindPropertyRelative ("_xbox"), GUIContent.none);
            EditorGUI.PropertyField (ps4Rect, property.FindPropertyRelative ("_ps4"), GUIContent.none);
            EditorGUI.indentLevel = indent;
        
            EditorGUI.EndProperty ();
        }
    }
}