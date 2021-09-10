using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SMLHelper.V2.Crafting;

namespace Grimolfr.SubnauticaZero
{
    internal class MaterialList
        : IList<Material>, ICloneable
    {
        private readonly List<Material> _list;

        public MaterialList()
        {
            _list = new List<Material>();
        }

        public MaterialList(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            _list = new List<Material>(capacity);
        }

        public MaterialList(IEnumerable<Material> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            _list = new List<Material>(collection);
        }

        public MaterialList(RecipeData recipe)
        {
            _list = new List<Material>(recipe?.ingredientCount ?? 0);

            if (recipe != null)
                _list.AddRange(recipe.Ingredients.Select(i => new Material(i)));
        }

        public int Count => _list.Count;

        public bool IsReadOnly => false;

        public Material this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }

        public void Add(Material item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public MaterialList Clone()
        {
            return new MaterialList(this.Select(m => m.Clone()));
        }

        public bool Contains(Material item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(Material[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Material> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        public int IndexOf(Material item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, Material item)
        {
            _list.Insert(index, item);
        }

        public bool Remove(Material item)
        {
            return _list.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        internal IEnumerable<(TechType TechType, int Depth)> FlattenMaterialHierarchy(int depth = 0)
        {
            return this.SelectMany(mat => Material.FlattenMaterialHierarchy(mat, depth));
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }
    }
}
