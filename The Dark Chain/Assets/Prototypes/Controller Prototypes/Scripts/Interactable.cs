using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private KeyCode m_interactKey = KeyCode.E;
    [SerializeField] private string m_prompt = "";
    [SerializeField] private float m_range = 3f;
    [SerializeField] private Tooltip m_tooltipPrefab = null;

    /// <summary>
    /// Instantiate offset
    /// </summary>
    [SerializeField] private Vector3 m_offset = Vector3.zero;

    private bool m_interacted;

    private bool m_playerInRange;
    [Header("Callbacks")] public UnityEvent OnInteracted;
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;

    [Header("References")] [SerializeField]
    private Transform m_playerRotation = null;

    private bool hasTooltip;
    private Tooltip m_tooltip;

    [SerializeField] private Transform m_canvas = null;
    [SerializeField] private TextMeshProUGUI m_testText = null;

    [UsedImplicitly]
    private void Awake()
    {
        OnInteracted.AddListener(Interactable_OnInteracted);

        if (m_tooltipPrefab == null) return;

        hasTooltip = true;

        m_tooltip = Instantiate(m_tooltipPrefab, transform.position + m_offset, Quaternion.identity, m_canvas);
        m_tooltip.UpdateInfo(m_interactKey, m_prompt);

        UpdateScreenPosition();
        // m_tooltip.gameObject.SetActive(false);
    }

    [UsedImplicitly]
    private void Update()
    {
        CheckVisibility();
        if (!m_playerInRange) return;
        DisplayTooltip();
        HandleInteract();
    }

    private void CheckVisibility()
    {
        bool playerInRangeBefore = m_playerInRange;

        Vector3 toPlayer = m_playerRotation.position - transform.position;
        float distance = toPlayer.magnitude;
        float dotToPlayer = Vector3.Dot(transform.forward, toPlayer);

        Vector3 toObserver = -toPlayer;
        float dotFromPlayer = Vector3.Dot(m_playerRotation.forward, toObserver.normalized);

        // Player is in range,
        // observer is facing the player,
        // and the player is not facing away from the observer
        // Debug.Log($"{name} - {distance < m_range}, {dotToPlayer > 0.0f}, {dotFromPlayer > 0.0f}");

        m_playerInRange = distance < m_range &&
                          dotToPlayer > 0.0f &&
                          dotFromPlayer > 0.0f;

        bool playerEntered = !playerInRangeBefore && m_playerInRange;
        if (!m_interacted && playerEntered)
        {
            m_testText.text = "Player Entered";
            m_tooltip.Show();
            OnPlayerEnter?.Invoke();
        }

        bool playerExited = playerInRangeBefore && !m_playerInRange;
        if (m_interacted || !playerExited) return;
        m_testText.text = "Player Exited";
        // m_tooltip.gameObject.SetActive(false);
        m_tooltip.Disable();
        OnPlayerExit?.Invoke();
    }

    private void DisplayTooltip()
    {
        if (m_interacted || !hasTooltip) return;
        UpdateScreenPosition();
    }

    private void HandleInteract()
    {
        if (!Input.GetKeyDown(m_interactKey) || m_interacted) return;
        m_testText.text = "Interacted";
        OnInteracted?.Invoke();
        m_interacted = true;
    }

    private void UpdateScreenPosition()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position + m_offset);
        m_tooltip.gameObject.GetComponent<RectTransform>().position = screenPosition;
        m_tooltip.gameObject.SetActive(m_playerInRange);
    }

    private void Interactable_OnInteracted()
    {
        m_interacted = true;
        // m_tooltip.gameObject.SetActive(false);
        m_tooltip.Disable();
    }

    [UsedImplicitly]
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_range);
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 2.0f);
    }
}