using Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils.Extensions;

namespace GamePlay
{
    /// <summary>
    /// Controls characters ragdoll-related interactions and provides transitions
    /// </summary>
    public class Ragdoll : MonoBehaviour
    {
        private Animator _ragdolledAnimator;
        private Vector3 _animatorPos;
        private Quaternion _animatorRot;
        private bool IsAI => CompareTag("Enemy");
        private int ForceMultiplier => IsAI ? 5 : 1;

        private Character _char;
        public Character Character => _char ? _char : (_char = GetComponentInChildren<Character>());

        private Collider[] _colliders;
        private Collider[] Colliders => _colliders != null ? _colliders : 
            (_colliders = Character.Animator.GetComponentsInChildren<Collider>());
        
        private Rigidbody[] _rigidbodies;
        private Rigidbody[] RigidBodies => _rigidbodies != null ? _rigidbodies : 
            (_rigidbodies = Character.Animator.GetComponentsInChildren<Rigidbody>());

        private void Awake()
        {
            // Store the initial local position and rotation of the character's animator
            _animatorPos = Character.Animator.transform.localPosition;
            _animatorRot = Character.Animator.transform.localRotation;
        }

        public void AddForceRagdolled(Vector3 force)
        {
            foreach (var rigid in RigidBodies)
                rigid.AddForce(force * ForceMultiplier, ForceMode.Impulse);
        }

        public void AddForce(Vector3 force) => Character.Rigidbody.AddForce(force * ForceMultiplier, ForceMode.Impulse);

        public void SetRagdoll(bool active)
        {
            // Enable or disable colliders on the character's animator components based on the ragdoll state
            foreach (var collider in Colliders)
                collider.enabled = active;

            foreach (var rigid in RigidBodies)
                rigid.constraints = active 
                    ? RigidbodyConstraints.None 
                    : RigidbodyConstraints.FreezeAll;
            
            Character.Rigidbody.constraints = active 
                ? RigidbodyConstraints.FreezeAll 
                : RigidbodyConstraints.FreezeRotation;
            
            Character.Rigidbody.useGravity = !active;
            Character.Collider.enabled = !active;
            Character.Animator.enabled = !active;
        }

        public void CollidersClose()
        {
            // Disable or enable colliders on the character and its children, except for the main collider
            var m_collider = GetComponent<Collider>();
            var colliders = GetComponentsInChildren<Collider>();

            foreach (var collider in colliders)
            {
                if (collider != m_collider)
                    collider.enabled = !collider.enabled;
                collider.Save(); // Save the collider state
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Ragdoll))]
    public class RagdollEditor : UnityEditor.Editor
    {
        Ragdoll script;

        private void OnEnable()
        {
            script = (Ragdoll)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayout.Space(10);

            if (GUILayout.Button("Ragdoll Collider Set Active"))
                script.CollidersClose();
            if (GUILayout.Button("Ragdoll"))
                script.SetRagdoll(true);
            if (GUILayout.Button("Reset Ragdoll"))
                script.SetRagdoll(false);
        }
    }
#endif
}