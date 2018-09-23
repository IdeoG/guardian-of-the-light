using UnityEngine;

[ExecuteInEditMode] 
public class GameController : MonoBehaviour
{
    private void Awake()
    {
        QualitySettings.vSyncCount = (int) _vSyncCount;
        RenderSettings.fogColor = _fogColor;
        Camera.main.backgroundColor = _fogColor;
    }

    private void OnValidate()
    {
        RenderSettings.fogColor = _fogColor;
        Camera.main.backgroundColor = _fogColor;
    }

    #region half private vars

    [SerializeField] private VSyncType _vSyncCount = VSyncType.EveryVSync60Fps;
    [SerializeField] private Color _fogColor;

        #endregion
}

public enum VSyncType
{
    EveryVSync60Fps = 1,
    EverySecondVSync30Fps = 2,
    NoVSync = 0
}