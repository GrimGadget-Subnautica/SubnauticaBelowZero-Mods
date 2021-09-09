using System.IO;
using System.Reflection;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;

namespace Grimolfr.SubnauticaZero.ScannerSalvage
{
    [QModCore]
    public static class Main
    {
        internal const string ModName = "Scanner Salvage";

        internal static Configuration Config { get; private set; }

        [QModPatch]
        public static void Initialize()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name);

            Log.Info($"Registering {ModName} configuration...");
            Config = OptionsPanelHandler.Main.RegisterModOptions<Configuration>();
            if (!File.Exists(Config.JsonFilePath))
                Config.Save();

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;

            Log.Info($"Processing patches for {assemblyName}...");
            var harmony = new Harmony($"Grimolfr_{assemblyName}");
            harmony.PatchAll(assembly);
        }
    }
}
