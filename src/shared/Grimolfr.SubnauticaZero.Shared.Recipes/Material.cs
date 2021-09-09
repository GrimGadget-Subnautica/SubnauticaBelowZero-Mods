namespace Grimolfr.SubnauticaZero
{
    internal struct Material
    {
        public Material(Ingredient ingredient)
        {
            TechType = ingredient.techType;
            Amount = ingredient.amount;

            Materials = new MaterialsList(TechType.GetRecipe());
        }

        public TechType TechType { get; set; }

        public int Amount { get; set; }

        public MaterialsList Materials { get; private set; }
    }
}
