using UnityEngine;

[ExecuteInEditMode] 
public class GameSettings : MonoBehaviour
{
    private Camera _camera;
    
    private void Awake()
    {
        _camera = GGameManager.Instance.MainCamera;
        QualitySettings.vSyncCount = (int) vSyncCount;
        RenderSettings.fogColor = fogColor;
        _camera.backgroundColor = fogColor;
    }

    private void OnValidate()
    {
        RenderSettings.fogColor = fogColor;
        if (Camera.main != null) Camera.main.backgroundColor = fogColor;
    }

    #region half private vars

    [SerializeField] private VSyncType vSyncCount = VSyncType.EveryVSync60Fps;
    [SerializeField] private Color fogColor;

        #endregion
}

public enum VSyncType
{
    EveryVSync60Fps = 1,
    EverySecondVSync30Fps = 2,
    NoVSync = 0
}