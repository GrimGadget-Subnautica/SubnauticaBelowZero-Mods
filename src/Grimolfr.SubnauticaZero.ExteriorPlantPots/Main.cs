using System.IO;
using System.Reflection;
using Grimolfr.SubnauticaZero.ExteriorPlantPots.Prefabs;
using HarmonyLib;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;

namespace Grimolfr.SubnauticaZero.ExteriorPlantPots
{
    [QModCore]
    public static class Main
    {
        internal static Configuration Config { get; private set; }

        [QModPatch]
        public static void Initialize()
        {
            Log.Info("Registering Exterior Plant Pots configuration...");
            Config = OptionsPanelHandler.Main.RegisterModOptions<Configuration>();
            if (!File.Exists(Config.JsonFilePath))
                Config.Save();

            Log.Info("Registering Exterior Plant Pot prefabs...");
            new ExteriorPlantPotPrefab().Patch();
            new ExteriorPlantPot2Prefab().Patch();
            new ExteriorPlantPot3Prefab().Patch();

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;

            Log.Info($"Processing patches for {assemblyName}...");
            var harmony = new Harmony($"Grimolfr_{assemblyName}");
            harmony.PatchAll(assembly);
        }
    }
}
