using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalComponent : BaseActionBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _startPointNumber;

    protected override void OnKeyActionPressedDown()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        SceneBundle.StartPointNumber = _startPointNumber;

        /** BUG: LoadScene
         * 1. Этот кусок кода работает неправильно. Показывает только загрузку на 1% и по завершению загрузки.
         * Нет детализации.
         * 2. Loading scene takes too much time to switch the scene. Loading time is about 1 second!!
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