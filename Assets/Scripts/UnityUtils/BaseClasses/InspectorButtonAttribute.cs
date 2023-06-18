using UnityEditor;
using UnityEngine;

namespace UnityUtils.BaseClasses
{
    /// <summary>
    /// This attribute can only be applied to fields because its
    /// associated PropertyDrawer only operates on fields (either
    /// public or tagged with the [SerializeField] attribute) in
    /// the target MonoBehaviour.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public InspectorButtonAttribute()
        {
                
        }
    }
    
#if UNITY_EDITOR
    public abstract class InspectorButtonPropertyDrawer<T> : PropertyDrawer
    {
        //For Example : return bl_GameData.Instance.AllWeaponStringList();
        public abstract string[] DisplayedOptions();
        
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            prop.intValue = EditorGUI.Popup(position, prop.displayName, prop.intValue, DisplayedOptions());
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + 2;
        }
    }
#endif
}