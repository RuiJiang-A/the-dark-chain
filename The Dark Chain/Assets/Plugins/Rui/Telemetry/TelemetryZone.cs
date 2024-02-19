using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Boopoo.Telemetry
{
    public class TelemetryZone : MonoBehaviour
    {
        [SerializeField] string _location = string.Empty;

        [UsedImplicitly]
        private void OnTriggerEnter(Collider other)
        {
            TelemetryLogger.ChangeLocation(this, _location);
        }

#if UNITY_EDITOR
        [UsedImplicitly]
        private void OnValidate()
        {
            gameObject.name = $"Location - {_location}";
        }
#endif
    }
}