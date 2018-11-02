using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using _Guardian_of_the_Light.Scripts.UI.Hint.interfaces;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    public class HintManager : MonoBehaviour, IHintManager
    {
        [SerializeField] private int _emptyHintActiveTimeMs;

        [Header("Panels")] 
        [SerializeField] private GameObject _yesNoHintPanel;
        [SerializeField] private GameObject _skipHintPanel;
        [SerializeField] private GameObject _emptyHintPanel;
        [SerializeField] private GameObject _multipleChoicePanel;
        [SerializeField] private GameObject _temporaryButtonHintPanel;

        [Header("Extra Texts")] 
        [SerializeField] private Text _yesNoHintText;
        [SerializeField] private Text _skipHintText;
        [SerializeField] private Text _emptyHintText;
        [SerializeField] private List<Text> _multipleChoiceHintText;

        [Header("Extra Tools")]
        [SerializeField] private List<GameObject> _multipleChoiceCursor;
        
        private IDisposable _keyYesPressedDown;
        private IDisposable _keyNoPressedDown;
        private IDisposable _keySkipPressedDown;
        private IDisposable _keyTemporaryButtonPressedDown;
        
        private IDisposable _keyConfirmPressedDown;
        private IDisposable _keyExitPressedDown;
        private IDisposable _keyUpPressedDown;
        private IDisposable _keyDownPressedDown;

        private IYesNoHint _iYesNoHint;
        private IEmptyHint _iEmptyHint;
        private ISkipHint _iSkipHint;
        private ITemporaryButtonHint _iTemporaryButtonHint;
        private IMultipleChoiceHint _iMultipleChoiceHint;


        public void ShowHint(HintType type, string text)
        {
            switch (type)
            {
                case HintType.YesNo:
                    ShowYesNoHintPanel(text);
                    break;
                case HintType.Empty:
                    ShowEmptyHintPanel(text);
                    break;
                case HintType.Skip:
                    ShowSkipHintPanel(text);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void ShowHint(HintType type, KeyCode keyCode)
        {
            switch (type)
            {
                case HintType.TemporaryButton:
                    throw new NotImplementedException(nameof(type));
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void ShowHint(HintType type, List<string> texts)
        {
            switch (type)
            {
                case HintType.MultipleChoice:
                    ShowMultipleChoiceHintPanel(texts);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private void ShowMultipleChoiceHintPanel(List<string> texts)
        {
            
            throw new NotImplementedException();
        }

        private void ShowSkipHintPanel(string text)
        {
            _skipHintPanel.SetActive(true);
            _skipHintText.text = text;

            _keySkipPressedDown = InputSystem.Instance.KeySkipHintPressedDown
                .Subscribe(_ =>
                {
                    _iSkipHint.OnSkipPressed();
                    _skipHintPanel.SetActive(false);

                    _keySkipPressedDown.Dispose();
                }).AddTo(this);
        }

        private void ShowEmptyHintPanel(string text)
        {
            _emptyHintPanel.SetActive(true);
            _emptyHintText.text = text;
            DelayedHideEmptyHintPanel();
        }

        private void ShowYesNoHintPanel(string text)
        {
            _yesNoHintPanel.SetActive(true);
            _yesNoHintText.text = text;

            _keyYesPressedDown = InputSystem.Instance.KeyYesHintPressedDown
                .Subscribe(_ =>
                {
                    _iYesNoHint.OnYesPressed();
                    _yesNoHintPanel.SetActive(false);

                    _keyNoPressedDown.Dispose();
                    _keyYesPressedDown.Dispose();
                }).AddTo(this);

            _keyNoPressedDown = InputSystem.Instance.KeyNoHintPressedDown
                .Subscribe(_ =>
                {
                    _iYesNoHint.OnNoPressed();
                    _yesNoHintPanel.SetActive(false);

                    _keyNoPressedDown.Dispose();
                    _keyYesPressedDown.Dispose();
                }).AddTo(this);
        }

        private async void DelayedHideEmptyHintPanel()
        {
            await Task.Delay(_emptyHintActiveTimeMs);

            _iEmptyHint.OnEmptyExpired();
            _emptyHintPanel.SetActive(false);
        }

        private void Awake()
        {
            var controller = FindObjectOfType<HintController>().GetComponent<HintController>();

            _iYesNoHint = controller;
            _iEmptyHint = controller;
            _iSkipHint = controller;
            _iTemporaryButtonHint = controller;
            _iMultipleChoiceHint = controller;
        }
    }
}