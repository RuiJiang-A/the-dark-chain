using UnityEngine;

public static class InputManager
{
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