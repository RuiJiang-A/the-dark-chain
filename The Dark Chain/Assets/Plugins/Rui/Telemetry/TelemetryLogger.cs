using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static Boopoo.Telemetry.Settings;

namespace Boopoo.Telemetry
{
    public class TelemetryLogger : MonoBehaviour
    {
        [Tooltip("Telemetry sent from this scene will use this label.")] [SerializeField]
        private string _location;

        [SerializeField] private Settings _settings;

        public Settings Settings
        {
            get => _settings;
            private set => _settings = value;
        }

        public string Location
        {
            get => _location;
            private set => _location = value;
        }

        Service _service;

        public Service Service => _service;

        [Space(8)]
        [Tooltip(
            "When successfully connected to the telemetry server, this will be called with the session ID number. Use this if you want to display the session number on-screen.")]
        public UnityEvent<int> OnConnectionEstablished;

        [Space(8)]
        [Tooltip(
            "When connection to the telemetry service fails, this will be called with the failure message. Use this if you want to print the error on-screen.")]
        public UnityEvent<string> OnDisconnected;

        static Dictionary<Scene, TelemetryLogger> _instances = new();

        #region Unity Callbacks

        [UsedImplicitly]
        private void Awake()
        {
            if (_service == null) Initialize();
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            if (_instances.TryGetValue(gameObject.scene, out TelemetryLogger value) && value == this)
                _instances.Remove(gameObject.scene);

            if (_service == null) return;
            _service.OnConnectionEstablished -= Service_OnConnectionEstablished;
            _service.OnDisconnected -= Service_OnDisconnected;
        }

#if UNITY_EDITOR
        [UsedImplicitly]
        private void OnValidate()
        {
            if (Settings == null) Settings = GetDefault();
        }
#endif

        #endregion

        private void Initialize()
        {
            // Ensure there's at most one cached logger in the scene.
            if (_instances.ContainsKey(gameObject.scene))
            {
                if (Settings.loggingLevel <= LoggingLevel.Warnings)
                    Debug.LogWarning(
                        $"Two different {nameof(TelemetryLogger)}s in one scene. This might lead to accidentally logging through the wrong one.");
            }
            else
            {
                _instances.Add(gameObject.scene, this);
            }

            // Fetch a telemetry service that matches our TelemetrySettings, spinning up a new one if needed.
            _service = Service.Connect(this,
                Service_OnConnectionEstablished,
                Service_OnDisconnected);
        }

        public void Log<T>(string eventType, T data)
        {
            var telemetry = new Event<T>(eventType, data);
            telemetry.sessionId = _service.SessionInfo.id;
            telemetry.sessionKey = _service.SessionInfo.key;
            telemetry.location = Location;
            _service.TryLog(this, telemetry);
        }

        public static void Log<T>(Scene source, string eventType, T data)
        {
            // Ask the selected logger to log this event.
            if (TryGetInstance(source, out var logger))
                logger.Log(eventType, data);
        }

        public static void Log<T>(Component sender, string eventType, T data)
        {
            Scene source = sender != null ? sender.gameObject.scene : default;
            Log(source, eventType, data);
        }

        public static bool TryGetInstance(Scene source, out TelemetryLogger logger)
        {
            // If no scene was provided, use the current active scene.
            if (source == default) source = SceneManager.GetActiveScene();
            // Try to find the logger for that scene.
            if (_instances.TryGetValue(source, out logger)) return true;

            // If there's none in our cache, check the target scene.
            Scene active = SceneManager.GetActiveScene();
            if (source != active) SceneManager.SetActiveScene(source);
            logger = FindObjectOfType<TelemetryLogger>();
            if (source != active) SceneManager.SetActiveScene(active);

            // If that fails, systematically search for *any* active logger we can use.
            if (logger == null)
            {
                foreach (TelemetryLogger value in _instances.Values)
                {
                    if (value == null) continue;
                    logger = value;
                    break;
                }
            }

            // If all our attempts have failed, report an error and abort.
            if (logger == null)
            {
                Debug.LogError(
                    $"Trying to log telemetry in a scene '{source.name}' when no TelemetryLogger is available. Be sure to place a TelemetryLogger in your scene.");
                return false;
            }

            if (logger.gameObject.scene != source)
            {
                // If we found a logger, but it's mismatched, warn about it.
                LogHelper.LogWarning(
                    $"Trying to log telemetry in a scene '{source.name}' with no loggers. Redirecting to active logger from '{logger.gameObject.scene.name}' instead. This might lead to misleading 'section' values reported in your telemetry.");
            }

            return true;
        }

        [System.Serializable]
        public struct DefaultData
        {
        }

        static readonly DefaultData DEFAULT_DATA = default;

        public static void Log(Component sender, string eventType)
        {
            Log(sender, eventType, DEFAULT_DATA);
        }

        public void Log(string eventType)
        {
            Log(eventType, DEFAULT_DATA);
        }

        public void ChangeLocation(string location)
        {
            if (!_service.NetworkEnabled) return;

            _location = location;
            _service.RequestServiceFor(this);
        }

        public static void ChangeLocation(Component sender, string location)
        {
            if (!TryGetInstance(sender.gameObject.scene, out TelemetryLogger logger)) return;
            logger.ChangeLocation(location);
        }

        #region Event Handlers

        private void Service_OnConnectionEstablished(int sessionId)
        {
            OnConnectionEstablished?.Invoke(sessionId);
        }

        private void Service_OnDisconnected(string message)
        {
            OnDisconnected?.Invoke(message);
        }

        #endregion
    }
}