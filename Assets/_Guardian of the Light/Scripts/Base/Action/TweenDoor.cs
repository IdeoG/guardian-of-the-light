using DG.Tweening;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Player;
using _Guardian_of_the_Light.Scripts.Systems;

namespace _Guardian_of_the_Light.Scripts.Base.Action
{
    public class TweenDoor : BaseAction
    {
        [SerializeField] private Vector3 _angle;
        [SerializeField] private float _duration;

        protected override void OnKeyActionPressedDown()
        {
            InputSystem.Instance.IsAnimationPlaying = true;
            GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>().LockInput = true;
            
            transform.DOLocalRotate(_angle, _duration)
                .OnComplete(() =>
                {
                    InputSystem.Instance.IsAnimationPlaying = false;
                    GameManagerSystem.Instance.Player.GetComponent<ThirdPersonUserControl>().LockInput = false;
                });
            
            var colliders = GetComponents<Collider>();
            foreach (var collider in colliders)
            {
                if (!collider.isTrigger) continue;
                
                collider.enabled = false;
            }
            
            OnTriggerExit(null);
        }
    }
}