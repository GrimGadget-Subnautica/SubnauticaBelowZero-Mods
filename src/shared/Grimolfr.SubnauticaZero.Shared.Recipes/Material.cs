using System;
using System.Linq;

namespace Grimolfr.SubnauticaZero
{
    internal class Material
        : ICloneable
    {
        public Material(Ingredient ingredient)
        {
            TechType = ingredient.techType;
            Amount = ingredient.amount;

            ComponentMaterials = new MaterialList(TechType.GetRecipe(), this);
        }

        private Material(Material source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            TechType = source.TechType;
            Amount = source.Amount;

            ComponentMaterials = new MaterialList(source.ComponentMaterials.Select(s => new Material(s) {Parent = this}));
        }

        public TechType TechType { get; }

        public int Amount { get;  set; }

        public Material Parent { get; internal set; }

        public MaterialList ComponentMaterials { get; set; }

        public Material Clone()
        {
            return new Material(this);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
