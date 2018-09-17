using UnityEngine;

public class HealthInventoryItem : BaseHealthInventoryAction
{
    [SerializeField] private float _healthMultiplier = 1f;

    protected override void OnKeyActionPressed(Health playerHealth)
    {
        if (!Health.CanReduce() || !playerHealth.CanEnhance()) return;

        ReduceHealth();
        playerHealth.Enhance(_healthMultiplier * Time.deltaTime);
    }

    protected override void OnKeyExtraActionPressed(Health playerHealth)
    {
        if (!Health.CanEnhance() || !playerHealth.CanReduce()) return;

        EnhanceHealth();
        playerHealth.Reduce(_healthMultiplier * Time.deltaTime);
    }

    private void EnhanceHealth()
    {
        Health.Enhance(_healthMultiplier * Time.deltaTime);
        Slider.value = Health.ReactivePercent.Value;
    }

    private void ReduceHealth()
    {
        Health.Reduce(_healthMultiplier * Time.deltaTime);
        Slider.value = Health.ReactivePercent.Value;
    }
}