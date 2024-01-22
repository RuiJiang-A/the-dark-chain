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
        float dotToPlayer = Vector3.Dot(transform.forward, toPlayer);

        Vector3 toObserver = -toPlayer;
        float dotFromPlayer = Vector3.Dot(m_player.forward, toObserver.normalized);
        
        // Player is in range,
        // observer is facing the player,
        // and the player is not facing away from the observer
        bool playerInRange = distance < m_range &&
                             dotToPlayer < 0.0f &&
                             dotFromPlayer > 0.0f;

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