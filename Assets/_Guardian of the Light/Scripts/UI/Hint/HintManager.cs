using System;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.UI.Hint.interfaces;

namespace _Guardian_of_the_Light.Scripts.UI.Hint
{
    public class HintManager : MonoBehaviour, IHintManager
    {
        [SerializeField] private GameObject _yesNoHintPanel;
        [SerializeField] private GameObject _skipHintPanel;
        [SerializeField] private GameObject _emptyHintPanel;
        [SerializeField] private GameObject _temporaryButtonHintPanel;

        private IDisposable _keyYesPressedDown;
        private IDisposable _keyNoPressedDown;
        private IDisposable _keySkipPressedDown;
        private IDisposable _keyTemporaryButtonPressedDown;

        private IYesNoHint _iYesNoHint;
        private IEmptyHint _iEmptyHint;
        private ISkipHint _iSkipHint;
        private ITemporaryButtonHint _iTemporaryButtonHint;
        
        
        public void ShowHint(HintType type, string text)
        {
            throw new System.NotImplementedException();
        }

        public void ShowHint(HintType type, KeyCode keyCode)
        {
            throw new System.NotImplementedException();
        }

        private void Awake()
        {
            var controller = FindObjectOfType<HintController>().GetComponent<HintController>();
            _iYesNoHint = controller;
            _iEmptyHint = controller;
            _iSkipHint = controller;
            _iTemporaryButtonHint = controller;
        }
    }
}
