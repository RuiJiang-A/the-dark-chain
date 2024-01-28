using JetBrains.Annotations;
using UnityEngine;

namespace Boopoo.Utilities
{
    public class CameraLook : MonoBehaviour
    {
        [SerializeField] private Transform m_orientation = null;

        private Vector2 m_rotation;
        [SerializeField] private float m_rotationSpeed = 3.0f;

        [SerializeField] private Vector2 m_constrainY = new(-60f, 60f);

        [UsedImplicitly]
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false; 
        }

        [UsedImplicitly]
        private void LateUpdate()
        {
            m_rotation.x += Input.GetAxis("Mouse X");
            m_rotation.y -= Input.GetAxis("Mouse Y");

            m_rotation.y = Mathf.Clamp(m_rotation.y, m_constrainY.x, m_constrainY.y);
            
            Quaternion cameraTargetRotation = Quaternion.Euler(m_rotation.y, m_rotation.x, 0);
            m_orientation.rotation = Quaternion.Slerp(m_orientation.rotation, cameraTargetRotation,
                m_rotationSpeed * Time.deltaTime);
        }
    }
}