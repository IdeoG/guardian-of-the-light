using System.Collections;
using UniRx;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.GameLogic.Hint
{
    public class Portal : UiHint
    {
        public override void DestroyItem()
        {
            Observable.FromCoroutine(HideVignette)
                .SelectMany(LoadScene)
                .SelectMany(ShowVignette)
                .Subscribe();
        }
    
        private IEnumerator HideVignette() 
        {
            Debug.Log("HideVignette");
            yield return null;
        }

        private IEnumerator LoadScene()
        {
            Debug.Log("LoadScene");
//            SceneBundleSystem.SpawnPointNumber = _spawnPointNumber;
//
//            SceneManager.LoadSceneAsync(_sceneName)
//                .AsAsyncOperationObservable()
//                .Do(x => Debug.Log($"Current progress = {x.progress}%"))
//                .Subscribe(x => Debug.Log($"Is Done = {x.isDone}%"));
            
            yield return null;
        }

        private IEnumerator ShowVignette()
        {
            Debug.Log("ShowVignette");
            yield return null;
        }
        
    }
}