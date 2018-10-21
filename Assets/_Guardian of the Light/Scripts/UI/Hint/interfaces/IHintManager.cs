using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.UI.Hint.interfaces
{
    public interface IHintManager
    {
        void ShowHint(HintType type, string text);
        void ShowHint(HintType type, KeyCode keyCode);
    }
}