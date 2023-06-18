using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class ListExtensions
    {
        public static T GetRandomItem<T>(this List<T> list)
        {
            int randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static T TryGet<T>(this List<T> list)
        {
            int randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }


        public static List<T> AddList<T>(this List<T> list, params List<T>[] add)
        {
            list.AddRange(add.SelectMany(addList => addList));

            return list;
        }
    }
}