using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay.Trigger
{
    /// <summary>
    /// Designed to show new target point for AI.
    /// </summary>
    public class PlatformTriggerForAI : MonoBehaviour
    {
        [SerializeField] private Transform enemyMoveTarget; // The target transform for enemy movement

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) // Check if the collider's tag is "Enemy"
                return;

            if (!other.TryGetComponentInParent(out Enemy enemy)) // Try to get the Enemy component from the parent object of the collider
                return;

            enemy.target = enemyMoveTarget; // Set the target transform for enemy movement to the specified enemyMoveTarget
        }
    }
}