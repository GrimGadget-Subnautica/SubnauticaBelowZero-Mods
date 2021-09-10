using System.Reflection;
using Grimolfr.SubnauticaZero.SalvageScanning.Salvage;
using HarmonyLib;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Patches
{
    [HarmonyPatch(typeof(CraftData))]
    internal static class CraftDataPatcher
    {
        [HarmonyPatch(nameof(CraftData.AddToInventory))]
        [HarmonyPrefix]
        public static bool PreAddToInventory(
            TechType techType,
            int num = 1,
            bool noMessage = false,
            bool spawnIfCantAdd = true)
        {
            Log.Debug($"{MethodBase.GetCurrentMethod().Name}::{techType} ({num})");

            if (BlueprintHandTargetPatcher.DataBoxTechType != null)
            {
                // Got a blueprint chip from a data box
                Log.Info($"Retrieved {BlueprintHandTargetPatcher.DataBoxTechType!.Value} blueprint data chip from a data box.");
                return true;
            }

            if (techType == TechType.Titanium && num == 2 && !noMessage && spawnIfCantAdd)
            {
                // Scanned a fragment
                var scannedTech = PDAScanner.scanTarget.techType;
                Log.Info($"Scanned {scannedTech}.");

                // reclaim salvage from the fragment
                return !new SalvageHelper(scannedTech.GetRecipe()).ReclaimSalvage();
            }

            // If we made it here, we didn't do anything.  Move on to the next patch, if any.
            return true;
        }
    }
}
