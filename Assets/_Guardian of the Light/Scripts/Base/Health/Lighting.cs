using UniRx;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    [ColorUsageAttribute(true,true)] [SerializeField] private Color _maxHealthMeshColor;
    [ColorUsageAttribute(true,true)] [SerializeField] private Color _minHealthMeshColor;
    
    [SerializeField] private Light _light;
    [SerializeField] private float _lightMaxRange = 8.5f;
    
    [Header("Mushroom Mesh Color")] [SerializeField]
    private SkinnedMeshRenderer _skinnedMesh;
    
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
        _light.range = percent * _lightMaxRange;
    }

    private void SetMeshColor(float percent)
    {
        var color = Color.Lerp(_minHealthMeshColor, _maxHealthMeshColor, percent);
        
        _skinnedMesh.material.SetColor("_EmissionColor", color);
    }
    
}