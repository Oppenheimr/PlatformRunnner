using GamePlay;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils.Extensions;

namespace Editor
{
    public class CreateCharacterWizard : ScriptableWizard
    {
        public GameObject target;
        public bool isPlayer;
        
        [MenuItem("Tools/Create Character")]
        public static void CreateWizard()
        {
            var wizard = DisplayWizard<CreateCharacterWizard>("Character Creator");

            var selectionObjects = Selection.objects;
            if (selectionObjects is not { Length: 1 }) return;

            var firstSelection = selectionObjects[0] as GameObject;
            if (firstSelection != null)
                wizard.target = firstSelection;
        }

        void OnWizardCreate()
        {
            if (!target.TryGetComponent(out Animator animator))
                if (!target.TryGetComponentInChildren(out animator))
                    animator = target.AddComponent<Animator>();

            var newGameObject = new GameObject { name = "New Character" };
            target.transform.SetParent(newGameObject.transform);

            if (isPlayer)
            {
                newGameObject.AddComponent<Player>();
                newGameObject.AddComponent<CapsuleCollider>();
                newGameObject.AddComponent<Rigidbody>();
                newGameObject.AddComponent<Ragdoll>();
            }
            else
            {
                newGameObject.AddComponent<Enemy>();
                newGameObject.AddComponent<CapsuleCollider>();
                newGameObject.AddComponent<Rigidbody>();
                newGameObject.AddComponent<Ragdoll>();
                newGameObject.AddComponent<NavMeshAgent>();
            }
            animator.transform.localPosition = Vector3.zero;
            animator.transform.localRotation = Quaternion.identity;
            AutoRagdoller.Build(animator);
        }
    }
}