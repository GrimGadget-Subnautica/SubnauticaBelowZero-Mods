using System;
using System.Reflection;
using HarmonyLib;
using Newtonsoft.Json.Linq;

namespace Grimolfr.SubnauticaZero.ScannerSalvage.Patches
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
                Log.Debug($"Retrieved {BlueprintHandTargetPatcher.DataBoxTechType!.Value} blueprint data chip from a data box.");
            }
            else if (techType == TechType.Titanium && num == 2 && !noMessage && spawnIfCantAdd)
            {
                // Scanned a fragment
                var scanned = PDAScanner.scanTarget.techType;
                Log.Debug($"Scanned {scanned}.");

                var recipe = scanned.GetRecipe();
                Log.Debug($"Using recipe: {Environment.NewLine}{JObject.FromObject(recipe).SerializeForLog()}");

                var materials = new MaterialsList(recipe);
                Log.Debug($"Recipe materials: {Environment.NewLine}{JArray.FromObject(materials).SerializeForLog()}");
            }

            return true;
        }

        [HarmonyPatch(nameof(CraftData.AddToInventory))]
        [HarmonyPostfix]
        public static void PostAddToInventory()
        {
            Log.Debug($"{MethodBase.GetCurrentMethod().Name}");
        }
    }
}
