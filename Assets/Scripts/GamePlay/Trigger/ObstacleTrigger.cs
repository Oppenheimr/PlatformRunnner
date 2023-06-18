using Core;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay.Trigger
{
    [RequireComponent(typeof(Collider))]
    public class ObstacleTrigger : MonoBehaviour
    {
        [SerializeField] private float bounceForce = 10;

        private void OnTriggerEnter(Collider other) => OnEnter(other.transform);

        private void OnCollisionEnter(Collision other) => OnEnter(other.transform);

        protected virtual void OnEnter(Transform other)
        {
            if (!other.CompareTag("Player") && !other.CompareTag("Enemy"))
                return;
            
            if(!other.TryGetComponentInParent(out Character character))
                return;
            
            Vector3 direction = transform.position.ToDirection(other.position);
            
            character.Ragdoll.SetRagdoll(true);
            character.Ragdoll.AddForceRagdolled(direction * bounceForce);
            
            character.Die();
            character.Respawn(2.5f);
            
            SoundManager.Instance.PlayDie();
        }
    }
}