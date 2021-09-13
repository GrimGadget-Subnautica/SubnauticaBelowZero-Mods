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
        private static readonly Random _Random = new Random();

        private readonly MaterialList _materialList;


        public SalvageTool(RecipeData recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));
            _materialList = new MaterialList(recipe);
        }

        private static int MinSalvage =>
            Main.Config.OperationMode
                switch
                {
                    OperationMode.Basic => 2,
                    OperationMode.Advanced => 0,
                    OperationMode.Any => 1,
                    _ => 2
                };

        private int MaxSalvage =>
            Main.Config.OperationMode
                switch
                {
                    OperationMode.Basic => Math.Max(_materialList.Count, MinSalvage),
                    OperationMode.Advanced => 2,
                    OperationMode.Any => 3,
                    _ => 2,
                };

        public bool ReclaimSalvage()
        {
            var salvage = SelectSalvage(new Random().Next(MinSalvage, Math.Max(MinSalvage, MaxSalvage) + 1)).ToList();

            for (var i = 0; i < MinSalvage - salvage.Count; i++) salvage.Add(TechType.Titanium);

            Log.Debug(
                $"Salvaging: {Environment.NewLine}"
                + $"{JArray.FromObject(salvage, Log.LoggingJsonSerializer).SerializeForLog()}");

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
                foreach (var child in BuildSalvageableItemList(material.ComponentMaterials, si))
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

        private void DestructiveSalvage(SalvageableItem choice)
        {
            if (choice == null) return;

            while (choice.Parent != null)
                choice = choice.Parent;

            var top = _materialList.First(m => m.TechType == choice.TechType);

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

                // Select a salvage item
                var choice = SelectItem(salvageList);
                if (choice == null) yield break;

                // Destroy salvage item in materials list
                DestructiveSalvage(choice);

                // return selected item's TechType

                yield return salvageList.Select(s => s.TechType).Randomize().First();
            }
        }
    }
}
