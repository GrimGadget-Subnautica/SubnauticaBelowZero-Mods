using System;
using System.Collections.Generic;
using System.Linq;

namespace Grimware.Linq
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<TResult> FullOuterGroupJoin<TA, TB, TKey, TResult>(
            this IEnumerable<TA> a,
            IEnumerable<TB> b,
            Func<TA, TKey> selectKeyA,
            Func<TB, TKey> selectKeyB,
            Func<IEnumerable<TA>, IEnumerable<TB>, TKey, TResult> projection,
            IEqualityComparer<TKey> cmp = null)
        {
            cmp ??= EqualityComparer<TKey>.Default;
            var alookup = a.ToLookup(selectKeyA, cmp);
            var blookup = b.ToLookup(selectKeyB, cmp);

            var keys = new HashSet<TKey>(alookup.Select(p => p.Key), cmp);
            keys.UnionWith(blookup.Select(p => p.Key));

            var join = from key in keys
                let xa = alookup[key]
                let xb = blookup[key]
                select projection(xa, xb, key);

            return join;
        }

        public static IEnumerable<TResult> FullOuterJoin<TA, TB, TKey, TResult>(
            this IEnumerable<TA> a,
            IEnumerable<TB> b,
            Func<TA, TKey> selectKeyA,
            Func<TB, TKey> selectKeyB,
            Func<TA, TB, TKey, TResult> projection,
            TA defaultA = default,
            TB defaultB = default,
            IEqualityComparer<TKey> cmp = null)
        {
            cmp ??= EqualityComparer<TKey>.Default;
            var aLookup = a.ToLookup(selectKeyA, cmp);
            var bLookup = b.ToLookup(selectKeyB, cmp);

            var keys = new HashSet<TKey>(aLookup.Select(p => p.Key), cmp);
            keys.UnionWith(bLookup.Select(p => p.Key));

            var join = from key in keys
                from xa in aLookup[key].DefaultIfEmpty(defaultA)
                from xb in bLookup[key].DefaultIfEmpty(defaultB)
                select projection(xa, xb, key);

            return join;
        }

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> collection)
        {
            return collection?.OrderBy(t => Guid.NewGuid());
        }
    }
}
