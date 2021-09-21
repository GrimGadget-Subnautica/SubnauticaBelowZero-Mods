using System.IO;
using System.Reflection;
using Grimolfr.SubnauticaZero.JsonConverters;
using Grimolfr.SubnauticaZero.SalvageScanning.Salvage;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;

namespace Grimolfr.SubnauticaZero.SalvageScanning
{
    [QModCore]
    public static class Main
    {
        internal const string ModName = "Scanner ReclaimSalvage";

        internal static Configuration Config { get; private set; }

        static Main()
        {
            Log.LoggingJsonSerializer.Converters.Add(new TechTypeLoggingConverter());
        }

        [QModPatch]
        public static void Initialize()
        {
            Log.Info($"Registering {ModName} configuration...");
            Config = OptionsPanelHandler.Main.RegisterModOptions<Configuration>();

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;

            Log.Info($"Processing patches for {assemblyName}...");
            var harmony = new Harmony($"Grimolfr_{assemblyName}");
            harmony.PatchAll(assembly);
        }
    }
}
