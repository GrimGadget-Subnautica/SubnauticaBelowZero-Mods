namespace Grimolfr.SubnauticaZero.ExteriorPlantPots.Prefabs
{
    [HarmonyLib.HarmonyPatch]
    public class ExteriorPlantPot2Prefab
        : ExteriorPlantPotPrefab
    {
        private const string _ClassId = "ExteriorPlanterPot2";
        private const string _FriendlyName = "Composite Exterior Plant Pot";
        private const string _Description = "Designer plant pot, suitable for use on land or underwater.";

        public ExteriorPlantPot2Prefab()
            : base(_ClassId, _FriendlyName, _Description)
        {
        }

        private protected override TechType BaseTechType => TechType.PlanterPot2;
    }
}
