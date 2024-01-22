using Boopoo.Utilities;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float m_accelerationForce = 1200f;
    [SerializeField] private float m_maxVelocity = 10.0f;
    private Vector3 m_moveDirection = Vector3.zero;

    [Header("References")] [SerializeField]
    private Animator m_animator = null;

    [SerializeField] private Rigidbody m_rigidbody = null;
    [SerializeField] private Transform m_rotation = null;

    [UsedImplicitly]
    private void LateUpdate()
    {
        MovePlayerRelativeToCamera();
        // RotateCharacter();
    }

    [UsedImplicitly]
    private void FixedUpdate()
    {
        m_rigidbody.AddForce(m_moveDirection * m_accelerationForce);

        float currentVelocity = m_rigidbody.velocity.magnitude;
        
        if (currentVelocity > m_maxVelocity)
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_maxVelocity;

        float percentage = currentVelocity / m_maxVelocity;
        m_animator.SetFloat("velocity", percentage);
    }

    private void MovePlayerRelativeToCamera()
    {
        float horizontalInput = InputManager.GetHorizontalInput();
        float verticalInput = InputManager.GetVerticalInput();

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward = forward.normalized;
        right = right.normalized;

        Vector3 relativeHorizontalInput = horizontalInput * right;
        Vector3 relativeVerticalInput = verticalInput * forward;

        // Update Character Move Direction
        m_moveDirection = relativeHorizontalInput * 0.2f + relativeVerticalInput;
        // m_moveDirection = relativeVerticalInput;
        // Update Character Mesh Rotation
        m_rotation.transform.forward = forward;
    }
}