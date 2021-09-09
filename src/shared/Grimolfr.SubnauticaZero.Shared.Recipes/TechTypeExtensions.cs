using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;

namespace Grimolfr.SubnauticaZero
{
    internal static class TechTypeExtensions
    {
        public static RecipeData GetRecipe(this TechType techType)
        {
            var entryData = PDAScanner.GetEntryData(techType);
            if (entryData == null) return null;

            return CraftDataHandler.GetRecipeData(entryData.blueprint);
        }
    }
}
