#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Editor
{
    /// <summary>
    /// Tool that helps developers in their work in the editor.
    /// </summary>
    public class EditorHelper : MonoBehaviour
    {
        public static void ReplaceWithComponent(GameObject target, Type originalType, Type newType)
        {
            if (!target.GetComponent(originalType) || target.GetComponent(newType)) return;
            Debug.Log($"Replacing {originalType.Name} -> {newType.Name}.");

            if (!target.GetComponent(originalType)) return;
            Component orgComp = target.GetComponent(originalType);
          
            if (!target.GetComponent(newType))
                target.AddComponent(newType);
            

            Component newComp = target.GetComponent(newType);
            List<string> skips = CopyCompValues(orgComp, newComp);
            if(skips.Count == 0) DestroyImmediate(orgComp);
        }

        public static List<string> CopyCompValues(Component source, Component dest, bool showDebug = false)
        {
            List<string> skips = new List<string>();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            FieldInfo[] fields = source.GetType().GetFields(flags);
            foreach (FieldInfo field in fields)
            {
                try
                {
                    if (field.IsPrivate) continue;
                    
                    if (dest != null && dest.GetType().GetField(field.Name, flags) != null)
                    {
                        dest.GetType().GetField(field.Name, flags).SetValue(dest, field.GetValue(source));
                    }
                    else
                    {
                        Debug.LogWarning("(" + source.name + ") Unable to copy field: " + field.Name);
                        skips.Add(field.Name);
                    }
                }
                catch (Exception ex)
                {
                    if (showDebug)
                    {
                        Debug.Log("Failed to copy field \"" + field.Name + "\": " + ex);
                    }
                }
            }
            
            EditorUtility.SetDirty(source.gameObject);
            EditorUtility.SetDirty(dest.gameObject);
        
            return skips;
        }
        
        public static Component CopyComponentValues(Component source, Component dest, bool showDebug = false)
        {
            List<string> skips = new List<string>();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            FieldInfo[] fields = source.GetType().GetFields(flags);
            foreach (FieldInfo field in fields)
            {
                try
                {
                    if (dest != null && dest.GetType().GetField(field.Name, flags) != null)
                    {
                        dest.GetType().GetField(field.Name, flags).SetValue(dest, field.GetValue(source));
                    }
                    else
                    {
                        Debug.LogWarning("(" + source.name + ") Unable to copy field: " + field.Name);
                        skips.Add(field.Name);
                    }
                }
                catch (Exception ex)
                {
                    if (showDebug)
                    {
                        Debug.Log("Failed to copy field \"" + field.Name + "\": " + ex);
                    }
                }
            }
            
            EditorUtility.SetDirty(source.gameObject);
            EditorUtility.SetDirty(dest.gameObject);
        
            return dest;
        }
    
        public static Component CutAndCopyComponentValues(Component source, Component dest, bool showDebug = false)
        {
            if (source.GetType() != dest.GetType()) dest = dest.gameObject.AddComponent(source.GetType());
        
            List<string> skips = new List<string>();
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            FieldInfo[] fields = source.GetType().GetFields(flags);
            foreach (FieldInfo field in fields)
            {
                try
                {
                    if (dest != null && dest.GetType().GetField(field.Name, flags) != null)
                    {
                        dest.GetType().GetField(field.Name, flags).SetValue(dest, field.GetValue(source));
                    }
                    else
                    {
                        Debug.LogWarning("(" + source.name + ") Unable to copy field: " + field.Name);
                        skips.Add(field.Name);
                    }
                }
                catch (Exception ex)
                {
                    if (showDebug)
                    {
                        Debug.Log("Failed to copy field \"" + field.Name + "\": " + ex);
                    }
                }
            }
        
            EditorUtility.SetDirty(source.gameObject);
            EditorUtility.SetDirty(dest.gameObject);
        
            return dest;
        }

        public static void AlignToFloors(Component source)
        {
            Debug.Log(source.transform.position);

            RaycastHit r;

            float errorRate = -0.1f;
            Transform t = source.transform;

            Ray ray = new Ray(t.position + t.up, Vector3.down);
            if (Physics.Raycast(ray, out r, 10000))
            {
                while (r.transform.gameObject == source.gameObject)
                {
                    Vector3 startRay = new Vector3(r.point.x, r.point.y + errorRate, r.point.z);
                    Ray ray2 = new Ray(startRay, Vector3.down);

                    errorRate -= 0.1f;
                    Physics.Raycast(ray2, out r, 100);
                    if (errorRate <= -100)
                    {
                        break;
                    }
                }

                if (r.transform.gameObject != source.transform)
                {
                    t.position = r.point;
                    EditorUtility.SetDirty(t);
                    EditorSceneManager.MarkSceneDirty(t.gameObject.scene);
                }
                else
                {
                    Debug.Log(source.transform.name + "basaramadi");
                }
            }

        }
    }

    public class CopyComp : UnityEditor.Editor
    {
        public static SerializedObject OPP_COMP_COPY;
        
        [MenuItem("CONTEXT/Transform/OPP Align To Floors", false, 2)]
        public static void OPP_AlignToFloors(MenuCommand command)
        {
            SerializedObject sObject = new SerializedObject(command.context);
            EditorHelper.AlignToFloors((Component)sObject.targetObject);
        }

        [MenuItem("CONTEXT/Component/OPP Copy Component Values", false, 2)]
        public static void OPP_CONTEXT_COPY(MenuCommand command)
        {
            OPP_COMP_COPY = new SerializedObject(command.context);
        }
        
        [MenuItem("CONTEXT/Component/OPP Paste Component Values", false, 2)]
        public static void OPP_CONTEXT_PASTE(MenuCommand command)
        {
            if (OPP_COMP_COPY == null)
            {
                if (EditorUtility.DisplayDialog("No Source Component",
                        "You don't have a source component targeted. First select \"Copy Component Values\" on the " +
                        "component that you would like to copy.",
                        "Okay"))
                {
                    return;
                }
            }
            else
            {
                SerializedObject target = new SerializedObject(command.context);
                if (EditorUtility.DisplayDialog("Paste Copied Component?",
                        "Would you like to paste \"" + OPP_COMP_COPY.targetObject + "\" component's values to: \"" + target.targetObject + "\"?",
                        "Yes", "No"))
                {
                    EditorHelper.CopyComponentValues((Component)OPP_COMP_COPY.targetObject, (Component)target.targetObject, true);
                }
            }
        }
        
        [MenuItem("CONTEXT/Component/OPP Cut & Paste Component Values", false, 2)]
        public static void OPP_CONTEXT_CUT_PASTE(MenuCommand command)
        {
            if (OPP_COMP_COPY == null)
            {
                if (EditorUtility.DisplayDialog("No Source Component",
                        "You don't have a source component targeted. First select \"Copy Component Values\" on the " +
                        "component that you would like to copy.",
                        "Okay"))
                {
                    return;
                }
            }
            else
            {
                SerializedObject target = new SerializedObject(command.context);
                EditorHelper.CutAndCopyComponentValues((Component)OPP_COMP_COPY.targetObject, (Component)target.targetObject, true);
                DestroyImmediate((Component)OPP_COMP_COPY.targetObject);
            }
        }
    }
#endif
}