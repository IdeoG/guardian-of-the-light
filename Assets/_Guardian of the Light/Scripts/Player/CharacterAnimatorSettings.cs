using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Random = System.Random;

namespace _Guardian_of_the_Light.Scripts.Player
{
    public class CharacterAnimatorSettings : MonoBehaviour
    {
        [Header("Eyes Settings")] 
        [SerializeField] private int eyesHaltMinDurationMs;
        [SerializeField] private int eyesHaltMaxDurationMs;
        [SerializeField] private int eyesLayerIndex;
        [SerializeField] private string eyesHaltShortName;
        
        private Animator _animator;
        private static readonly int EyesHaltFrequency = Animator.StringToHash("eyesHaltFrequency");

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            var stateMachineTriggers = _animator.GetBehaviours<ObservableStateMachineTrigger>();

            foreach (var stateMachineTrigger in stateMachineTriggers)
            {
                stateMachineTrigger.OnStateExitAsObservable()
                    .Where(info => info.LayerIndex == eyesLayerIndex)
                    .Where(info => info.StateInfo.shortNameHash.Equals(Animator.StringToHash(eyesHaltShortName)))
                    .Subscribe(_ => SetEyesHaltFrequency());
            }
        }

        private void SetEyesHaltFrequency()
        {
            var eyesHaltRandomDurationMs = new Random().Next(eyesHaltMinDurationMs, eyesHaltMaxDurationMs);
            var eyesHaltFrequency = 1000f / eyesHaltRandomDurationMs;
            
            _animator.SetFloat(EyesHaltFrequency, eyesHaltFrequency);
        }
    }
}