﻿using UniRx;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _maxValue;
    [SerializeField] private float _value;

    public ReactiveProperty<float> ReactivePercent;


    public bool CanEnhance()
    {
        return ReactivePercent.Value < 1f;
    }

    public bool CanReduce()
    {
        return ReactivePercent.Value > 0;
    }

    public void Reduce(float value)
    {
        _value = Mathf.Clamp(_value - value, 0, _maxValue);
        ReactivePercent.Value = _value / _maxValue;
    }

    public void Enhance(float value)
    {
        _value = Mathf.Clamp(_value + value, 0, _maxValue);
        ReactivePercent.Value = _value / _maxValue;
    }

    private void Awake()
    {
        ReactivePercent = new ReactiveProperty<float>(_value / _maxValue);
    }
}