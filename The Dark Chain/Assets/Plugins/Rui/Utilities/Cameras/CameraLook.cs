using JetBrains.Annotations;
using UnityEngine;

namespace Boopoo.Utilities
{
    public class CameraLook : MonoBehaviour
    {
        [SerializeField] private Transform m_orientation = null;
        private Vector2 m_rotation;

        [UsedImplicitly]
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        [UsedImplicitly]
        private void LateUpdate()
        {
            m_rotation.x += Input.GetAxis("Mouse X");
            m_rotation.y -= Input.GetAxis("Mouse Y");

            // m_rotation.x = Mathf.Clamp(m_rotation.x, -75f, 75f);
            m_rotation.y = Mathf.Clamp(m_rotation.y, -60f, 60f);
            m_orientation.transform.rotation = Quaternion.Euler(m_rotation.y, m_rotation.x, 0);
        }
    }
}