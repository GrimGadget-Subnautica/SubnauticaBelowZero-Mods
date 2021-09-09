using System.Reflection;
using HarmonyLib;

namespace Grimolfr.SubnauticaZero.ScannerSalvage.Patches
{
    [HarmonyPatch(typeof(BlueprintHandTarget))]
    internal static class BlueprintHandTargetPatcher
    {
        internal static TechType? DataBoxTechType { get; private set; }

        [HarmonyPatch(nameof(BlueprintHandTarget.UnlockBlueprint))]
        [HarmonyPrefix]
        public static bool PreUnlockBlueprint(BlueprintHandTarget __instance)
        {
            Log.Debug($"{MethodBase.GetCurrentMethod().Name}::{__instance.unlockTechType}");
            Log.Debug(__instance);

            DataBoxTechType = __instance.unlockTechType;

            return true;
        }

        [HarmonyPatch(nameof(BlueprintHandTarget.UnlockBlueprint))]
        [HarmonyPostfix]
        public static void PostUnlockBlueprint()
        {
            Log.Debug(MethodBase.GetCurrentMethod().Name);

            DataBoxTechType = null;
        }
    }
}
