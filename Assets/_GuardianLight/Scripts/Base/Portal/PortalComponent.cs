using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalComponent : BaseActionBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _startPointNumber;

    protected override void OnKeyPressedAction()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        SceneBundle.StartPointNumber = _startPointNumber;
        
        /** BUG: LoadScene
         * Этот кусок кода работает неправильно. Показывает только загрузку на 1% и по завершению загрузки.
         * Нет детализации.
         */
        var operation = SceneManager.LoadSceneAsync(_sceneName);
//            .AsAsyncOperationObservable()
//            .Do(x => Debug.Log(string.Format("Current progress = {0}%", x.progress)))
//            .Subscribe(x => Debug.Log(string.Format("Is Done = {0}%", x.isDone)));

        while (!operation.isDone)
        {
            Debug.Log(operation.progress);

            yield return null;
        }
    }
    
}