﻿using System.IO;
using System.Reflection;
using Grimolfr.SubnauticaZero.ExteriorPlantPots.Prefabs;
using HarmonyLib;
using QModManager.API.ModLoading;
using QModManager.Utility;
using SMLHelper.V2.Handlers;

namespace Grimolfr.SubnauticaZero.ExteriorPlantPots
{
    [QModCore]
    public static class Main
    {
        internal static ExteriorPlantPotsConfiguration Config { get; private set; }

        [QModPatch]
        public static void Initialize()
        {
            Logger.Log(Logger.Level.Info, "Registering Exterior Plant Pots configuration...");
            Config = OptionsPanelHandler.Main.RegisterModOptions<ExteriorPlantPotsConfiguration>();
            if (!File.Exists(Config.JsonFilePath))
                Config.Save();

            Logger.Log(Logger.Level.Info, "Patching Exterior Plant Pot prefabs...");
            new ExteriorPlantPotPrefab().Patch();
            new ExteriorPlantPot2Prefab().Patch();
            new ExteriorPlantPot3Prefab().Patch();

            var assembly = Assembly.GetExecutingAssembly();
            var assemblyName = assembly.GetName().Name;

            Logger.Log(Logger.Level.Info, $"Processing all pre/post patches for {assemblyName}...");
            var harmony = new Harmony($"Grimolfr_{assemblyName}");
            harmony.PatchAll(assembly);
        }
    }
}
