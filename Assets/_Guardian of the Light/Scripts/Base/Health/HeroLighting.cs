using UniRx;
using UnityEngine;

public class HeroLighting : MonoBehaviour
{
    private Health _health;


    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Start()
    {
        _health.ReactivePercent.ObserveEveryValueChanged(x => x.Value)
            .Subscribe(x => OnHealthChanged(x));

        OnHealthChanged(_health.ReactivePercent.Value);
    }

    private void OnHealthChanged(float percent)
    {
        SetLightRange(percent);
        SetMeshColor(percent);
    }

    private void SetLightRange(float percent)
    {
        _light.range = Mathf.Lerp(_minLightRange, _maxLightRange, percent);
    }

    private void SetMeshColor(float percent)
    {
        var bodyColor = Color.Lerp(_minHealthBodyMeshColor, _maxHealthBodyMeshColor, percent);
        var eyesColor = Color.Lerp(_minHealthEyesMeshColor, _maxHealthEyesMeshColor, percent);
        
        _bodySkinnedMesh.material.SetColor("_EmissionColor", bodyColor);
        _eyesSkinnedMesh.material.SetColor("_EmissionColor", eyesColor);
    }

    #region half private
    
    [Header("Colors")]
    [ColorUsageAttribute(true, true)] [SerializeField]
    private Color _maxHealthBodyMeshColor;

    [ColorUsageAttribute(true, true)] [SerializeField]
    private Color _minHealthBodyMeshColor;
    
    [ColorUsageAttribute(true, true)] [SerializeField]
    private Color _maxHealthEyesMeshColor;

    [ColorUsageAttribute(true, true)] [SerializeField]
    private Color _minHealthEyesMeshColor;

    [Header("Light")]
    [SerializeField] private Light _light;
    [SerializeField] private float _minLightRange = 4f;
    [SerializeField] private float _maxLightRange = 8.5f;

    [Header("Meshes")] 
    [SerializeField] private SkinnedMeshRenderer _bodySkinnedMesh;
    [SerializeField] private SkinnedMeshRenderer _eyesSkinnedMesh;

        #endregion
}