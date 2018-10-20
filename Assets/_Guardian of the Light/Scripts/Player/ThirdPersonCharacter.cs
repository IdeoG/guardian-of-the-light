using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
    private float m_GravityMultiplier = 2f;
    private float m_GroundCheckDistance = 10f;
    private float m_JumpPower = 5f;
    [SerializeField] private float m_AnimSpeedMultiplier = 1.5f;
    [SerializeField] private float m_MoveSpeedMultiplier = 2.1f;
    private float m_StationaryTurnSpeed = 180;
    private float m_MovingTurnSpeed = 360;
    private float m_RunCycleLegOffset = 0.2f;
    
    private const float KHalf = 0.5f;
    private Animator m_Animator;
    private CapsuleCollider m_Capsule;
    private Vector3 m_CapsuleCenter;
    private float m_CapsuleHeight;
    private bool m_Crouching;
    private float m_ForwardAmount;
    private Vector3 m_GroundNormal;
    private bool m_IsGrounded;
    private float m_OrigGroundCheckDistance;
    private Rigidbody m_Rigidbody;
    private float m_TurnAmount;


    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY |
                                  RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }


    public void Move(Vector3 move, bool crouch, bool jump)
    {
        if (move.magnitude > 1f) move.Normalize();
        
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        CheckGroundStatus();
        ApplyExtraTurnRotation();

        if (m_IsGrounded)
            HandleGroundedMovement(crouch, jump);
        else
            HandleAirborneMovement();

        ScaleCapsuleForCrouching(crouch);
        PreventStandingInLowHeadroom();
        UpdateAnimator(move);
    }


    private void ScaleCapsuleForCrouching(bool crouch)
    {
        if (m_IsGrounded && crouch)
        {
            if (m_Crouching) return;
            m_Capsule.height = m_Capsule.height / 2f;
            m_Capsule.center = m_Capsule.center / 2f;
            m_Crouching = true;
        }
        else
        {
            var crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * KHalf, Vector3.up);
            var crouchRayLength = m_CapsuleHeight - m_Capsule.radius * KHalf;
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * KHalf, crouchRayLength, Physics.AllLayers,
                QueryTriggerInteraction.Ignore))
            {
                m_Crouching = true;
                return;
            }

            m_Capsule.height = m_CapsuleHeight;
            m_Capsule.center = m_CapsuleCenter;
            m_Crouching = false;
        }
    }

    private void PreventStandingInLowHeadroom()
    {
        if (m_Crouching) return;
        
        var crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * KHalf, Vector3.up);
        var crouchRayLength = m_CapsuleHeight - m_Capsule.radius * KHalf;
        if (Physics.SphereCast(crouchRay, m_Capsule.radius * KHalf, crouchRayLength, Physics.AllLayers,
            QueryTriggerInteraction.Ignore))
            m_Crouching = true;
    }


    private void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("Crouch", m_Crouching);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        if (!m_IsGrounded) m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);

        var runCycle = Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        var jumpLeg = (runCycle < KHalf ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded) m_Animator.SetFloat("JumpLeg", jumpLeg);
        
        m_Animator.speed = m_IsGrounded && move.magnitude > 0 ?
            m_Animator.speed = m_AnimSpeedMultiplier :
            m_Animator.speed = 1;
    }


    private void HandleAirborneMovement()
    {
        var extraGravityForce = Physics.gravity * m_GravityMultiplier - Physics.gravity;
        
        m_Rigidbody.AddForce(extraGravityForce);
        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }


    private void HandleGroundedMovement(bool crouch, bool jump)
    {
        if (!jump || crouch || !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded")) return;
        
        m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
        m_IsGrounded = false;
        m_Animator.applyRootMotion = false;
        m_GroundCheckDistance = 0.1f;
    }

    private void ApplyExtraTurnRotation()
    {
        var turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }


    public void OnAnimatorMove()
    {
        if (!m_IsGrounded || !(Time.deltaTime > 0)) return;
        
        var v = m_Animator.deltaPosition * m_MoveSpeedMultiplier / Time.deltaTime;
        v.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = v;
    }


    private void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            m_IsGrounded = true;
            m_GroundNormal = hitInfo.normal;
            m_Animator.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
        }
    }
}