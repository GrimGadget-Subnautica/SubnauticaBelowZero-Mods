using System;
using System.Collections.Generic;
using System.Linq;
using Grimware.Linq;
using Newtonsoft.Json.Linq;
using SMLHelper.V2.Crafting;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Salvage
{
    internal class SalvageTool
    {
        private readonly TechType _techType;
        private readonly RecipeData _recipe;
        private static readonly Random _Random = new Random();

        private readonly MaterialList _materialList;

        public SalvageTool(TechType techType)
        {
            _techType = techType;

            _recipe = techType.GetRecipe();
            _materialList = new MaterialList(_recipe);
        }

        private static int MinSalvage =>
            Main.Config.OperationMode
                switch
                {
                    OperationMode.Advanced => 0,
                    OperationMode.Any => 1,
                    _ => 2
                };

        private int MaxSalvage =>
            Main.Config.OperationMode
                switch
                {
                    OperationMode.Advanced => 2,
                    _ => Math.Max(_materialList.Count, MinSalvage),
                };

        public bool ReclaimSalvage()
        {
            if (_techType == TechType.None || _recipe == null || _materialList == null || !_materialList.Any())
                return false;

            if (Main.Config.EnableVerboseLogging)
                Log.Debug(
                    $"Reclaiming salvage from {_techType}{Environment.NewLine}"
                    + $"{JArray.FromObject(_materialList, Log.LoggingJsonSerializer).SerializeForLog()}");

            var salvageCount = new Random().Next(MinSalvage, Math.Max(MinSalvage, MaxSalvage) + 1);
            Log.Debug($"Attempting to salvage {salvageCount} items {{Operation Mode: {Main.Config.OperationMode} ({MinSalvage} - {MaxSalvage})}}");

            var salvage = SelectSalvage(salvageCount).ToList();

            while (salvage.Count < MinSalvage)
                salvage.Add(TechType.Titanium);

            Log.Debug($"Salvaging: {Environment.NewLine}{JArray.FromObject(salvage, Log.LoggingJsonSerializer).SerializeForLog()}");

            foreach (var techType in salvage)
                CraftData.AddToInventory(techType);

            return salvage.Count > 0;
        }

        private static IEnumerable<SalvageableItem> BuildSalvageableItemList(IEnumerable<Material> materialList, SalvageableItem parent = null)
        {
            return
                (materialList ?? Enumerable.Empty<Material>())
                .SelectMany(m => BuildSalvageableItemList(m, parent));
        }

        private static IEnumerable<SalvageableItem> BuildSalvageableItemList(Material material, SalvageableItem parent = null)
        {
            if (material == null) yield break;

            for (var i = 0; i < material.Amount; i++)
            {
                var si = new SalvageableItem(material, parent);

                yield return si;
                foreach (var child in BuildSalvageableItemList(material.MaterialList, si))
                    yield return child;
            }
        }

        private static SalvageableItem SelectItem(IEnumerable<SalvageableItem> salvageList)
        {
            var salvageArray = salvageList as SalvageableItem[] ?? salvageList.ToArray();

            if (!salvageArray.Any()) return null;

            var x = salvageArray.Sum(s => s.Weight) * _Random.NextDouble();
            foreach (var item in salvageArray.OrderByDescending(s => s.Weight))
            {
                if ((x -= item.Weight) < 0.0) return item;
            }

            return null;
        }

        private void DestroySalvagedComponent(SalvageableItem choice)
        {
            if (choice == null) return;

            while (choice.Parent != null)
                choice = choice.Parent;

            var top = _materialList.First(m => m.TechType == choice.TechType);

            if (Main.Config.EnableVerboseLogging)
                Log.Debug($"Removing 1x {top.TechType} from salvageable materials list...");

            if (top.Amount > 1)
                top.Amount--;
            else
                _materialList.Remove(top);
        }

        private IEnumerable<TechType> SelectSalvage(int salvageCount)
        {
            for (var i = 0; i < salvageCount; i++)
            {
                // Build list of possible salvage
                // Filter list to supported items and set weights
                var salvageableItemList =
                    BuildSalvageableItemList(_materialList)
                        .GroupJoin(Salvageable.Weights, si => si.TechType, sw => sw.Key, (item, weights) => new {Item = item, Weights = weights})
                        .Where(a => a.Weights.Any(kvp => kvp.Value is > 0.0 and <= 10.0))
                        .Select(
                            a =>
                            {
                                a.Item.Weight *= a.Weights.Where(kvp => kvp.Value is > 0.0 and <= 10.0).Min(w => w.Value);
                                return a.Item;
                            })
                        .ToArray();

                if (Main.Config.EnableVerboseLogging)
                    Log.Debug(
                        $"Salvageable Item List: {Environment.NewLine}"
                        + JArray.FromObject(salvageableItemList, Log.LoggingJsonSerializer).SerializeForLog());

                // Select a salvage item
                var choice = SelectItem(salvageableItemList);
                if (choice == null) yield break;

                // Destroy salvage item in materials list
                DestroySalvagedComponent(choice);

                // return selected item's TechType

                yield return choice.TechType;
            }
        }
    }
}
