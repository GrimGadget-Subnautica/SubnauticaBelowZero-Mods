using System;
using System.Collections.Generic;
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

            ComponentMaterials = new MaterialList(TechType.GetRecipe());
        }

        private Material(Material source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            TechType = source.TechType;
            Amount = source.Amount;

            ComponentMaterials = source.ComponentMaterials.Clone();
        }

        public TechType TechType { get; }

        public int Amount { get; }

        public MaterialList ComponentMaterials { get; }

        public Material Clone()
        {
            return new Material(this);
        }

        public IEnumerable<(TechType TechType, int Depth)> FlattenMaterialHierarchy(int fromDepth = 0) => FlattenMaterialHierarchy(this, fromDepth);

        internal static IEnumerable<(TechType TechType, int Depth)> FlattenMaterialHierarchy(Material material, int depth) =>
            Enumerable.Repeat<(TechType TechType, int Depth)>((material.TechType, depth), material.Amount)
                .Concat(material.ComponentMaterials.FlattenMaterialHierarchy(depth + 1));

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
