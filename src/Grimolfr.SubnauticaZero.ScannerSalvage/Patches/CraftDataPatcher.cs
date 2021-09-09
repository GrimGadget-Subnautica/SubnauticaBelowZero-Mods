using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;

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

            if (techType == TechType.Titanium && num == 2 && !noMessage && spawnIfCantAdd)
            {
                if (BlueprintHandTargetPatcher.DataBoxTechType != null)
                {
                    // Got a blueprint chip from a data box
                    Log.Debug($"Retrieved {BlueprintHandTargetPatcher.DataBoxTechType!.Value} blueprint data chip from a data box.");
                }
                else
                {
                    // Scanned a fragment
                    var scanned = PDAScanner.scanTarget.techType;

                    Log.Debug($"Scanned {scanned}.");

                    var recipe = scanned.GetRecipe();

                    Log.Debug($"Using recipe: {Environment.NewLine}{JObject.FromObject(recipe).SerializeForLog()}");
                }
            }

            return true;
        }

        [HarmonyPatch(nameof(CraftData.AddToInventory))]
        [HarmonyPostfix]
        public static void PostAddToInventory()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name);
        }
    }
}
