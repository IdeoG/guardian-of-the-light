using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using _Guardian_of_the_Light.Scripts.Systems;

namespace _Guardian_of_the_Light.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float forwardForceMultiplier;
        [SerializeField] private float runForceMultiplier;
        [SerializeField] private float jumpForceMultiplier;
        [SerializeField] private float smoothTime;

        private Rigidbody _rigidbody;
        private Animator _animator;
        private Transform _camera;
        private Vector2 _move;
        private static readonly int ForwardAmount = Animator.StringToHash("forwardAmount");

        private const float StationaryTurnSpeed = 180;
        private const float MovingTurnSpeed = 360;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _camera = GGameManager.Instance.MainCamera.transform;

            // Crutch: I need smooth stop, but have no idea how to impl it 
            InputSystem.Instance.IsUiActive.Where(state => state)
                .Subscribe(state => _animator.SetFloat(ForwardAmount, 0f));

            Observable
                .ZipLatest(InputSystem.Instance.VerticalAxis,InputSystem.Instance.HorizontalAxis,
                    InputSystem.Instance.PlayerRunAxis)
                .Subscribe(list =>
                {
                    var v = list[0];
                    var h = list[1];
                    var runAmount = list[2];

                    var rawMove = new Vector2(v, h) * (1 + runAmount * runForceMultiplier);
                    _move = Vector2.Lerp(_move, rawMove, smoothTime * Time.deltaTime);
                    
                    var axisValue = 0.5f * Mathf.Sqrt(Mathf.Max(_move.x * _move.x, _move.y * _move.y));
                    var (turnAmount, forwardAmount) = CalculateMoveParams(_move.x, _move.y);
                    ApplyForwardForce(axisValue);
                    ApplyExtraTurnRotation(turnAmount, forwardAmount);
                });

            InputSystem.Instance.KeyJumpPressedDown.Subscribe(_ => ApplyJumpForce());
        }

        private void ApplyJumpForce()
        {
            _rigidbody.AddRelativeForce(jumpForceMultiplier * 100_000 * Time.deltaTime * Vector3.up);
        }

        private (float, float) CalculateMoveParams(float vertical, float horizontal)
        {
            var run = 1f; // Crutch: Move to runAmount! 
            var camForward = Vector3.Scale(_camera.forward, new Vector3(1, 0, 1)).normalized;
            var move = vertical * camForward + horizontal * _camera.right;

            if (Math.Abs(move.magnitude - 1f) > 0.01f)
                move.Normalize();

            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, Vector3.up);

            var turnAmount = Mathf.Atan2(move.x, move.z);
            var forwardAmount = move.z * (0.4f * run + 0.6f);

            return (turnAmount, forwardAmount);
        }

        public void OnAnimatorMove()
        {
            var moveSpeedMultiplier = 2.5f;
            var v = _animator.deltaPosition * moveSpeedMultiplier / Time.deltaTime;
            v.y = _rigidbody.velocity.y;
            _rigidbody.velocity = v;
        }

        private void ApplyForwardForce(float forwardAmount)
        {
            _rigidbody.AddRelativeForce(forwardForceMultiplier * 100_000 * forwardAmount * Time.deltaTime *
                                        Vector3.forward);
            _animator.SetFloat(ForwardAmount, forwardAmount);
        }

        private void ApplyExtraTurnRotation(float turnAmount, float forwardAmount)
        {
            var turnSpeed = Mathf.Lerp(StationaryTurnSpeed, MovingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }
    }
}