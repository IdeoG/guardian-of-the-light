using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float MaxHealth;
    [SerializeField] public float Value;


    public bool CanEnhance()
    {
        return Value < MaxHealth;
    }

    public bool CanReduce()
    {
        return Value > 0;
    }

    public void Reduce(float value)
    {
        Value = Mathf.Clamp(Value - value, 0, MaxHealth);
    }

    public void Enhance(float value)
    {
        Value = Mathf.Clamp(Value + value, 0, MaxHealth);
    }

    private void Awake()
    {
        Value = Value > 0 ? Value : MaxHealth;
    }
}