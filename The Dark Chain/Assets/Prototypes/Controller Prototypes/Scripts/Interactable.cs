using JetBrains.Annotations;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [Header("Settings")] [SerializeField] private float m_range = 0.5f;

    [Header("References")] [SerializeField]
    private Transform m_player = null;

    [SerializeField] private TextMeshProUGUI m_display = null;

    [UsedImplicitly]
    private void Update()
    {
        Vector3 toPlayer = m_player.position - transform.position;
        float distance = toPlayer.magnitude;
        float dot = Vector3.Dot(transform.forward, toPlayer);

        bool playerInRange = distance < m_range && dot < 0.0f;
        // Debug.Log("Player in Range, pop up Interact UI");

        m_display.text = playerInRange
            ? "<color=\"green\">Can be seen by Player"
            : "<color=\"red\">Cannot be seen by Player";
    }

    [UsedImplicitly]
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_range);
    }
}