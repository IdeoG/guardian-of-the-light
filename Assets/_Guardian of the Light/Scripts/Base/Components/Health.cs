using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float MaxValue;
    [SerializeField] public float Value;


    public bool CanEnhance()
    {
        return Value < MaxValue;
    }

    public bool CanReduce()
    {
        return Value > 0;
    }

    public void Reduce(float value)
    {
        Value = Mathf.Clamp(Value - value, 0, MaxValue);
    }

    public void Enhance(float value)
    {
        Value = Mathf.Clamp(Value + value, 0, MaxValue);
    }

    private void Awake()
    {
        Value = Value > 0 ? Value : MaxValue;
    }
}