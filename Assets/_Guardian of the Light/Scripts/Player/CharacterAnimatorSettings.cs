using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.Player
{
    public class CharacterAnimatorSettings : MonoBehaviour
    {
        [Header("Eyes Settings")] 
        [SerializeField] private float eyesHaltMinDuration;
        [SerializeField] private float eyesHaltMaxDuration;
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
            var eyesHaltRandomDuration = Random.Range(eyesHaltMinDuration, eyesHaltMaxDuration);
            var eyesHaltFrequency = 1f / eyesHaltRandomDuration;
            
            _animator.SetFloat(EyesHaltFrequency, eyesHaltFrequency);
        }
    }
}