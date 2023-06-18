using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils.Extensions;

namespace GamePlay
{
    public class Ragdoll : MonoBehaviour
    {
        private Animator _ragdolledAnimator;
        private Vector3 _animatorPos;
        private Quaternion _animatorRot;
        private bool IsAI => CompareTag("Enemy");
        private int ForceMultiplier => IsAI ? 5 : 1;
        
        private Character _char;
        public Character Character => _char ? _char : (_char =  GetComponentInChildren<Character>());

        private void Awake()
        {
            _animatorPos = Character.Animator.transform.localPosition;
            _animatorRot = Character.Animator.transform.localRotation;
        }

        public void AddForceRagdolled(Vector3 force)
        {
            var spine = Character.Animator.GetBoneTransform(HumanBodyBones.Spine);
            var spineRigid = spine.GetComponent<Rigidbody>();
            spineRigid.AddForce(force * ForceMultiplier, ForceMode.Impulse);
        }
        
        public void AddForce(Vector3 force) => Character.Rigidbody.AddForce(force * ForceMultiplier, ForceMode.Impulse);

        public void SetRagdoll(bool active)
        {
            Character.Animator.transform.SetParent(active ? null : transform);
            
            if (!active)
            {
                Character.Animator.transform.localPosition = _animatorPos;
                Character.Animator.transform.localRotation = _animatorRot;
                
                if (IsAI)
                {
                    var currentPos = Character.transform.position;
                    currentPos.y = 3.4f;
                    Character.transform.position = currentPos;
                }
            }
            
            if (IsAI)
                GetComponent<NavMeshAgent>().enabled = !active;
            
            Character.Rigidbody.useGravity = !active;
            Character.Collider.enabled = !active;
            
            
            Character.Animator.enabled = !active;
            
            var colliders = Character.Animator.GetComponentsInChildren<Collider>();
            
            foreach (var collider in colliders)
                collider.enabled = active;
        }
        
        public void CollidersClose()
        {
            var m_collider = GetComponent<Collider>();
            var colliders = GetComponentsInChildren<Collider>();
            
            foreach (var collider in colliders)
            {
                if(collider != m_collider)
                    collider.enabled = !collider.enabled;
                collider.Save();
            }
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(Ragdoll))]
    public class RagdollEditor : Editor
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