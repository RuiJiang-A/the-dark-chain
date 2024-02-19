using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Boopoo.Telemetry
{
    public static class JSONHelper
    {
        private static readonly Regex DATA_PURGE = new("\"data\":.*}\\Z", RegexOptions.Compiled);

        public static bool TryParseJson<T>(string json, out T result)
        {
            try
            {
                result = JsonUtility.FromJson<T>(json);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error parsing JSON: {e.Message}\n{json}");
                result = default;
                return false;
            }
        }

        public static string PurgeData(string json)
        {
            return DATA_PURGE.Replace(json, "\"data\": null}");
        }


        public static bool IsInvalidData<T>(Event<T> source, string json)
        {
            return json.IndexOf("\"data\":", StringComparison.Ordinal) < 0;
        }
    }
}