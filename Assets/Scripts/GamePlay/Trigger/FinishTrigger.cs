using System.Collections;
using Core;
using UnityEngine;
using UnityUtils.Extensions;
using Object = UnityEngine.Object;

namespace GamePlay.Trigger
{
    /// <summary>
    /// Designed to add collider feature for finish line.
    /// </summary>
    public class FinishTrigger : TriggerParticle
    {
        [SerializeField] private Transform characterPoint; // Reference to the character point transform

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponentInParent(out Character character))
                return; // Exit if the collider's parent object doesn't have a Character component

            SpawnWinParticle(); // Spawn the win particle effect
            character.Rigidbody.constraints = RigidbodyConstraints.FreezeAll; // Freeze the character's rigidbody movement
            SoundManager.Instance.PlayWin(); // Play the win sound effect

            if (other.CompareTag("Player"))
            {
                GameManager.IsPlay = false; // Set the IsPlay flag in the GameManager to false
                UIManager.Instance.OnFinish(); // Trigger the finish UI
                ((Player)character).Move(new Vector2(0, 0)); // Stop the player's movement
                other.transform.Teleport(characterPoint); // Teleport the player to the character point
            }
            else if (other.CompareTag("Enemy"))
            {
                character.Rigidbody.AddForce(Vector3.up * 450, ForceMode.Impulse); // Apply an upward impulse force to the enemy character
                character.Rigidbody.AddForce(Vector3.forward * 150, ForceMode.Impulse); // Apply a forward impulse force to the enemy character
                StartCoroutine(SetActiveWaiter(other)); // Start a coroutine to deactivate the enemy character after a delay
            }
        }

        public void SpawnWinParticle() => SpawnParticle(PoolType.WinParticle, characterPoint); // Spawn the win particle effect at the character point

        private IEnumerator SetActiveWaiter(Object target)
        {
            yield return new WaitForSeconds(3); // Wait for 3 seconds
            target.SetActivate(false); // Deactivate the target object
        }
    }
}