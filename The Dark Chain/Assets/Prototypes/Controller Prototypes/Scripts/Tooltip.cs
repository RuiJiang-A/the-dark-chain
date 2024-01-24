using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [Header("References")] [SerializeField]
    TextMeshProUGUI m_keyText = null;

    [SerializeField] TextMeshProUGUI m_promptText = null;

    public void UpdateInfo(KeyCode interactKey)
    {
        if (m_keyText) m_keyText.text = interactKey.ToString();
    }

    public void UpdateInfo(string prompt)
    {
        if (m_promptText) m_promptText.text = prompt;
    }

    public void UpdateInfo(KeyCode interactKey, string prompt)
    {
        if (m_keyText) m_keyText.text = interactKey.ToString();

        if (m_promptText) m_promptText.text = prompt;
    }
}