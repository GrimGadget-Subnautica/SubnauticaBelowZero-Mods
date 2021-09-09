﻿using System;
using System.Collections;
using System.Collections.Generic;
using SMLHelper.V2.Crafting;

namespace Grimolfr.SubnauticaZero
{
    public class MaterialsList
        : IList<Material>
    {
        private readonly List<Material> _list;

        public MaterialsList()
        {
            _list = new List<Material>();
        }

        public MaterialsList(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));

            _list = new List<Material>(capacity);
        }

        public MaterialsList(IEnumerable<Material> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            _list = new List<Material>(collection);
        }

        public MaterialsList(RecipeData recipe)
        {
            if (recipe == null) throw new ArgumentNullException(nameof(recipe));
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_list).GetEnumerator();
        }
    }
}
