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
        
        public void ShowHint(HintType type, string text)
        {
            throw new System.NotImplementedException();
        }

        public void ShowHint(HintType type, KeyCode keyCode)
        {
            throw new System.NotImplementedException();
        }
    }
}