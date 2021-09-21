using System.Collections.Concurrent;
using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;

namespace Grimolfr.SubnauticaZero
{
    internal static class TechTypeExtensions
    {
        private static readonly ConcurrentDictionary<TechType, RecipeData> _TechRecipes = new ConcurrentDictionary<TechType, RecipeData>();

        public static RecipeData GetRecipe(this TechType? techType) => techType == null ? null : GetRecipe(techType.Value);

        public static RecipeData GetRecipe(this TechType techType) =>
            _TechRecipes.GetOrAdd(techType, tt => GetTechRecipe(tt) ?? GetScannerEntryRecipe(tt));

        private static RecipeData GetScannerEntryRecipe(TechType techType)
        {
            var entryData = PDAScanner.GetEntryData(techType);
            return
                entryData != null
                    ? CraftDataHandler.GetRecipeData(entryData.blueprint)
                    : null;
        }

        private static RecipeData GetTechRecipe(TechType techType) => CraftDataHandler.GetRecipeData(techType);
    }
}
