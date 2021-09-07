using System;
using System.Runtime.CompilerServices;
using QModManager.Utility;

namespace Grimolfr.SubnauticaZero
{
    internal static class Log
    {
        // Debug
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Debug(string message, bool showOnScreen = false)
        {
            if (message != null && Logger.DebugLogsEnabled)
                Logger.Log(Logger.Level.Debug, message, showOnScreen: showOnScreen);
        }

        // Information
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Info(string message, bool showOnScreen = false)
        {
            if (message != null)
                Logger.Log(Logger.Level.Info, message, showOnScreen: showOnScreen);
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

    }
}
