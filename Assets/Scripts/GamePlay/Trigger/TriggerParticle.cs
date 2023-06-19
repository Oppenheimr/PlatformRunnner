using System.Collections;
using Core;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay.Trigger
{
    /// <summary>
    /// Designed to be the base class for trigger scripts to work with particles.
    /// </summary>
    public abstract class TriggerParticle : MonoBehaviour
    {
        protected abstract void OnTriggerEnter(Collider other);

        protected void SpawnParticle(PoolType type, Transform teleport) => 
            StartCoroutine(ParticleWaiter(type, teleport));

        // Coroutine to handle the waiting and deactivation of particles
        private IEnumerator ParticleWaiter(PoolType type, Transform teleport)
        {
            // Get a particle from the object pool
            var particle = PoolManager.Instance.GetParticle(type);
            // Set the particle's position to the teleport position
            particle.GameObject().transform.position = teleport.position;
            // Wait for 5 seconds
            yield return new WaitForSeconds(5);
            // Deactivate the particle
            particle.SetActivate(false);
        }

    }
}