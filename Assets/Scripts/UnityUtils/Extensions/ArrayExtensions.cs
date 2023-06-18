using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] Remove<T>(this T[] targetArray, T item)
        {
            bool detected = false;
            T[] newArray = new T[targetArray.Length -1 ];
            
            for (int i = 0; i < targetArray.Length - 1; i++)
            {
                if (item.Equals(targetArray[i]))
                    detected = true;

                if (!detected)
                    newArray[i] = targetArray[i];
                else
                    newArray[i] = targetArray[i + 1];
            }

            return newArray;
        }
        
        public static T[] RemoveAt<T>(this T[] target, int index)
        {
            bool detected = false;
            T[] newChar = new T[target.Length -1 ];
            for (int i = 0; i < target.Length - 1; i++)
            {
                if (index == i)
                    detected = true;

                if (!detected)
                    newChar[i] = target[i];
                else
                    newChar[i] = target[i + 1];
            }

            return newChar;
        }

        public static T[] AddFirst<T>(this T[] target, T newItem)
        {
            var result = new T[target.Length +1];
            result[0] = newItem;
            
            for (var i = 0; i < target.Length; i++)
                result[i + 1] = target[i];
            return result;
        }
        
        public static T[] Add<T>(this T[] target, T newItem)
        {
            var result = new T[target.Length +1];
            for (var i = 0; i < target.Length; i++)
                result[i] = target[i];
            
            result[target.Length] = newItem;
            return result;
        }

        public static T GetRandomItem<T>(this T[] list)
        {
            int randomIndex = Random.Range(0, list.Length);
            return list[randomIndex];
        }
    }
}