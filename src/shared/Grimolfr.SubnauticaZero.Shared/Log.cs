using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using QModManager.Utility;

// ReSharper disable MemberCanBePrivate.Global
namespace Grimolfr.SubnauticaZero
{
    internal static class Log
    {
        private static readonly JsonSerializerSettings LoggingJsonSerializerSettings =
            new JsonSerializerSettings
            {
                MaxDepth = 8,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

        internal static JsonSerializer LoggingJsonSerializer { get; } = CreateLoggingJsonSerializer();

        public static string SerializeForLog(this JArray jArray)
        {
            const int stringBuilderInitialCapacity = 1024 * 16;

            if (jArray == null) return null;

            var sb = new StringBuilder(stringBuilderInitialCapacity);
            using var writer = new StringWriter(sb);

            LoggingJsonSerializer.Serialize(writer, jArray);

            return sb.ToString();

        }

        public static string SerializeForLog(this JObject jObject)
        {
            const int stringBuilderInitialCapacity = 1024 * 16;

            if (jObject == null) return null;

            var sb = new StringBuilder(stringBuilderInitialCapacity);
            using var writer = new StringWriter(sb);

            LoggingJsonSerializer.Serialize(writer, jObject);

            return sb.ToString();
        }

        // Debug
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string message, bool showOnScreen = false)
        {
            if (message != null && Logger.DebugLogsEnabled)
                Logger.Log(Logger.Level.Debug, message, showOnScreen: showOnScreen);
        }

        public static void Debug(JArray jArray, bool showOnScreen = false)
        {
            if (jArray != null && Logger.DebugLogsEnabled)
                Debug(jArray.SerializeForLog(), showOnScreen);
        }

        public static void Debug(JObject jObject, bool showOnScreen = false)
        {
            if (jObject != null && Logger.DebugLogsEnabled)
                Debug(jObject.SerializeForLog(), showOnScreen);
        }

        public static void Debug(IEnumerable array, bool showOnScreen = false)
        {
            if (array != null && Logger.DebugLogsEnabled)
                Debug(JArray.FromObject(array), showOnScreen);
        }

        public static void Debug(object @object, bool showOnScreen = false)
        {
            if (@object != null && Logger.DebugLogsEnabled)
                Debug(JObject.FromObject(@object), showOnScreen);
        }

        // Information
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Info(string message, bool showOnScreen = false)
        {
            if (message != null)
                Logger.Log(Logger.Level.Info, message, showOnScreen: showOnScreen);
        }

        public static void Info(JArray jArray, bool showOnScreen = false)
        {
            if (jArray != null)
                Info(jArray.SerializeForLog(), showOnScreen);
        }

        public static void Info(JObject jObject, bool showOnScreen = false)
        {
            if (jObject != null)
                Info(jObject.SerializeForLog(), showOnScreen);
        }

        public static void Info(IEnumerable array, bool showOnScreen = false)
        {
            if (array != null)
                Info(JArray.FromObject(array), showOnScreen);
        }

        public static void Info(object @object, bool showOnScreen = false)
        {
            if (@object != null)
                Info(JObject.FromObject(@object), showOnScreen);
        }

        // Warning
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warn(string message, bool showOnScreen = false)
        {
            if (message != null)
                Logger.Log(Logger.Level.Warn, message, showOnScreen: showOnScreen);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Warn(Exception ex)
        {
            if (ex != null)
                Logger.Log(Logger.Level.Warn, ex: ex);
        }

        // Error
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Error(Exception ex)
        {
            if (ex != null)
                Logger.Log(Logger.Level.Error, ex: ex);
        }

        // Fatal
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Fatal(Exception ex)
        {
            if (ex != null)
                Logger.Log(Logger.Level.Fatal, ex: ex);
        }

        private static JsonSerializer CreateLoggingJsonSerializer()
        {
            var serializer = JsonSerializer.CreateDefault(LoggingJsonSerializerSettings);

            serializer.Converters.Add(new StringEnumConverter());

            return serializer;
        }
    }
}
