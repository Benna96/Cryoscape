using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using UnityEngine;

public static class EnumerableExtensions
{
    // https://stackoverflow.com/questions/6865419/indexof-for-multiple-results
    public static IEnumerable<int> IndicesOf<T>(this Collection<T> collection, T item) where T : class
    {
        for (int i = 0; i < collection.Count; i++)
            if (collection[i] == item)
                yield return i;
    }

    public static IEnumerable<int> IndicesOf<T>(this List<T> list, T item) where T : class
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i] == item)
                yield return i;
    }
}
