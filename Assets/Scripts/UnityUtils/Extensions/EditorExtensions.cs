#if UNITY_EDITOR
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;


namespace UnityUtils.Extensions
{
    /// <summary>
    ///  This class for Unity Editor. 
    /// </summary>
    public static class EditorExtensions
    {
        public static void Save(this Object target)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return;
            
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(target.GameObject().scene);
#endif
        }
        
        /// <summary>
        /// Selection changer in unity editor.
        /// </summary>
        /// <param name="target"> Select Target</param>
        public static void Select(this GameObject target)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                return;

            
            Selection.activeGameObject = target;
            SceneView.lastActiveSceneView.FrameSelected();
#endif
        }
    }
}
