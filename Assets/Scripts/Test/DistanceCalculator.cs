using UnityEditor;
using UnityEngine;
using UnityUtils.Extensions;

namespace Editor.Test
{
    public class DistanceCalculator : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Transform other;
        
        public void Calculate() => Debug.Log(target.position.Distance(other.position));
        
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(DistanceCalculator))]
    public class DistanceCalculatorEditor : UnityEditor.Editor
    {
        DistanceCalculator script;

        private void OnEnable()
        {
            script = (DistanceCalculator)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);

            if (GUILayout.Button("Calculate Distance"))
            {
                script.Calculate();
            }     
        }
    }
#endif
}