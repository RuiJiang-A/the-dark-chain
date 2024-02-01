using JetBrains.Annotations;
using UnityEngine;

namespace Boopoo.MotionMatching
{
    public class MotionMatchingManager : MonoBehaviour
    {
        private const DeltaTime.Mode DELTA_TIME_MODE = DeltaTime.Mode.DeltaTime;


        [Header("Speeds (m/s)")] [SerializeField]
        private Speed walking = new();

        [SerializeField] private Speed running = new();

        DesiredData desired;
        SimulatedData simulated;

        [Header("References")] [SerializeField]
        Rigidbody m_rigidbody;

        [SerializeField] Transform m_camera;

        [SerializeField] Transform character;
        [SerializeField] GameObject debug;

        [UsedImplicitly]
        private void Awake()
        {
            desired = new DesiredData();
            desired.Initialize();

            simulated = new SimulatedData();
            simulated.Initialize();

#if !UNITY_EDITOR
            if (debug != null) debug.SetActive(false);
#endif
        }

        [UsedImplicitly]
        private void Update()
        {
            float deltaTime = DeltaTime.Get(DELTA_TIME_MODE);
            float cameraAzimuth = InputManager.GetMainCameraAzimuth();

            // Get User Input
            // Vector3 input = InputManager.GetMovementInput();
            Vector3 input = InputManager.GetMovementInput();
            Vector3 direction = InputManager.CalculateMoveDirection(m_camera);

            desired.UpdateGait(deltaTime);

            Speed speed = new();
            CalculateSpeed(ref speed);

            desired.UpdateCurrentVelocity(input, cameraAzimuth, simulated.rotation, speed);
            desired.UpdateCurrentRotation(input, direction, cameraAzimuth);

            // transform.position += desired.velocity * deltaTime;
            // character.rotation = desired.rotation;

            desired.UpdateVelocityChange(deltaTime);
            desired.UpdateRotationChange(deltaTime);

            Visualize();
        }

        private void FixedUpdate()
        {
            m_rigidbody.velocity = desired.velocity;
            // m_rigidbody.rotation = desired.rotation;
            character.rotation = desired.rotation;
        }

        private void CalculateSpeed(ref Speed speed)
        {
            speed.forward = Mathf.Lerp(running.forward, walking.forward, desired.gait);
            speed.side = Mathf.Lerp(running.side, walking.side, desired.gait);
            speed.backward = Mathf.Lerp(running.backward, walking.backward, desired.gait);
        }

        private void Visualize()
        {
#if UNITY_EDITOR
#endif
        }
    }
}