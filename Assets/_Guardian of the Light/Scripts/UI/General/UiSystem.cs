using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.UI.General
{
    public class UiSystem : MonoBehaviour
    {
        
        private void Awake()
        {   
            DontDestroyOnLoad(gameObject);
        }
        
    }
}