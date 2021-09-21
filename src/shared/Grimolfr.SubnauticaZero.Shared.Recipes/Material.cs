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

            MaterialList = new MaterialList(TechType.GetRecipe(), this);
        }

        private Material(Material source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            TechType = source.TechType;
            Amount = source.Amount;

            MaterialList = new MaterialList(source.MaterialList.Select(s => new Material(s) {Parent = this}));
        }

        public int CountAllRawMaterials() => MaterialList.Count > 0 ? MaterialList.Sum(mat => mat.CountAllRawMaterials()) : 1;

        public TechType TechType { get; }

        public int Amount { get;  set; }

        public Material Parent { get; internal set; }

        public MaterialList MaterialList { get; set; }

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
