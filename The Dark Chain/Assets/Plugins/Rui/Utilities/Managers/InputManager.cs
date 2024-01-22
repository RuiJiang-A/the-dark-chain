using UnityEngine;

namespace Boopoo.Utilities
{
    public static class InputManager
    {
        public static float GetHorizontalInput()
        {
            return Input.GetAxisRaw("Horizontal");
        }
        public static float GetVerticalInput()
        {
            return Input.GetAxisRaw("Vertical");
        }

        public static Vector3 GetDirectionalInputRaw()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            return new Vector3(horizontal, 0, vertical);
        }

        public static Vector3 GetDirectionalInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            return new Vector3(horizontal, 0, vertical);
        }
    }
}