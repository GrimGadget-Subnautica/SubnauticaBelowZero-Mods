using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using SMLHelper.V2.Crafting;

namespace Grimolfr.SubnauticaZero.SalvageScanning.Salvage
{
    internal class SalvageHelper
    {
        private const int _MinSalvage = 2;

        private readonly RecipeData _recipe;
        private readonly MaterialList _materialList;

        public SalvageHelper(RecipeData recipe)
        {
            _recipe = recipe ?? throw new ArgumentNullException(nameof(recipe));
            _materialList = new MaterialList(recipe);
        }

        private int MaxSalvage => _materialList.Count > _MinSalvage ? _materialList.Count : _MinSalvage;

        public bool ReclaimSalvage()
        {
            var salvageCount = new Random().Next(_MinSalvage, Math.Max(_MinSalvage, Math.Min(3, MaxSalvage)) + 1);

            var salvage = SelectSalvage(salvageCount).ToArray();

            foreach (var techType in salvage)
                CraftData.AddToInventory(techType);

            Log.Debug(
                $"Salvaged: {Environment.NewLine}"
                + $"{JArray.FromObject(salvage, Log.LoggingJsonSerializer).SerializeForLog()}");

            return salvage.Length > 0;
        }

        private IEnumerable<TechType> SelectSalvage(int salvageCount)
        {
            var random = new Random();
            var salvaged = new List<TechType>(salvageCount);

            for (var i = 0; i++ < salvageCount;)
            {
                var salvageItems = SelectSalvageableMaterials(this).ToArray();
                var weightTotal = salvageItems.Select(s => s.Weight * s.Amount).Sum();
                var choice = random.NextDouble() * weightTotal;

                var ptr = 0.0;
                foreach (var item in salvageItems)
                {
                    var itemWeight = item.Weight * item.Amount;
                    if (choice > ptr && choice <= ptr + itemWeight)
                    {
                        salvaged.Add(item.TechType);
                        break;
                    }

                    ptr += itemWeight;
                }
            }

            for (var c = salvaged.Count; c < _MinSalvage; c++)
                salvaged.Add(TechType.Titanium);

            return salvaged.OrderBy(tt => tt);
        }

        private static IEnumerable<SalvageableItem> SelectSalvageableMaterials(SalvageHelper helper)
        {
            var salvageItems =
                Salvageable.AllTypes
                    .GroupJoin(
                        helper._materialList.FlattenMaterialHierarchy(1),
                        m => m.TechType,
                        m => m.TechType,
                        (s, mats) => new {Salvageable = s, Materials = mats?.ToArray()})
                    .Where(a => a.Materials != null && a.Materials.Length > 0)
                    .Select(
                        a =>
                            new SalvageableItem
                            {
                                TechType = a.Salvageable.TechType,
                                Rarity = a.Salvageable.Rarity,
                                Amount = a.Materials.Length,
                                MaxDepth = a.Materials.Max(m => m.Depth)
                            })
                    .OrderByDescending(s => s.Rarity)
                    .ThenByDescending(s => s.TechType)
                    .ToArray();

            return salvageItems;
        }
    }
}
