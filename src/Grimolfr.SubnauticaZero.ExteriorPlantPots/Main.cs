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
        internal static ExteriorPlantPotConfiguration Config { get; private set; }

        [QModPatch]
        public static void Initialize()
        {
            Config = OptionsPanelHandler.Main.RegisterModOptions<ExteriorPlantPotConfiguration>();

            if (!File.Exists(Config.JsonFilePath))
                Config.Save();

            new ExteriorPlantPotPrefab().Patch();

            var assembly = Assembly.GetExecutingAssembly();
            var harmony = new Harmony($"Grimolfr_{assembly.GetName().Name}");
            harmony.PatchAll(assembly);

            /*
             * ExteriorPlanterPot2ClassId,
             * "Composite Exterior Plant Pot",
             * "Designer plant pot, suitable for use on land or underwater.",
             * "Submarine/Build/PlanterPot2",
             * TechType.PlanterPot2
             *
             * ExteriorPlanterPot3ClassId,
             * "Chic Exterior Plant Pot",
             * "Upmarket plant pot, suitable for use on land or underwater.",
             * "Submarine/Build/PlanterPot3",
             * TechType.PlanterPot3
            */
        }
    }
}
