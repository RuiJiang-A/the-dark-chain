using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [Header("References")] [SerializeField]
    TextMeshProUGUI m_keyText = null;

    [SerializeField] TextMeshProUGUI m_promptText = null;

    Vector3 m_originalScale;
    Vector3 m_disableScale;
    float m_showDuration = 0.2f;

    [UsedImplicitly]
    private void Awake()
    {
        m_originalScale = transform.localScale;
        m_disableScale = Vector3.zero;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(m_originalScale, m_showDuration);
    }

    public void Disable()
    {
        transform.DOScale(m_disableScale, m_showDuration).OnComplete(
            () => { gameObject.SetActive(false); });
    }

    public void UpdateInfo(KeyCode interactKey, string prompt)
    {
        if (m_keyText) m_keyText.text = interactKey.ToString();

        if (m_promptText) m_promptText.text = prompt;
    }
}