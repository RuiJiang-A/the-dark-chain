using System;

namespace Boopoo.Telemetry
{
    [Serializable]
    public class Event<T>
    {
        public T data;
        public int sessionId = -1;
        public string sessionKey = null;
        public string location = null;
        public string name;

        public TimeLogger timeLogger = new();

        public Event(string name, T data = default)
        {
            this.name = name;
            this.data = data;
            timeLogger.Time = DateTimeOffset.UtcNow;
        }
    }

    public class TimeLogger
    {
        private long _timeCode;

        public DateTimeOffset Time
        {
            get => DateTimeOffset.FromUnixTimeMilliseconds(_timeCode);
            set => _timeCode = value.ToUnixTimeMilliseconds();
        }
    }
}