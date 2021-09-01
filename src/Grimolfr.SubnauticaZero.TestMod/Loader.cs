using HarmonyLib;
using QModManager.API.ModLoading;

namespace Grimware.SubnauticaZero.Mods.TestMod
{
    [QModCore]
    public static class Loader
    {
        [QModPatch]
        public static void Initialize()
        {
            var harmony = new Harmony(TextContent.ModId);
            harmony.PatchAll();
        }
    }
}
