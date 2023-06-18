using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Sondan başlayarak hedef kelimeleri siler örnek:
        /// Remote(Clone) - (Clone) => Remote
        /// </summary>
        /// <param name="str"></param>
        /// <param name="minus"></param>
        /// <returns></returns>
        public static string Eject(this string str, string minus)
        {
            char[] strArray = str.ToCharArray();
            char[] minusArray = minus.ToCharArray();
            
            int strArrayLength = strArray.Length - 1;
            int minusArrayLength = minusArray.Length - 1;
            
            for (int i = strArrayLength ; i > 0; i--)
            {
                for (int i2 = minusArrayLength ; i2 >= 0; i2--)
                {
                    if(strArray.Length <= i || minusArray.Length <= i2)
                        continue;
                    
                    if (strArray[i] == minusArray[i2])
                    {
                        strArray = strArray.RemoveAt(i);
                        minusArray = minusArray.RemoveAt(i2);
                        strArrayLength--;
                        minusArrayLength--;
                    }
                    else break;
                }
            }

            string newStr= "";
            foreach (var newCharacter in strArray)
            {
                newStr += newCharacter;
            }
            return newStr;
        }
        
        #region Unity Editor
#if UNITY_EDITOR
        public static string CheckDuplicate(ref string path)
        {
            if (!FileExists(path)) return path;
            
            if (EditorUtility.DisplayDialog("Duplicate Detected!",
                    "A prefab with the same name already exists, would you like to overwrite it or create a new one, keeping both?",
                    "Overwrite", "Keep Both"))
            {
                //Overwrite
                DeleteFile(path);
                Debug.Log("Generating prefab at: " + path);
            }
            else
            {
                //Keep Both
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                Debug.Log("Generating prefab at: " + path);
            }
            return path;
        }
#endif
        #endregion

        public static bool FileExists(string path)
        {
            if (!path.StartsWith("Assets" + Path.DirectorySeparatorChar))
            {
                path = "Assets" + Path.DirectorySeparatorChar + path;
            }
            return File.Exists(path);
        }
        
        private static void DeleteFile(string path)
        {
            if (!path.StartsWith("Assets" + Path.DirectorySeparatorChar))
            {
                path = "Assets" + Path.DirectorySeparatorChar + path;
            }
            File.Delete(path);
        }
    }
}