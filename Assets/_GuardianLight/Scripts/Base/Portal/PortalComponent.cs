using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalComponent : BaseActionBehaviour
{
    [SerializeField] private string _sceneName;
    [SerializeField] private int _startPointNumber;

    protected override void OnKeyPressedAction()
    {
        LoadScene();
    }

    private void LoadScene()
    {
        SceneBundle.StartPointNumber = _startPointNumber;
        SceneManager.LoadSceneAsync(_sceneName)
            .AsObservable()
            .Subscribe(x => Debug.Log(string.Format("Current progress = {0}%", x.progress)));
    }
}