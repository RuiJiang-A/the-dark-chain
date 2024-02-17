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

        private DesiredInfo desired;
        private SimulatedInfo simulated;

        private TrajectoryInfo trajectory;

        // Search
        private float searchTime = 0.1f;
        private float searchTimer = 0.1f;
        private float forceSearchTimer = 0.1f;

        [Header("References")] [SerializeField]
        private Rigidbody m_rigidbody = null;

        [SerializeField] private Transform m_camera = null;
        [SerializeField] private Transform character = null;

        [UsedImplicitly]
        private void Awake()
        {
            desired = new DesiredInfo();
            desired.Initialize();

            simulated = new SimulatedInfo();
            simulated.Initialize();

            trajectory = new TrajectoryInfo(4);
        }

        [UsedImplicitly]
        private void Update()
        {
            // Controller 

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

            character.rotation = desired.currentRotation;

            desired.UpdateVelocityChange(deltaTime);
            desired.UpdateRotationChange(deltaTime);

            // Search
            bool forceSearch = ShouldForceSearch(deltaTime, desired);

            // Prediction
            PredictTrajectory();

            bool endOfAnimation = false;

            bool needSearch = forceSearch || searchTimer <= 0.0f || endOfAnimation;
            if (needSearch) Search();

            searchTimer -= deltaTime;
        }

        [UsedImplicitly]
        private void FixedUpdate()
        {
            m_rigidbody.velocity = desired.velocity;
        }

        private void CalculateSpeed(ref Speed speed)
        {
            speed.forward = Mathf.Lerp(running.forward, walking.forward, desired.gait);
            speed.side = Mathf.Lerp(running.side, walking.side, desired.gait);
            speed.backward = Mathf.Lerp(running.backward, walking.backward, desired.gait);
        }

        private bool ShouldForceSearch(float deltaTime, DesiredInfo info)
        {
            float velocityChangeThreshold = info.velocityChangeThreshold;
            float rotationChangeThreshold = info.rotationChangeThreshold;

            if (forceSearchTimer <= 0.0f)
            {
                bool isVelocityChangeThresholdMet = info.previousVelocityChange.magnitude >= velocityChangeThreshold
                                                    && info.currentVelocityChange.magnitude < velocityChangeThreshold;

                bool isRotationChangeThresholdMet = info.previousRotationChange.magnitude >= rotationChangeThreshold
                                                    && info.currentRotationChange.magnitude < rotationChangeThreshold;

                if (isVelocityChangeThresholdMet || isRotationChangeThresholdMet) return true;
            }
            else if (forceSearchTimer > 0) forceSearchTimer -= deltaTime;

            return false;
        }


        private void PredictTrajectory()
        {
            trajectory.PredictRotations();
        }

        private void Search()
        {
            OnSearch();
            searchTimer = searchTime;
        }

        protected virtual void OnSearch()
        {
        }
    }
}