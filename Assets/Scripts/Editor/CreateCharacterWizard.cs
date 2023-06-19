using GamePlay;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils.Extensions;

namespace Editor
{
    /// <summary>
    /// A tool to help create a new character.
    /// </summary>
    public class CreateCharacterWizard : ScriptableWizard
    {
        public GameObject target; // Reference to the target GameObject
        public bool isPlayer; // Flag indicating whether the character is a player or an enemy

        [MenuItem("Tools/Create Character")]
        public static void CreateWizard()
        {
            var wizard =
                DisplayWizard<CreateCharacterWizard>("Character Creator"); // Display the character creation wizard

            var selectionObjects = Selection.objects;
            if (selectionObjects is not { Length: 1 }) return; // Exit if there is no or more than one object selected

            var firstSelection = selectionObjects[0] as GameObject;
            if (firstSelection != null)
                wizard.target = firstSelection; // Assign the selected GameObject as the target in the wizard
        }

        void OnWizardCreate()
        {
            if (!target.TryGetComponent(out Animator animator))
            {
                if (!target.TryGetComponentInChildren(out animator))
                    animator = target
                        .AddComponent<Animator>(); // Add an Animator component to the target if it doesn't exist
            }

            var newGameObject = new GameObject { name = "New Character" }; // Create a new GameObject for the character
            target.transform.SetParent(newGameObject.transform); // Set the target as a child of the new GameObject

            if (isPlayer)
            {
                newGameObject.AddComponent<Player>(); // Add Player component to the new GameObject
                newGameObject.AddComponent<CapsuleCollider>(); // Add CapsuleCollider component to the new GameObject
                newGameObject.AddComponent<Rigidbody>(); // Add Rigidbody component to the new GameObject
                newGameObject.AddComponent<Ragdoll>(); // Add Ragdoll component to the new GameObject
            }
            else
            {
                newGameObject.AddComponent<Enemy>(); // Add Enemy component to the new GameObject
                newGameObject.AddComponent<CapsuleCollider>(); // Add CapsuleCollider component to the new GameObject
                newGameObject.AddComponent<Rigidbody>(); // Add Rigidbody component to the new GameObject
                newGameObject.AddComponent<Ragdoll>(); // Add Ragdoll component to the new GameObject
                newGameObject.AddComponent<NavMeshAgent>(); // Add NavMeshAgent component to the new GameObject
            }

            animator.transform.localPosition = Vector3.zero; // Reset the local position of the animator
            animator.transform.localRotation = Quaternion.identity; // Reset the local rotation of the animator

            AutoRagdoller.Build(animator); // Build the ragdoll based on the animator
        }
    }
}