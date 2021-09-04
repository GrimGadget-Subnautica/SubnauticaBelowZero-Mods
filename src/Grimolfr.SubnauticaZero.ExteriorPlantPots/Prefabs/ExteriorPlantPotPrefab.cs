using System;
using System.Collections;
using JetBrains.Annotations;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UnityEngine;
using Logger = QModManager.Utility.Logger;
using Object = UnityEngine.Object;

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

        public override TechCategory CategoryForPDA => TechCategory.ExteriorModule;

        public override TechGroup GroupForPDA => TechGroup.ExteriorModules;

        public override TechType RequiredForUnlock => TechType.FarmingTray;

        private protected virtual TechType BaseTechType => TechType.PlanterPot;

        public override GameObject GetGameObject()
        {
            var original = Resources.Load<GameObject>($"Submarine/Build/{BaseTechType}");

            return PreprocessPrefab(Object.Instantiate(original));
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            var taskResult = new TaskResult<GameObject>();

            yield return CraftData.GetPrefabForTechTypeAsync(BaseTechType, false, taskResult);

            gameObject?.Set(PreprocessPrefab(Object.Instantiate(taskResult.Get())));
        }

        protected override RecipeData GetBlueprintRecipe() => new RecipeData(new Ingredient(TechType.Titanium, 2)) {craftAmount = 1};

        private protected GameObject PreprocessPrefab(GameObject prefab)
        {
            if (prefab == null) return null;

            Logger.Log(Logger.Level.Debug, $"Setting Planter object properties...");
            var planter = prefab.GetComponent<Planter>();
            planter.isIndoor = false;
            planter.environment = Planter.PlantEnvironment.Dynamic;

            Logger.Log(Logger.Level.Debug, $"Setting Constructable object properties...");
            var constructable = planter.GetComponent<Constructable>();
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

            Logger.Log(Logger.Level.Debug, $"Setting TechTag object properties...");
            planter.GetComponent<TechTag>().type = TechType;

            Logger.Log(Logger.Level.Debug, $"Setting PrefabIdentifier object properties...");
            planter.GetComponent<PrefabIdentifier>().ClassId = ClassID;

            return prefab;
        }
    }
}
