using Boopoo.Utilities;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// acceleration force
    /// </summary>
    [Header("Settings")] [SerializeField] private float m_accelerationForce = 1200f;

    /// <summary>
    /// Velocity constraint
    /// </summary>
    [SerializeField] private float m_maxVelocity = 10.0f;

    /// <summary>
    /// desire move direction
    /// </summary>
    private Vector3 m_moveDirection = Vector3.zero;

    /// <summary>
    /// Allow automatic rotate character when moving
    /// </summary>
    [SerializeField] private bool m_allowPlayerRotation = true;

    /// <summary>
    /// character rotation speed
    /// </summary>
    [SerializeField] private float m_rotationSpeed = 5.0f;

    /// <summary>
    /// Animator controller for player
    /// </summary>
    [Header("References")] [SerializeField]
    private Animator m_animator = null;

    /// <summary>
    /// Player's rigidbody
    /// </summary>
    [SerializeField] private Rigidbody m_rigidbody = null;

    /// <summary>
    /// Transform controls player mesh rotation
    /// </summary>
    [SerializeField] private Transform m_rotation = null;

    [UsedImplicitly]
    private void LateUpdate()
    {
        Vector3 currentInput = InputManager.GetDirectionalInput();
        m_animator.SetFloat("inputX", currentInput.x);
        m_animator.SetFloat("inputZ", currentInput.z);

        MovePlayerRelativeToCamera();
    }

    [UsedImplicitly]
    private void FixedUpdate()
    {
        m_rigidbody.AddForce(m_moveDirection * m_accelerationForce);

        Vector3 currentVelocity = m_rigidbody.velocity;

        if (currentVelocity.sqrMagnitude > m_maxVelocity * m_maxVelocity)
            m_rigidbody.velocity = currentVelocity.normalized * m_maxVelocity;
    }

    private void MovePlayerRelativeToCamera()
    {
        float verticalInput = InputManager.GetVerticalInputRaw();
        m_moveDirection = InputManager.CalculateMoveDirection(Camera.main.transform);

        // Update Character Mesh Rotation
        if (m_moveDirection == Vector3.zero || !m_allowPlayerRotation) return;

        Quaternion targetRotation = verticalInput < 0
            ? Quaternion.LookRotation(-m_moveDirection)
            : Quaternion.LookRotation(m_moveDirection);

        m_rotation.rotation = Quaternion.Slerp(
            m_rotation.rotation,
            targetRotation,
            m_rotationSpeed * Time.deltaTime
        );
    }
}