using Boopoo.Telemetry;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class TelemetryStateUpdater : MonoBehaviour
{
    [SerializeField] private TelemetryLogger _logger = null;
    [SerializeField] private TextMeshProUGUI _textRef = null;
    private int _updateCounter;

    [UsedImplicitly]
    private void Update()
    {
        if (_updateCounter <= 0) return;
        UpdateState();
        _updateCounter--;
    }

    public void UpdateState()
    {
        Service service = _logger.Service;
        if (service == null)
        {
            _updateCounter++;
            return;
        }

        string connectionMode = service.NetworkEnabled ? "Online" : "Offline";

        _textRef.text += $"{connectionMode} - Session# {service.SessionInfo.id}\n";

        if (!service.NetworkEnabled)
            _textRef.text += "<color=red>Logs are online only in the current version.</color>\n";
    }
}