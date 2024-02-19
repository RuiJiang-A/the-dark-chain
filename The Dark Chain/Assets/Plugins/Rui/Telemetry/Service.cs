using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using static Boopoo.Telemetry.Settings;

namespace Boopoo.Telemetry
{
    public struct SessionInfo
    {
        public int id;
        public string key;

        public SessionInfo(int id, string key)
        {
            this.id = id;
            this.key = key;
        }
    }

    public class Service : MonoBehaviour
    {
        public enum ServiceStatus
        {
            NotInitialized, // Slightly more explicit than Uninitialized, indicating the service hasn't been initialized yet.
            Initializing, // This could be a clearer term than Waking, indicating the service is in the process of starting up.
            Authenticating, // Unchanged, clearly indicates the service is in the process of authenticating.
            Operational, // A bit broader than Ready, indicating the service is fully operational and ready to use.
            Disconnected // Unchanged, clearly indicates the service is not currently connected.
        }

        private static Service _currentService = null;
        private static Dictionary<Settings, Service> _settingsToServiceMap = new();

        private Settings _settings;
        private bool _networkEnabled;

        public bool NetworkEnabled => _networkEnabled;

        private ServiceStatus _state = ServiceStatus.NotInitialized;
        private SessionInfo _sessionInfo;
        private string _sceneName;
        private float _lastMessageTime;

        public SessionInfo SessionInfo => _sessionInfo;

        string _lastLocation;

        readonly Queue<string> _requestQueue = new();
        private int _requestBuffered = 0;

        const int MAX_LOGS_PER_SECOND = 10;
        const float MIN_SECONDS_BETWEEN_LOGS = 1f / MAX_LOGS_PER_SECOND;
        const int RATE_WARNING_THRESHOLD = 2 * MAX_LOGS_PER_SECOND;

        [SerializeField] public event System.Action<int> OnConnectionEstablished;
        [SerializeField] public event System.Action<string> OnDisconnected;

        [UsedImplicitly]
        private void OnDestroy()
        {
            Disconnect();

            OnConnectionEstablished = null;
            OnDisconnected = null;
        }

        private void Initialize(Settings settings)
        {
            _settings = settings;
            LogHelper.LoggingLevel = _settings.loggingLevel;

            _networkEnabled = _settings.transmissionMode switch
            {
                TransmissionMode.Always => true,
                TransmissionMode.OnDemand => Application.isEditor == false,
                TransmissionMode.LocalOnly => false,
                _ => false
            };

            string message = $"Loaded settings from '{settings.name}': " +
                             $"{(_networkEnabled ? "<color=green>Logging to server enabled</color>" : "<color=yellow>Logging to local debug console only</color>")}.";

            LogHelper.LogConnection(message);
        }

        public static Service Connect(TelemetryLogger logger, System.Action<int> onConnectionSuccess,
            System.Action<string> onConnectionFailure)
        {
            if (logger.Settings == null)
            {
                Debug.LogError($"Telemetry logger {logger.name} has no Telemetry Settings assigned.");
                return null;
            }

            if (_currentService != null) _currentService.Disconnect();

            if (!_settingsToServiceMap.TryGetValue(logger.Settings, out Service service))
            {
                service = new GameObject("Telemetry Service").AddComponent<Service>();
                DontDestroyOnLoad(service.gameObject);
                service.Initialize(logger.Settings);
                _settingsToServiceMap.Add(logger.Settings, service);
                _currentService = service;

                // Listerner must be added before connecting function
                service._sceneName = logger.gameObject.scene.name;
                service.AddListeners(onConnectionSuccess, onConnectionFailure);
                service.ConnectTo(logger);
            }
            else
            {
                _currentService = service;
                // Listerner must be added before RequestServiceFor
                service.AddListeners(onConnectionSuccess, onConnectionFailure);
                service.RequestServiceFor(logger);
            }

            return service;
        }


        private void ConnectTo(TelemetryLogger logger)
        {
            _lastLocation = logger.Location;
            InitializeSession();
        }

        public void RequestServiceFor(TelemetryLogger logger)
        {
            if (logger.Location != _lastLocation)
            {
                var telemetry = new Event<string>("Change Section", logger.Location);
                telemetry.location = _lastLocation;
                telemetry.sessionId = _sessionInfo.id;
                telemetry.sessionKey = _sessionInfo.key;
                TryLog(logger, telemetry);
            }

            _lastLocation = logger.Location;

            // TODO: Implement logic
        }

