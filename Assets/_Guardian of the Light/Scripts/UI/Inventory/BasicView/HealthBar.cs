using System;
using System.Globalization;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _filledImage;
    [SerializeField] private Text _text;

    private IDisposable _disposablePlayerPercent;
    private IDisposable _disposablePlayerHealth;
    
    
    private void OnEnable()
    {
        _disposablePlayerPercent = GameManagerSystem.Instance.Player.GetComponent<Health>().ReactivePercent
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(x => _filledImage.fillAmount = x );
        
        _disposablePlayerHealth = GameManagerSystem.Instance.Player.GetComponent<Health>().ReactiveHealth
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(x => _text.text = x.ToString() );
    }

    private void OnDisable()
    {
        _disposablePlayerPercent.Dispose();
        _disposablePlayerHealth.Dispose();
    }
}