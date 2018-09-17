﻿using UniRx;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    private Health _health;
    [SerializeField] private Light _light;
    [SerializeField] private float _lightMaxRange = 8.5f;

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
        _light.range = percent * _lightMaxRange;
    }
}