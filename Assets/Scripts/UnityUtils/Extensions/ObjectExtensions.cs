using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class ObjectExtensions
    {
        public static bool TryGetComponentInParent<T>(this Object thisGameObject, out T component)
        {
            component = thisGameObject.GameObject().GetComponentInParent<T>();
            return component is not null;
        }
        
        public static bool TryGetComponentsInChildren<T>(this Object thisGameObject, out List<T> components)
        {
            components = thisGameObject.GameObject().GetComponentsInChildren<T>().ToList();
            if (components == null) return false;
            return components.Count > 0;
        }
        
        public static bool TryGetComponentInChildren<T>(this Object thisGameObject, out T component)
        {
            component = thisGameObject.GameObject().GetComponentInChildren<T>();
            return component is not null;
        }
        
        #region Unity Editor
#if UNITY_EDITOR
        public static void OpenObjectInEditor(this Object target)
        {
            string path = AssetDatabase.GetAssetPath(target);
            Object obj = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
            Selection.activeObject = obj;
        }
#endif
        #endregion
    }
}