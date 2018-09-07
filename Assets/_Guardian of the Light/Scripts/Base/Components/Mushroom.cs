using UnityEngine;

[RequireComponent(typeof(Health))]
public class Mushroom : BaseHealthAction
{
    [SerializeField] private float _consumedHealthPerFrame;

    private SkinnedMeshRenderer _skinnedMesh;
    private float _skinnedMeshK;
    private Health _health;

    protected override void OnKeyActionPressed(Health playerHealth)
    {
        if (!_health.CanReduce() || !playerHealth.CanEnhance()) return;

        _health.Reduce(_consumedHealthPerFrame);
        _skinnedMesh.material.ReduceEmissionColorAlpha(0.3f * _consumedHealthPerFrame / _health.MaxValue);
        _skinnedMesh.SetBlendShapeWeight(0, _skinnedMesh.GetBlendShapeWeight(0) + _consumedHealthPerFrame * _skinnedMeshK);
        
        playerHealth.Enhance(_consumedHealthPerFrame);
    }

    protected override void OnKeyExtraActionPressed(Health playerHealth)
    {
        if (!_health.CanEnhance() || !playerHealth.CanReduce()) return;

        _health.Enhance(_consumedHealthPerFrame);
        _skinnedMesh.material.EnhanceEmissionColorAlpha(0.3f * _consumedHealthPerFrame / _health.MaxValue);
        _skinnedMesh.SetBlendShapeWeight(0, _skinnedMesh.GetBlendShapeWeight(0) - _consumedHealthPerFrame * _skinnedMeshK);
        
        playerHealth.Reduce(_consumedHealthPerFrame);
    }

    private void Awake()
    {
        _health = GetComponent<Health>();
        _skinnedMesh = GetComponent<SkinnedMeshRenderer>();

        _skinnedMeshK = 100f / _health.MaxValue;
    }

}