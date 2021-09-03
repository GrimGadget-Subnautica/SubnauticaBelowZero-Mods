using System.Collections;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UnityEngine;

namespace Grimolfr.SubnauticaZero.ExteriorPlantPots.Prefabs
{
    [HarmonyLib.HarmonyPatch]
    public class ExteriorPlantPotPrefab
        : Buildable
    {
        private const string _ClassId = "ExteriorPlanterPot";
        private const string _FriendlyName = "Basic Exterior Plant Pot";
        private const string _Description = "Titanium plant pot, suitable for use on land or underwater.";

        public ExteriorPlantPotPrefab()
            : base(_ClassId, _FriendlyName, _Description)
        {
        }

        private protected ExteriorPlantPotPrefab(string classId, string friendlyName, string description)
            : base(classId, friendlyName, description)
        {
        }

        public virtual Color ColorTint => Main.Config?.CustomTint ?? new Color(0.5f, 1.0f, 0, 83f);

        public override TechCategory CategoryForPDA => TechCategory.ExteriorModule;

        public override TechGroup GroupForPDA => TechGroup.ExteriorModules;

        public override TechType RequiredForUnlock => TechType.FarmingTray;

        private protected virtual TechType BaseTechType => TechType.PlanterPot;

        public override GameObject GetGameObject()
        {
            var original = Resources.Load<GameObject>($"Submarine/Build/{BaseTechType}");

            return PreProcessPrefab(Object.Instantiate(original));
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            var taskResult = new TaskResult<GameObject>();

            yield return CraftData.GetPrefabForTechTypeAsync(BaseTechType, false, taskResult);

            gameObject?.Set(PreProcessPrefab(Object.Instantiate(taskResult.Get())));
        }

        protected override RecipeData GetBlueprintRecipe() => new RecipeData(new Ingredient(TechType.Titanium, 2)) {craftAmount = 1};

        private protected GameObject PreProcessPrefab(GameObject prefab)
        {
            var pot = prefab.GetComponent<Planter>();

            var constructable = pot.GetComponent<Constructable>();
            constructable.techType = TechType;

            constructable.allowedInBase = false;
            constructable.allowedInSub = false;
            constructable.allowedOutside = true;
            constructable.allowedOnCeiling = false;
            constructable.allowedOnGround = true;
            constructable.allowedOnWall = false;
            constructable.allowedOnConstructables = false;

            constructable.forceUpright = true;
            constructable.controlModelState = true;
            constructable.rotationEnabled = true;

            var planter = pot.GetComponent<Planter>();
            planter.isIndoor = false;
            planter.environment = Planter.PlantEnvironment.Dynamic;

            var techTag = pot.GetComponent<TechTag>();
            techTag.type = TechType;

            var identifier = pot.GetComponent<PrefabIdentifier>();
            identifier.ClassId = ClassID;

            prefab.GetComponentInChildren<SkinnedMeshRenderer>(true).material.color = ColorTint;

            return prefab;
        }
    }
}
