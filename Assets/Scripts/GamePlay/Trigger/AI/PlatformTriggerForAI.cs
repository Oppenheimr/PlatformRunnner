using Data;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay.Trigger.AI
{
    /// <summary>
    /// Designed to show new target point for AI.
    /// </summary>
    public abstract class PlatformTriggerForAI : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) // Check if the collider's tag is "Enemy"
                return;

            if (!other.TryGetComponentInParent(out Enemy enemy)) // Try to get the Enemy component from the parent object of the collider
                return;
            
            OnTriggerEnemy(enemy);
            
            
        }

        protected abstract void OnTriggerEnemy(Enemy enemy);
    }
}