using DG.Tweening;
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
            _vignette.Collapse()
                .OnComplete(() => LoadScene());
        }
    
        private void LoadScene()
        {
            SceneBundleSystem.SpawnPointNumber = _spawnPointNumber;   
            SceneManager.LoadSceneAsync(_sceneName)
                .AsObservable()
                .DoOnCompleted(() => _vignette.Expand())
                .Subscribe(operation => Debug.Log($"Portal: operation.progress -> {operation.progress}"));

        }

        protected override void Awake()
        {
            base.Awake();
            _vignette = FindObjectOfType<Vignette>().GetComponent<Vignette>();
        }
    }
}