using System;
using UniRx;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using _Guardian_of_the_Light.Scripts.Systems;

namespace _Guardian_of_the_Light.Scripts.Player
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private static bool LockInput => !InputSystem.Instance.CanMove();

        private ThirdPersonCharacter m_Character;
        private Transform m_Cam;
        private Vector3 m_CamForward;
        private Vector3 m_Move;
        private bool m_Jump;
        private bool m_Crouch;
        private float m_Run;

        private IDisposable _crouchDisposable;
        private IDisposable _jumpDisposable;
        private IDisposable _runDisposable;


        private void Awake()
        {
            m_Character = GetComponent<ThirdPersonCharacter>();
            m_Cam = UnityEngine.Camera.main.transform;
        }

        private void OnEnable()
        {
            _crouchDisposable = InputSystem.Instance.KeyCrouchPressed
                .Subscribe(x => m_Crouch = x)
                .AddTo(this);

            _jumpDisposable = InputSystem.Instance.KeyJumpPressedDown
                .Subscribe(_ => m_Jump = true)
                .AddTo(this);
            
            _runDisposable = InputSystem.Instance.PlayerRunAxis
                .Subscribe(x => m_Run = x)
                .AddTo(this);
        }

        private void OnDisable()
        {
            _crouchDisposable.Dispose();
            _jumpDisposable.Dispose();
            _runDisposable.Dispose();

            m_Character.Move(Vector3.zero, false, false, 0.0f);
            m_Jump = false;
            m_Run = 0.0f;
        }

        private void FixedUpdate()
        {
            var h = CrossPlatformInputManager.GetAxis("Horizontal");
            var v = CrossPlatformInputManager.GetAxis("Vertical");

            if (m_Cam != null)
            {
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right; // TODO: Remove direction from camera. It shouldn't be referenced to camera transform
            }
            else
            {
                m_Move = v * Vector3.forward + h * Vector3.right;
            }


            if (!LockInput)
            {   
                m_Character.Move(m_Move, m_Crouch, m_Jump, m_Run);
            }
            else
            {
                m_Character.Move(Vector3.zero, false, false, 0.0f);
            }

            m_Jump = false;
        }
    }
}