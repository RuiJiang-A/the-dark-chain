using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

public class Examinable : MonoBehaviour
{
    [SerializeField] private Canvas m_examinationCanvas = null;

    // [SerializeField] private Camera m_camera = null;
    [SerializeField] private int m_examinationLayer = 7;

    // [SerializeField] private Image m_backgroundImage = null;

    // [SerializeField] private TextMeshProUGUI m_descriptionText = null;
    // [SerializeField] private string m_description = string.Empty;
    [SerializeField] private Vector3 m_scale = Vector3.one;
    // [SerializeField] private Vector3 m_offset = new(0, 0, -5);

    [SerializeField] private KeyCode m_exitKey = KeyCode.Escape;
    private bool m_isExamining;

    private Vector3 m_previousMousePosition;

    private Vector3 m_originalPosition;
    private Vector3 m_originalScale;
    private Quaternion m_originalRotation;
    private int m_originalLayer;

    private float m_animationDuration = 0.2f;


    [UsedImplicitly]
    private void Update()
    {
        if (!m_isExamining) return;
        if (Input.GetKeyDown(m_exitKey)) StopExamination();
        // Examine();
    }

    [UsedImplicitly]
    public void StartExamination()
    {
        m_isExamining = true;

        m_originalPosition = transform.position;
        m_originalScale = transform.localScale;
        m_originalRotation = transform.rotation;
        m_originalLayer = gameObject.layer;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ChangeToExaminationLayer();

        m_examinationCanvas.gameObject.SetActive(m_isExamining);
        // m_backgroundImage.gameObject.SetActive(m_isExamining);

        // m_descriptionText.text = m_description;

        transform.DOScale(m_scale, m_animationDuration);

        // Position the camera at an offset relative to the object's center
        // Vector3 cameraPosition = transform.position + m_offset;
        // m_camera.transform.position = cameraPosition;

        // Vector3 center = GetComponentInChildren<Collider>().bounds.center;
        // Make the camera look at the object's center
        // m_camera.transform.LookAt(center);

        Time.timeScale = 0;
    }

    private void Examine()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 deltaMousePosition = currentMousePosition - m_previousMousePosition;
        float rotationSpeed = 0.5f;

        Vector3 center = GetComponentInChildren<Collider>().bounds.center;

        // Rotate around the object's center on the Y-axis (up)
        transform.RotateAround(center, Vector3.up, deltaMousePosition.x * rotationSpeed);

        // Rotate around the object's center on the X-axis (left)
        transform.RotateAround(center, Vector3.left, deltaMousePosition.y * rotationSpeed);

        m_previousMousePosition = currentMousePosition;
    }

    public void StopExamination()
    {
        m_isExamining = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ChangeToOriginalLayer();

        // m_descriptionText.text = string.Empty;
        // m_backgroundImage.gameObject.SetActive(m_isExamining);

        Time.timeScale = 1;

        transform.DOMove(m_originalPosition, m_animationDuration);
        transform.DOScale(m_originalScale, m_animationDuration);
        transform.DORotateQuaternion(m_originalRotation, m_animationDuration).OnComplete(
            () => { m_examinationCanvas.gameObject.SetActive(m_isExamining); }
        );
    }

    private void ChangeToOriginalLayer()
    {
        gameObject.layer = m_originalLayer;
        // Iterate through all child GameObjects and change their layers
        ChangeLayerOfChildren(gameObject, m_originalLayer);
    }

    private void ChangeToExaminationLayer()
    {
        gameObject.layer = m_examinationLayer;
        // Iterate through all child GameObjects and change their layers
        ChangeLayerOfChildren(gameObject, m_examinationLayer);
    }


    private void ChangeLayerOfChildren(GameObject obj, int layer)
    {
        foreach (Transform child in obj.transform)
        {
            // Change the layer of the child
            child.gameObject.layer = layer;

            // Recursively change the layers of any children of this child
            if (child.childCount > 0)
            {
                ChangeLayerOfChildren(child.gameObject, layer);
            }
        }
    }
}