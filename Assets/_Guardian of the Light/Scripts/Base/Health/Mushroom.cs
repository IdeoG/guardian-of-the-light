using UnityEngine;

/** TODO: Mushroom -> Inspector View Improvements
 * Два поля в инспекторе
 * 1. Начальный цвет для гриба - 100%
 * 2. Конечный цвет для гриба - 0%
 */
public class Mushroom : BaseHealthAction
{
    [ColorUsageAttribute(true,true)] [SerializeField] private Color _maxHealthMeshColor;
    [ColorUsageAttribute(true,true)] [SerializeField] private Color _minHealthMeshColor;

    [Header("Mushroom Light")] [SerializeField]
    private Light _light;

    [SerializeField] private float _maxEmission;
    [SerializeField] private float _maxIntensity;

    [Header("Mushroom Particles")] 
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private float _reducedHealthPerTime = 0.1f;

    [Header("Mushroom Mesh Color")] [SerializeField]
    private SkinnedMeshRenderer _skinnedMesh;

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
//        EnhanceMeshColor();
    }

    private void ReduceHealth(float percent)
    {
        Health.Reduce(_reducedHealthPerTime);
        SetMushroomLightAndAnimation(percent);
        SetMushroomParticlesEmission(percent);
        SetMeshColor(percent);
//        ReduceMeshColor();
    }

    private void SetMeshColor(float percent)
    {
        var color = Color.Lerp(_minHealthMeshColor, _maxHealthMeshColor, percent);
        
        _skinnedMesh.material.SetColor("_EmissionColor", color);
    }
    
    private void EnhanceMeshColor()
    {
        var color = _skinnedMesh.material.GetColor("_EmissionColor");
        var deltaColor = 0.08f * Mathf.LinearToGammaSpace(_reducedHealthPerTime);

        color = color * (1 + deltaColor);
        _skinnedMesh.material.SetColor("_EmissionColor", color);
    }

    private void ReduceMeshColor()
    {
        var color = _skinnedMesh.material.GetColor("_EmissionColor");
        var deltaColor = 0.04f * Mathf.LinearToGammaSpace(_reducedHealthPerTime);

        color = color * (1 - deltaColor);
        _skinnedMesh.material.SetColor("_EmissionColor", color);
    }

    private void SetMushroomLightAndAnimation(float percent)
    {
        _light.intensity = percent * _maxIntensity;
        Animator.Play("Mushroom", 0, 1 - percent);
    }

    private void SetMushroomParticlesEmission(float percent)
    {
        _particles.emissionRate = percent * _maxEmission;
    }
}