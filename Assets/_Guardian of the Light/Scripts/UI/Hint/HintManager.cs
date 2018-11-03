using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using _Guardian_of_the_Light.Scripts.Extensions;
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
        [SerializeField] private List<Text> _multipleChoiceText;

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

        private int _choicesCount;
        private int _currentChoice;


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
            _multipleChoicePanel.SetActive(true);
            
            foreach (var cursor in _multipleChoiceCursor)
                cursor.SetActive(false);
            
            foreach (var text in _multipleChoiceText)
                text.gameObject.SetActive(false);

            _currentChoice = 0;
            _choicesCount = texts.Count;
            for (var ind = 0; ind < _choicesCount; ++ind)
            {
                _multipleChoiceText[ind].gameObject.SetActive(true);
                _multipleChoiceText[ind].text = texts[ind];
                _multipleChoiceText[ind].color = _multipleChoiceText[ind].color.With(a: 0.5f);
            }
            
            _multipleChoiceCursor[_currentChoice].SetActive(true);
            _multipleChoiceText[_currentChoice].color = _multipleChoiceText[_currentChoice].color.With(a: 0.5f);
            
            _keyExitPressedDown = InputSystem.Instance.KeyExitHintPressedDown
                .Subscribe(_ =>
                {
                    _iMultipleChoiceHint.OnExitPressed();
                    _multipleChoicePanel.SetActive(false);
                    
                    _keyExitPressedDown.Dispose();
                    _keyConfirmPressedDown.Dispose();
                    _keyUpPressedDown.Dispose();
                    _keyDownPressedDown.Dispose();
                }).AddTo(this);
            
            _keyConfirmPressedDown = InputSystem.Instance.KeyConfirmHintPressedDown
                .Subscribe(_ =>
                {
                    _iMultipleChoiceHint.OnConfirmPressed(_currentChoice);
                    _multipleChoicePanel.SetActive(false);
                    
                    _keyExitPressedDown.Dispose();
                    _keyConfirmPressedDown.Dispose();
                    _keyUpPressedDown.Dispose();
                    _keyDownPressedDown.Dispose();
                }).AddTo(this);
            
            _keyUpPressedDown = InputSystem.Instance.KeyUpArrowPressed
                .Subscribe(_ =>
                {
                    if (_currentChoice == 0) return;
                    
                    _multipleChoiceCursor[_currentChoice].SetActive(false);
                    _multipleChoiceText[_currentChoice].color = _multipleChoiceText[_currentChoice].color.With(a: 0.5f);
                    _currentChoice--;
                    
                    _multipleChoiceCursor[_currentChoice].SetActive(true);
                    _multipleChoiceText[_currentChoice].color = _multipleChoiceText[_currentChoice].color.With(a: 1f);
                }).AddTo(this);
            
            _keyDownPressedDown = InputSystem.Instance.KeyDownArrowPressed
                .Subscribe(_ =>
                {
                    if (_currentChoice == _choicesCount - 1) return;
                    
                    _multipleChoiceCursor[_currentChoice].SetActive(false);
                    _multipleChoiceText[_currentChoice].color = _multipleChoiceText[_currentChoice].color.With(a: 0.5f);
                    _currentChoice++;
                    
                    _multipleChoiceCursor[_currentChoice].SetActive(true);
                    _multipleChoiceText[_currentChoice].color = _multipleChoiceText[_currentChoice].color.With(a: 1f);
                }).AddTo(this);
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
            var controller = FindObjectOfType<HintProvider>().GetComponent<HintProvider>();

            _iYesNoHint = controller;
            _iEmptyHint = controller;
            _iSkipHint = controller;
            _iTemporaryButtonHint = controller;
            _iMultipleChoiceHint = controller;
        }
    }
}