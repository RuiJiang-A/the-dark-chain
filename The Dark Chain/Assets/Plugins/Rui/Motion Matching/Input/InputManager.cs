using UnityEngine;

namespace Boopoo.MotionMatching
{
    public static class InputManager
    {
        private const string HORIZONTAL_AXIS = "Horizontal";
        private const string VERTICAL_AXIS = "Vertical";

        public static float GetHorizontalInputRaw()
        {
            return Input.GetAxisRaw(HORIZONTAL_AXIS);
        }

        public static float GetVerticalInputRaw()
        {
            return Input.GetAxisRaw(VERTICAL_AXIS);
        }

        public static Vector3 GetMovementInput()
        {
            float horizontal = Input.GetAxis(HORIZONTAL_AXIS);
            float vertical = Input.GetAxis(VERTICAL_AXIS);
            return new Vector3(horizontal, 0, vertical);
        }

        public static Vector3 CalculateMoveDirection(Transform cameraTransform)
        {
            float horizontalInput = GetHorizontalInputRaw();
            float verticalInput = GetVerticalInputRaw();

            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward = forward.normalized;
            right = right.normalized;

            Vector3 relativeRightInput = horizontalInput * right;
            Vector3 relativeForwardInput = verticalInput * forward;

            return relativeForwardInput + relativeRightInput;
        }

        /// <summary>
        /// Camera ZX Plane Azimuth
        /// 0 - Z Positive, 180 - Z Negative, 90 - X Positive, 270 - X Negative
        /// </summary>
        /// <returns></returns>
        public static float GetMainCameraAzimuth()
        {
            Transform camera = Camera.main.transform;
            Vector3 forward = camera.forward;
            forward.y = 0;

            if (forward == Vector3.zero) return 0;

            float azimuth = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
            if (azimuth < 0) azimuth += 360;

            return azimuth;
        }
    }
}