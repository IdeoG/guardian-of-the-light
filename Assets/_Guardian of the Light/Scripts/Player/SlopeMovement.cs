using UnityEngine;
using _Guardian_of_the_Light.Scripts.Extensions;

namespace _Guardian_of_the_Light.Scripts.Player
{
    // TODO: Not enough realistic movement
    public class SlopeMovement : MonoBehaviour
    {
        private void OnCollisionStay(Collision other)
        {
            var isStair = other.collider.tag.Equals("Stair");
            var isMoving = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")).magnitude > 0.1f;
            if (!isStair || !isMoving)
                return;
            
            var cachedPosition = _transform.position;
            foreach (var contactPoint in other.contacts)
            {
                var diff = contactPoint.point - cachedPosition;
                var isForwardLook = Mathf.Abs(Vector3.Angle(diff.With(y: 0), _transform.rotation * Vector3.forward)) < 30f;
                if (!(_thresholdStepHeight < diff.y) || !(diff.y < _maxStepHeight) || !isForwardLook) 
                    continue;
                
                _rigidbody.AddForce(Vector3.up * _force); // TODO: Add force according to the movingForce
                break;
            }
        }

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        [SerializeField] private float _maxStepHeight = 0.5f;
        [SerializeField] private float _thresholdStepHeight = 0.1f;
        [SerializeField] private float _force = 5000f;

        private Rigidbody _rigidbody;
        private Transform _transform;
    }
}