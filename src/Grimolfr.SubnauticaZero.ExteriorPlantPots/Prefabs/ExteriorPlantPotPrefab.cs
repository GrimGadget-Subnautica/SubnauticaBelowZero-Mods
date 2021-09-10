using System;
using System.Collections;
using SMLHelper.V2.Assets;
using SMLHelper.V2.Crafting;
using UnityEngine;
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

        public virtual Color MaterialColor => Color.white;

        public override TechCategory CategoryForPDA => TechCategory.ExteriorModule;

        public override TechGroup GroupForPDA => TechGroup.ExteriorModules;

        public override string IconFileName => $"{ClassID}.png";

        public override TechType RequiredForUnlock => Main.Config.RequireExteriorGrowbed ? TechType.FarmingTray : BaseTechType;

        private protected virtual TechType BaseTechType => TechType.PlanterPot;

        public override GameObject GetGameObject()
        {
            var original = Resources.Load<GameObject>($"Submarine/Build/{BaseTechType}");

            return CreateInstanceFrom(Object.Instantiate(original));
        }

        public override IEnumerator GetGameObjectAsync(IOut<GameObject> gameObject)
        {
            var taskResult = new TaskResult<GameObject>();

            yield return CraftData.GetPrefabForTechTypeAsync(BaseTechType, false, taskResult);

            gameObject?.Set(CreateInstanceFrom(Object.Instantiate(taskResult.Get())));
        }

        protected override RecipeData GetBlueprintRecipe() => new RecipeData(new Ingredient(TechType.Titanium, 2)) {craftAmount = 1};

        protected override Sprite GetItemSprite()
        {
            return SpriteManager.Get(BaseTechType);
        }

        private GameObject CreateInstanceFrom(GameObject prefab)
        {
            if (prefab == null) return null;

            Log.Debug($"Creating new instance of {TechType}");
            var instance = Object.Instantiate(prefab);

            var constructable = instance.GetComponent<Constructable>();
            constructable.techType = TechType;
            constructable.allowedInBase = false;
            constructable.allowedInSub = false;
            constructable.allowedOnCeiling = false;
            constructable.allowedOnWall = false;
            constructable.allowedOutside = true;
            constructable.allowedOnGround = true;
            constructable.allowedOnConstructables = false;
            constructable.forceUpright = true;
            constructable.rotationEnabled = true;

            var planter = instance.GetComponent<Planter>();
            planter.isIndoor = false;
            planter.environment = Planter.PlantEnvironment.Dynamic;

            instance.GetComponent<TechTag>().type = TechType;

            instance.GetComponent<PrefabIdentifier>().ClassId = ClassID;

            foreach (var renderer in instance.GetComponentsInChildren<Renderer>(true) ?? Array.Empty<Renderer>())
            {
                foreach (var material in renderer?.materials ?? Array.Empty<Material>())
                    if (material != null)
                        material.color = MaterialColor;
            }

            var largeWorldEntity = instance.AddComponent<LargeWorldEntity>();
            largeWorldEntity.cellLevel = LargeWorldEntity.CellLevel.Medium;

            return instance;
        }
    }
}
