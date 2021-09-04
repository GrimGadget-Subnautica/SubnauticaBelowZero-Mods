namespace Grimolfr.SubnauticaZero.ExteriorPlantPots.Prefabs
{
    [HarmonyLib.HarmonyPatch]
    public class ExteriorPlantPot3Prefab
        : ExteriorPlantPotPrefab
    {
        private const string _ClassId = "ExteriorPlanterPot3";
        private const string _FriendlyName = "Chic Exterior Plant Pot";
        private const string _Description = "Upmarket plant pot, suitable for use on land or underwater.";

        public ExteriorPlantPot3Prefab()
            : base(_ClassId, _FriendlyName, _Description)
        {
        }

        private protected override TechType BaseTechType => TechType.PlanterPot3;
    }
}
