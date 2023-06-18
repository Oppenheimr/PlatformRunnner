using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay.Trigger
{
    public class PlatformTriggerForAI : MonoBehaviour
    {
        [SerializeField] private Transform enemyMoveTarget;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy"))
                return;
            
            if(!other.TryGetComponentInParent(out Enemy enemy))
                return;

            enemy.target = enemyMoveTarget;
        }
    }
}