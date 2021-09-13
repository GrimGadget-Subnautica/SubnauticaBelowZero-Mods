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
            bool __runOriginal,
            TechType techType,
            int num = 1,
            bool noMessage = false,
            bool spawnIfCantAdd = true)
        {
            Log.Debug($"{MethodBase.GetCurrentMethod().Name}::{techType} ({num})");

            if (!__runOriginal) return false;

            if (BlueprintHandTargetPatcher.DataBoxTechType != null)
            {
                // Got a blueprint chip from a data box
                Log.Info($"Retrieved {BlueprintHandTargetPatcher.DataBoxTechType!.Value} blueprint data chip from a data box.");

                // All we got was a thumb drive with a blueprint that we already unlocked.  We'll melt it down for the metals.
                CraftData.AddToInventory(TechType.Copper);
                CraftData.AddToInventory(TechType.Gold);

                return false;
            }

            if (techType == TechType.Titanium && num == 2 && !noMessage && spawnIfCantAdd)
            {
                // Scanned a fragment
                var scannedTech = PDAScanner.scanTarget.techType;
                Log.Info($"Scanned {scannedTech}.");
                if (scannedTech == TechType.None) return true;


                // reclaim salvage from the fragment
                var recipe = scannedTech.GetRecipe();
                if (recipe == null) return true;
                return !new SalvageTool(recipe).ReclaimSalvage();
            }

            // If we made it here, we didn't do anything.  Move on to the next patch, if any.
            return true;
        }
    }
}
