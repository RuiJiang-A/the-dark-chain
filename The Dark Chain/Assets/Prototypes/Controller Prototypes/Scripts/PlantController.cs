using Boopoo.Utilities;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private float m_speed = 1.5f;

    [Header("Integration")] [SerializeField]
    private Vector3 m_position;

    [SerializeField] private Vector3 m_velocity;
    [SerializeField] private Vector3 m_acceleration;

    [Header("References")] [SerializeField]
    private Animator m_animator = null;

    [UsedImplicitly]
    // Start is called before the first frame update
    private void Start()
    {
        m_acceleration = Vector3.one;
    }

    [UsedImplicitly]
    // Update is called once per frame
    private void Update()
    {
    }

    [UsedImplicitly]
    private void FixedUpdate()
    {
        IntegrateAcceleration();
        IntegrateVelocity();

        transform.Translate(m_position);
        m_position = Vector3.zero;
    }

    private void IntegrateAcceleration()
    {
        Vector3 force = InputManager.GetDirectionalInput();
        m_acceleration = force * m_speed;
        m_animator.SetBool("isRunning", m_acceleration != Vector3.zero);
        m_velocity += m_acceleration * Time.fixedDeltaTime;
    }

    private void IntegrateVelocity()
    {
        const float dampingFactor = 0.05f;
        float frameDamping = Mathf.Pow(dampingFactor, Time.fixedDeltaTime);

        m_position += m_velocity * Time.fixedDeltaTime;
        m_velocity *= frameDamping;

        if (m_velocity.magnitude < float.Epsilon) m_velocity = Vector3.zero;
    }
}