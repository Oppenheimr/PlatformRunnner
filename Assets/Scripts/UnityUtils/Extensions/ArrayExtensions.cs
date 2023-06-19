using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] TakeOrdinaryAddToEnd<T>(this T[] targetArray, int index)
        {
            T[] newArray = new T[targetArray.Length];

            for (int i = 0; i < targetArray.Length; i++)
            {
                if (i == index) // If the current index matches the specified index
                {
                    newArray[targetArray.Length - 1] = targetArray[i]; // Place the element at the specified index at the end of the new array
                }
                else if (i > index) // If the current index is after the specified index
                {
                    newArray[i - 1] = targetArray[i]; // Shift the elements after the specified index one position to the left in the new array
                }
                else // If the current index is before the specified index
                {
                    newArray[i] = targetArray[i]; // Copy the element as it is to the new array
                }
            }

            return newArray;
            
            // var newArray = new T[targetArray.Length];
            //
            // for (int i = 0; i < targetArray.Length; i++)
            // {
            //     if (i == index) // If the current index matches the specified index
            //         // Assign the element at the specified index to the current index in the new array
            //         newArray[i] = targetArray[i + 1];
            //     else if (i < index) // If the current index is before the specified index
            //         // Copy the element as it is to the new array
            //         newArray[i] = targetArray[i];
            //     else // If the current index is after the specified index
            //         // Shift the elements one position to the right and copy them to the new array
            //     {
            //         if (i)
            //             newArray[i] = targetArray[i - 1];
            //     }
            // }
            //
            // // "^1" is "targetArray.Length -1"
            // newArray[^1] = targetArray[index];
            // return newArray;
        }

        public static T[] Remove<T>(this T[] targetArray, T item)
        {
            bool detected = false;
            T[] newArray = new T[targetArray.Length - 1];

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
            T[] newChar = new T[target.Length - 1];
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
            var result = new T[target.Length + 1];
            result[0] = newItem;

            for (var i = 0; i < target.Length; i++)
                result[i + 1] = target[i];
            return result;
        }

        public static T[] Add<T>(this T[] target, T newItem)
        {
            var result = new T[target.Length + 1];
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