using System;
using UnityEngine;

public class Vegetation : BaseHealthAction
{
    

    protected override void OnKeyActionPressed(Health playerHealth)
    {
        if (!Health.CanReduce() || !playerHealth.CanEnhance()) return;

        ReduceHealth(Health.ReactivePercent.Value);
        playerHealth.Enhance(_reducedHealthPerTime);
    }

    protected override void OnKeyExtraActionPressed(Health playerHealth)
    {
        if (!Health.CanEnhance() || !playerHealth.CanReduce()) return;

        EnhanceHealth(Health.ReactivePercent.Value);
        playerHealth.Reduce(_reducedHealthPerTime);
    }

    private void EnhanceHealth(float percent)
    {
        Health.Enhance(_reducedHealthPerTime);
        SetMushroomParticlesEmission(percent);
        SetMushroomLightAndAnimation(percent);
        SetMeshColor(percent);
    }

    private void ReduceHealth(float percent)
    {
        Health.Reduce(_reducedHealthPerTime);
        SetMushroomLightAndAnimation(percent);
        SetMushroomParticlesEmission(percent);
        SetMeshColor(percent);
    }

    private void SetMeshColor(float percent)
    {   
        _skinnedMesh.material.SetColor("_EmissionColor", Color.Lerp(_minHealthMeshColor, _maxHealthMeshColor, percent));
    }

    private void SetMushroomLightAndAnimation(float percent)
    {
        _light.intensity = percent * _maxIntensity;
        if (Animator != null) Animator.Play(_animationClipName, 0, 1 - percent);
    }

    private void SetMushroomParticlesEmission(float percent)
    {
        if (_particles != null) _particles.emissionRate = percent * _maxEmission;
    }

    private void Start()
    {
        CheckSensitiveFields();
        
        var percent = Health.ReactivePercent.Value;
        
        SetMushroomLightAndAnimation(percent);
        SetMushroomParticlesEmission(percent);
        SetMeshColor(percent);
    }

    private void CheckSensitiveFields()
    {
        if (Animator != null && _animationClipName.Equals("")) 
            throw new NullReferenceException("Animation is empty for Vegetation");
    }

    #region half private

    [SerializeField] private float _reducedHealthPerTime = 0.1f;
    [ColorUsageAttribute(false,true)] [SerializeField] private Color _maxHealthMeshColor;
    [ColorUsageAttribute(false,true)] [SerializeField] private Color _minHealthMeshColor;

    [Header("Light")] 
    [SerializeField] private Light _light;
    [SerializeField] private float _maxEmission;
    [SerializeField] private float _maxIntensity;

    [Header("Particles")] 
    [SerializeField] private ParticleSystem _particles;

    [Header("Mesh Color")] 
    [SerializeField] private SkinnedMeshRenderer _skinnedMesh;

    [Header("Animation")] 
    [SerializeField] private string _animationClipName;
    
    #endregion
}