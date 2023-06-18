using Core;
using UnityEditor;
using UnityEngine;

namespace GamePlay
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float smoothSpeed = 0.125f;  // Smoothness of movement
        [SerializeField] private Vector3 offset;          // Distance between camera and target

        private void LateUpdate()
        {
            // Get the target's position and set the offset to adjust the distance between the camera and the target
            var desiredPosition = GameManager.PlayerPos + offset;

            // Move the camera smoothly towards the target using the Lerp function
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            //Only x position
            var currentPos = transform.position;
            currentPos.x = smoothedPosition.x;
            
            // Update the camera's position with the new position
            transform.position = smoothedPosition;
        }

        public void SetupOffset() => offset = transform.position - GameManager.PlayerPos;
        
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(CameraController))]
    public class CameraControllerEditor : Editor
    {
        CameraController script;

        private void OnEnable()
        {
            script = (CameraController)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);

            if (GUILayout.Button("Setup Offset"))
            {
                script.SetupOffset();
            }     
        }
    }
#endif
}