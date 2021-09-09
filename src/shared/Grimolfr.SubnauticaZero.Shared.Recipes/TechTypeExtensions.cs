using SMLHelper.V2.Crafting;
using SMLHelper.V2.Handlers;

namespace Grimolfr.SubnauticaZero
{
    internal static class TechTypeExtensions
    {
        public static RecipeData GetRecipe(this TechType techType)
        {
            return ((TechType?)techType).GetRecipe();
        }

        public static RecipeData GetRecipe(this TechType? techType)
        {
            return
                CraftDataHandler.GetRecipeData(techType ?? default)
                ?? CraftDataHandler.GetRecipeData(PDAScanner.GetEntryData(techType ?? default).blueprint);
        }
    }
}