        private void InitializeSession()
        {
            if (!_networkEnabled) InitializeOfflineSession();
            else StartCoroutine(Start_InitializeOnlineSession());
        }

        private void InitializeOfflineSession()
        {
            int id = -65535;
            string key = string.Empty;
            _sessionInfo = new SessionInfo(id, key);

            _networkEnabled = false;
            _state = ServiceStatus.Operational;

            _currentService.OnConnectionEstablished?.Invoke(_sessionInfo.id);
        }

        private IEnumerator Start_InitializeOnlineSession()
        {
            _state = ServiceStatus.Initializing;
            // TODO: implement initializing logic

            _state = ServiceStatus.Authenticating;
            // TODO: implement authenticating logic

            StartCoroutine(Start_RequestSessionInfo());
            yield return null;
        }

        private IEnumerator Start_RequestSessionInfo()
        {
            Settings settings = _currentService._settings;
            string url = $"{settings.ServerURL}/api/sessions/create-session";
            Debug.Log(settings.Version);
            UnityWebRequest request = NetworkHelper.Post(url, $"{{\"build_version\":\"{settings.Version}\"}}");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                bool validResponse = JSONHelper.TryParseJson(json, out SessionInfo sessionInfo);
                if (validResponse)
                {
                    _sessionInfo = sessionInfo;
                    _state = ServiceStatus.Operational;
                    _currentService.OnConnectionEstablished?.Invoke(_sessionInfo.id);
                }
                else
                {
                    _state = ServiceStatus.Disconnected;
                    _currentService.OnDisconnected?.Invoke(
                        $"Invalid JSON response received: {request.downloadHandler.text}");
                    InitializeOfflineSession();
                }
            }
            else
            {
                string error = NetworkHelper.ExtractErrorMessage(request);
                LogHelper.LogError($"Failed request session info: {error}");
                _state = ServiceStatus.Disconnected;
                _currentService.OnDisconnected?.Invoke(error);
                InitializeOfflineSession();
            }
        }

        // call when intent to disconnect instead of crashed
        private void Disconnect()
        {
            string message = $"Service in \"{_sceneName}\" safely disconnected.";
            LogHelper.LogConnection(message);
            OnDisconnected?.Invoke(message);
        }


        public void TryLog<T>(TelemetryLogger logger, Event<T> telemetry)
        {
            string data = JsonUtility.ToJson(telemetry);

            if (telemetry.data == null) data = JSONHelper.PurgeData(data);

#if UNITY_EDITOR
            else if (JSONHelper.IsInvalidData(telemetry, data))
            {
                LogHelper.LogError(
                    $"Trying to log event '{telemetry.name}' with non-serializable data payload type {typeof(T)}. Be sure to use the [System.Serializable] attribute on any custom class/struct you want to use as telemetry data.");
            }
#endif
            _requestQueue.Enqueue(data);
            LogAllToServer();
        }

        private void LogAllToServer()
        {
            if (_networkEnabled && _state != ServiceStatus.Operational)
            {
                LogHelper.LogError("Have not connect to server yet, nothing will be logged");
                return;
            }

            Settings settings = _currentService._settings;
            string url = $"{settings.ServerURL}/api/events/create-event";

            while (_requestQueue.Count > 0) StartCoroutine(LogToServer(url));
        }

        private IEnumerator LogToServer(string url)
        {
            string data = _requestQueue.Dequeue();
            LogHelper.LogVerbose($"Logging event: {data}");

            _lastMessageTime = Time.realtimeSinceStartup;

            UnityWebRequest request = NetworkHelper.Post(url, data);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                LogHelper.LogError($"Failed to log event: {NetworkHelper.ExtractErrorMessage(request)}\n{data}");
            else LogHelper.LogVerbose($"Successfully logged event: {data}");

            float secondsSinceLastMessage = Time.realtimeSinceStartup - _lastMessageTime;
            yield return new WaitForSecondsRealtime(MIN_SECONDS_BETWEEN_LOGS - secondsSinceLastMessage);
        }

        // warning not an universal function to add listerner
        // a utility method to assign listerners from TelemetryLogger and self listener
        private void AddListeners(Action<int> onConnectionSuccess, Action<string> onConnectionFailure)
        {
            OnConnectionEstablished += Service_OnConnectionEstablished;
            OnConnectionEstablished += onConnectionSuccess;
            OnDisconnected += onConnectionFailure;
        }

        private static void Service_OnConnectionEstablished(int sessionId)
        {
            LogHelper.LogConnection($"Connect to session {sessionId}.");
        }
    }
}