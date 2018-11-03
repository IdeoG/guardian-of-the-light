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
        public bool LockInput;

        private ThirdPersonCharacter m_Character;
        private Transform m_Cam;
        private Vector3 m_CamForward;
        private Vector3 m_Move;
        private bool m_Jump;
        private bool m_Crouch;

        private IDisposable _crouchDisposable;
        private IDisposable _jumpDisposable;


        private void Awake()
        {
            m_Character = GetComponent<ThirdPersonCharacter>();
            m_Cam = Camera.main.transform;
        }

        private void OnEnable()
        {
            _crouchDisposable = InputSystem.Instance.KeyCrouchPressed
                .Subscribe(x => m_Crouch = x)
                .AddTo(this);

            _jumpDisposable = InputSystem.Instance.KeyJumpPressedDown
                .Subscribe(_ => m_Jump = true)
                .AddTo(this);
        }

        private void OnDisable()
        {
            _crouchDisposable.Dispose();
            _jumpDisposable.Dispose();

            m_Character.Move(Vector3.zero, false, false);
            m_Jump = false;
        }

        private void FixedUpdate()
        {
            var h = CrossPlatformInputManager.GetAxis("Horizontal");
            var v = CrossPlatformInputManager.GetAxis("Vertical");

            if (m_Cam != null)
            {
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                m_Move = v * Vector3.forward + h * Vector3.right;
            }

            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;

            if (!LockInput)
            {
                m_Character.Move(m_Move, m_Crouch, m_Jump);
            }
            else
            {
                m_Character.Move(Vector3.zero, false, false);
            }

            m_Jump = false;
        }
    }
}