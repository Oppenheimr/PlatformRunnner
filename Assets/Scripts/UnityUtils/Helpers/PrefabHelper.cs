using System.IO;
using UnityEditor;
using UnityEngine;
using UnityUtils.Extensions;

namespace UnityUtils.Helpers
{
    public static class PrefabHelper
    {
        #region Unity Editor
#if UNITY_EDITOR
        public static void SavePrefab(GameObject target, string saveLocation, bool select = true)
        {
            StringExtensions.CheckDuplicate(ref saveLocation);
            var saved = PrefabUtility.SaveAsPrefabAsset(target, saveLocation);
            Object.DestroyImmediate(target);
            target = (GameObject) PrefabUtility.InstantiatePrefab(saved);
            if (select) target.Select();
        }
        
        public static void SavePrefab(GameObject target)
        {
            var saveLocation = string.Format("Assets{0}Resources{0}{1}.prefab",
                Path.DirectorySeparatorChar, target.name);
         
            StringExtensions.CheckDuplicate(ref saveLocation);
            
            var saved = PrefabUtility.SaveAsPrefabAsset(target, saveLocation);
            Object.DestroyImmediate(target);
            target = (GameObject) PrefabUtility.InstantiatePrefab(saved);
            target.Select();
        }
#endif
        #endregion
    }
}