using UnityEditor;

namespace UnityUtils.Helpers
{
    public class PathHelper
    {
        #region Unity Editor
#if UNITY_EDITOR
        public static T GetFromPath<T>(string filter, string searchInFolder) where T : class
        {
            var paths = AssetDatabase.FindAssets(filter, new [] { searchInFolder });
            string path = AssetDatabase.GUIDToAssetPath(paths[0]);
            return AssetDatabase.LoadAssetAtPath(path, typeof(T)) as T;
        }
#endif
        #endregion
    }
}