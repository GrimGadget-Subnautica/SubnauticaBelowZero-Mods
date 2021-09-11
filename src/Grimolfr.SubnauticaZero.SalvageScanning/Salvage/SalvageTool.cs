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
        private const int _MinSalvage = 2;
        private static readonly Random _Random = new Random();

        private readonly RecipeData _recipe;
        private readonly MaterialList _materialList;


        public SalvageTool(RecipeData recipe)
        {
            _recipe = recipe ?? throw new ArgumentNullException(nameof(recipe));
            _materialList = new MaterialList(recipe);
        }

        public bool ReclaimSalvage()
        {
            var maxSalvage = Math.Max(_materialList.Count, _MinSalvage);
            var salvageCount = new Random().Next(_MinSalvage, Math.Max(_MinSalvage, maxSalvage) + 1);

            var salvage = SelectSalvage(salvageCount).ToArray();

            //CraftData.AddToInventory(_materialList.Select(m => m.TechType).Randomize().First());

            foreach (var techType in salvage)
                CraftData.AddToInventory(techType);

            Log.Debug(
                $"Salvaged: {Environment.NewLine}"
                + $"{JArray.FromObject(salvage, Log.LoggingJsonSerializer).SerializeForLog()}");

            return salvage.Length > 0;
        }

        private IEnumerable<TechType> SelectSalvage(int salvageCount)
        {
            for (var i = 0; i < salvageCount; i++)
            {
                var salvageList =
                    // Build list of possible salvage
                    BuildSalvageableItemList(_materialList)
                        // Filter list to supported items
                        .GroupJoin(Salvageable.Weights, si => si.TechType, sw => sw.Key, (item, weights) => new {Item = item, Weights = weights})
                        // Set weights
                        .Where(a => a.Weights.Any(kvp => kvp.Value is > 0.0 and <= 1.0))
                        .Select(
                            a =>
                            {
                                a.Item.Weight = a.Weights.Min(w => w.Value);
                                return a.Item;
                            })
                        .ToArray();

                Log.Debug(
                    $"Salvageable Items: {Environment.NewLine}"
                    + JArray.FromObject(salvageList, Log.LoggingJsonSerializer).SerializeForLog());

                // Select a salvage item
                var choice = SelectItem(salvageList);
                if (choice == null) yield break;

                // Destroy salvage item in materials list
                DestructiveSalvage(choice);

                // return selected item's TechType

                yield return salvageList.Select(s => s.TechType).Randomize().First();
            }
        }

        private void DestructiveSalvage(SalvageableItem choice)
        {
            if (choice == null) return;

            while (choice.Parent != null)
                choice = choice.Parent;

            var top = _materialList.FirstOrDefault(m => m.TechType == choice.TechType);

            if (top.Amount > 1)
                top.Amount--;
            else
                _materialList.Remove(top);
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
                foreach (var child in BuildSalvageableItemList(material.ComponentMaterials, si))
                    yield return child;
            }
        }

    }
}
