using System;
using UnityEngine;

namespace Boopoo.MotionMatching
{
    [Serializable]
    public struct DesiredInfo
    {
        public bool isStrafing;

        public float gait;
        public float gaitVelocity;

        public Vector3 velocity;
        public Vector3 currentVelocity;
        public Vector3 currentVelocityChange;
        public Vector3 previousVelocityChange;
        public float velocityChangeThreshold;

        public Quaternion rotation;
        public Quaternion currentRotation;
        public Vector3 currentRotationChange;
        public Vector3 previousRotationChange;
        public float rotationChangeThreshold;

        public void Initialize()
        {
            velocityChangeThreshold = rotationChangeThreshold = 50.0f;
        }

        public void UpdateStrafe()
        {
            isStrafing = Input.GetKey(KeyCode.F);
        }

        public void UpdateGait(float deltaTime, float gaitChangeHalflife = 0.1f)
        {
            // Running 0.0f, Walking 1.0f
            float targetPosition = Input.GetKey(KeyCode.LeftShift) ? 0.0f : 1.0f;
            Spring.SolveDamper(ref gait, ref gaitVelocity,
                targetPosition, gaitChangeHalflife, deltaTime
            );
        }

        public void UpdateCurrentVelocity(
            Vector3 movementInput,
            float cameraAzimuth, Quaternion rotation, Speed speed
        )
        {
            // Rotate movement input to match camera azimuth
            Vector3 globalDirection = Quaternion.Euler(0, cameraAzimuth, 0) * movementInput;

            // Convert global direction to local space
            Vector3 localDirection = Quaternion.Inverse(rotation) * globalDirection;

            // Calculate local velocity based on local direction and speed parameters
            Vector3 localVelocity = localDirection.z > 0.0f
                ? new Vector3(speed.side, 0.0f, speed.forward)
                : new Vector3(speed.side, 0.0f, speed.backward);

            // Scale local velocity by local direction
            localVelocity = Vector3.Scale(localVelocity, localDirection);

            // Transform local velocity to world space and return
            currentVelocity = rotation * localVelocity;
        }

        public void UpdateCurrentRotation(Vector3 input, Vector3 direction, float cameraAzimuth)
        {
            (Quaternion rotation, bool isStrafing, Vector3 velocity) info = (rotation, isStrafing, velocity);
            UpdateRotation(ref info, input, direction, cameraAzimuth);
            currentRotation = info.rotation;

            // currentRotation = rotation;
            //
            // if (isStrafing)
            // {
            //     Vector3 directionBase = new Vector3(0, 0, -1);
            //     Quaternion azimuthRotation = Quaternion.Euler(0, cameraAzimuth, 0);
            //     Vector3 desiredDirection = azimuthRotation * directionBase;
            //
            //     if (direction.magnitude > 0.01f)
            //         desiredDirection = azimuthRotation * Vector3.Normalize(direction);
            //
            //     float angle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
            //     currentRotation = Quaternion.Euler(0, angle, 0);
            // }
            // else if (input.magnitude > 0.01f)
            // {
            //     Vector3 desiredDirection = Vector3.Normalize(velocity);
            //     float angle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
            //     currentRotation = Quaternion.Euler(0, angle, 0);
            // }
        }

        public static void UpdateRotation(ref (Quaternion rotation, bool isStrafing, Vector3 velocity) info,
            Vector3 input, Vector3 direction, float cameraAzimuth)
        {
            Quaternion updatedRotation = info.rotation;

            if (info.isStrafing)
            {
                Vector3 directionBase = new(0, 0, -1);
                Quaternion azimuthRotation = Quaternion.Euler(0, cameraAzimuth, 0);
                Vector3 desiredDirection = azimuthRotation * directionBase;

                if (direction.magnitude > 0.01f)
                    desiredDirection = azimuthRotation * Vector3.Normalize(direction);

                float angle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
                updatedRotation = Quaternion.Euler(0, angle, 0);
            }
            else if (input.magnitude > 0.01f)
            {
                Vector3 desiredDirection = Vector3.Normalize(info.velocity);
                float angle = Mathf.Atan2(desiredDirection.x, desiredDirection.z) * Mathf.Rad2Deg;
                updatedRotation = Quaternion.Euler(0, angle, 0);
            }

            info.rotation = updatedRotation;
        }

        public void UpdateVelocityChange(float deltaTime)
        {
            previousVelocityChange = currentVelocityChange;
            currentVelocityChange = (currentVelocity - velocity) / deltaTime;
            velocity = currentVelocity;
        }

        public void UpdateRotationChange(float deltaTime)
        {
            previousRotationChange = currentRotationChange;

            Quaternion difference = Quaternion.Inverse(rotation) * currentRotation;
            difference = difference.normalized;

            difference.ToAngleAxis(out float angle, out Vector3 axis);
            currentRotationChange = (angle / deltaTime) * axis;

            rotation = currentRotation;
        }
    }

    [Serializable]
    public struct SimulatedInfo
    {
        public Vector3 position;
        public Vector3 velocity;
        public Vector3 angularVelocity;
        public Vector3 acceleration;
        public Quaternion rotation;

        public float velocityHalflife;
        public float rotationHalflife;

        public void Initialize()
        {
            velocityHalflife = rotationHalflife = 0.27f;
        }
    }
}