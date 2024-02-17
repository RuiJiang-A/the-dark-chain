using System.Collections.Generic;
using UnityEngine;

namespace Boopoo.MotionMatching
{
    public struct TrajectoryInfo
    {
        private List<Vector3> desiredVelocities;
        private List<Quaternion> desiredRotations;
        private List<Vector3> positions;
        private List<Vector3> velocities;
        private List<Vector3> accelerations;
        private List<Quaternion> rotations;
        private List<Vector3> angularVelocities;

        public TrajectoryInfo(int capacity)
        {
            desiredVelocities = new List<Vector3>(capacity);
            desiredRotations = new List<Quaternion>(capacity);
            positions = new List<Vector3>(capacity);
            velocities = new List<Vector3>(capacity);
            accelerations = new List<Vector3>(capacity);
            rotations = new List<Quaternion>(capacity);
            angularVelocities = new List<Vector3>(capacity);
        }

        public void PredictPositions()
        {
        }

        public void PredictDesiredVelocities()
        {
        }

        public void PredictDesiredRotations(Quaternion initialDesiredRotation, Vector3 input,
            Vector3 direction, float cameraAzimuth, bool isStrafing, float deltaTime)
        {
            desiredRotations[0] = initialDesiredRotation;
            for (int i = 1; i < desiredRotations.Count; i++)
            {
                (Quaternion rotation, bool isStrafing, Vector3 velocity) info =
                    (rotation: desiredRotations[i - 1], isStrafing, velocity: desiredVelocities[i]);

                float currentCameraAzimuth = PredictCameraAzimuth(input, cameraAzimuth, isStrafing, i * deltaTime);

                DesiredInfo.UpdateRotation(ref info, input, direction, currentCameraAzimuth);
                desiredRotations[i] = info.rotation;
            }
        }

        public void PredictRotations()
        {
        }

        private static float PredictCameraAzimuth(Vector3 input, float cameraAzimuth, bool isStrafing, float deltaTime)
        {
            float axisX = isStrafing ? 0f : -input.x;
            return cameraAzimuth + 2.0f * deltaTime * axisX;
        }

        public void Render()
        {
            Color color = Color.white;

            for (int i = 1; i < positions.Count; i++)
            {
                // DrawSphereWires(positions[i], 0.05f, 4, 10, color);
                // DrawLine3D(positions[i],
                //     positions[i] + 0.6f * (rotations[i] * new Vector3(0, 0, 1f)), color);
                // DrawLine3D(positions[i - 1], positions[i], color);
            }
        }
    }
}