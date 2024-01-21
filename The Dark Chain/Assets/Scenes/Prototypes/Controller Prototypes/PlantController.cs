using JetBrains.Annotations;
using UnityEngine;

namespace Boopoo.Prototype
{
    public class PlantController : MonoBehaviour
    {
        [SerializeField] private float speed = 20.0f;

        [Header("Integration")]
        [SerializeField] private Vector3 position;
        [SerializeField] private Vector3 velocity;
        [SerializeField] private Vector3 acceleration;

        [UsedImplicitly]
        // Start is called before the first frame update
        private void Start()
        {
            acceleration = Vector3.one;
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

            transform.position += position;
            position = Vector3.zero;
        }

        private void IntegrateAcceleration()
        {
            Vector3 force = InputManager.GetDirectionalInput();
            acceleration = force * speed;
            velocity += acceleration * Time.fixedDeltaTime;
        }

        private void IntegrateVelocity()
        {
            const float dampingFactor = 0.05f;
            float frameDamping = Mathf.Pow(dampingFactor, Time.fixedDeltaTime);

            position += velocity * Time.fixedDeltaTime;
            velocity *= frameDamping;

            if (velocity.magnitude < float.Epsilon) velocity = Vector3.zero;
        }
    }
}