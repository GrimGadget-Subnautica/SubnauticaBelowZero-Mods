using HarmonyLib;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Patches
{
    [HarmonyPatch(typeof(BlueprintHandTarget))]
    internal static class BlueprintHandTargetPatcher
    {
        internal static TechType? DataBoxTechType { get; private set; }

        [HarmonyPatch(nameof(BlueprintHandTarget.UnlockBlueprint))]
        [HarmonyPrefix]
        public static bool PreUnlockBlueprint(BlueprintHandTarget __instance)
        {
            DataBoxTechType = __instance.unlockTechType;

            return true;
        }

        [HarmonyPatch(nameof(BlueprintHandTarget.UnlockBlueprint))]
        [HarmonyPostfix]
        public static void PostUnlockBlueprint()
        {
            DataBoxTechType = null;
        }
    }
}
