using System;
using UniRx;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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
    private KeyCode _crouchKeyCode;

    private IDisposable _jumpDisposable;

    private void Start()
    {
        _crouchKeyCode = InputSystem.Instance._crouchKey;
        _jumpDisposable = InputSystem.Instance.KeyJumpPressedDown
            .Where(_ => !InputSystem.Instance.IsInInventory)
            .Subscribe(_ => m_Jump = true)
            .AddTo(this);

        m_Character = GetComponent<ThirdPersonCharacter>();
        m_Cam = Camera.main.transform;
    }

    private void OnDisable()
    {
        _jumpDisposable.Dispose();

        m_Character.Move(Vector3.zero, false, false);
        m_Jump = false;
    }

    private void FixedUpdate()
    {
        var h = CrossPlatformInputManager.GetAxis("Horizontal");
        var v = CrossPlatformInputManager.GetAxis("Vertical");
        var crouch = Input.GetKey(_crouchKeyCode);

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
            m_Character.Move(m_Move, crouch, m_Jump);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }

        m_Jump = false;
    }
}