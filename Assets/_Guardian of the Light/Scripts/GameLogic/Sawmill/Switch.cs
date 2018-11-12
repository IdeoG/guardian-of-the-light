using DG.Tweening;
using UnityEngine;

namespace _Guardian_of_the_Light.Scripts.Base.Action
{
    public class Switch : BaseAction
    {
        [SerializeField] private float _downAngle;
        [SerializeField] private float _downDurationTime;
        [SerializeField] private float _upDurationTime;

        protected override void OnKeyActionPressedDown()
        {
            transform.DOLocalRotate(new Vector3(_downAngle, 0, 0), _downDurationTime)
                .OnComplete(() => transform.DOLocalRotate(Vector3.zero, _upDurationTime));
        }
    }
}
