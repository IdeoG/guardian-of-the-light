using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using _Guardian_of_the_Light.Scripts.UI.Vignette;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Hint
{
    public class Portal : UiHint
    {   
        [SerializeField] private int _spawnPointNumber;
        [SerializeField] private string _sceneName;
        
        private Vignette _vignette;

        public override void DestroyItem()
        {
            Observable.FromCoroutine(HideVignette)
                .SelectMany(LoadScene)
                .Subscribe();
        }
    
        private IEnumerator HideVignette() 
        {
            yield return _vignette.Collapse();
        }

        private IEnumerator LoadScene()
        {
            SceneBundleSystem.SpawnPointNumber = _spawnPointNumber;   
            yield return SceneManager.LoadSceneAsync(_sceneName);
        }

        protected override void Awake()
        {
            base.Awake();
            _vignette = FindObjectOfType<Vignette>().GetComponent<Vignette>();
        }
    }
}