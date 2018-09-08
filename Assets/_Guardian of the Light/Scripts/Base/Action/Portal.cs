using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : BaseAction
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _startPointNumber;

    protected override void OnKeyActionPressedDown()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneBundleSystem.StartPointNumber = _startPointNumber;

        SceneManager.LoadSceneAsync(_sceneName)
            .AsAsyncOperationObservable()
            .Do(x => Debug.Log($"Current progress = {x.progress}%"))
            .Subscribe(x => Debug.Log($"Is Done = {x.isDone}%"));

    }
}