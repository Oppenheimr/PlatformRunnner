using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay
{
    /// <summary>
    /// Is the base class of characters such as enemy and player.
    /// </summary>
    public class Character : MonoBehaviour
    {
        protected bool IsDead; // Flag indicating if the character is dead

        private Vector3 _awakePos; // The initial awake position of the character
        private Quaternion _awakeRot; // The initial awake rotation of the character

        #region Properties

        private Animator _animator;
        public Animator Animator => _animator ? _animator : (_animator = GetComponentInChildren<Animator>());

        private Ragdoll _ragdoll;
        public Ragdoll Ragdoll => _ragdoll ? _ragdoll : (_ragdoll = GetComponentInChildren<Ragdoll>());

        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody ? _rigidbody : (_rigidbody = GetComponentInChildren<Rigidbody>());

        private Collider _collider;
        public Collider Collider => _collider ? _collider : (_collider = GetComponentInChildren<Collider>());

        #endregion

        protected virtual void Awake()
        {
            _awakePos = transform.position; // Store the initial position of the character
            _awakeRot = transform.rotation; // Store the initial rotation of the character
        }

        private void OnCollisionEnter(Collision other)
        {
            // Check if the collided object has the "Player" or "Enemy" tag
            if (!other.transform.CompareTag("Player") && !other.transform.CompareTag("Enemy"))
                return;

            // Try to get the Character component from the parent object of the collided object
            if (!other.transform.TryGetComponentInParent(out Character character))
                return;

            // Calculate the direction from this character to the collided object
            Vector3 direction = transform.position.ToDirection(other.transform.position);

            character.Ragdoll.AddForce(direction * 5); // Apply a force to the collided character
            Ragdoll.AddForce(other.transform.position.ToDirection(transform.position) *
                             5); // Apply a force to this character

            SoundManager.Instance.PlayHit(); // Play the hit sound effect
        }

        public virtual void Die() => IsDead = true; // Method to mark the character as dead

        public void Respawn(float delay)
        {
            StartCoroutine(RespawnWaiter());

            IEnumerator RespawnWaiter()
            {
                yield return new WaitForSeconds(delay); // Wait for the specified delay
                transform.position = _awakePos; // Reset the position of the character
                transform.rotation = _awakeRot; // Reset the rotation of the character

                Ragdoll.SetRagdoll(false); // Deactivate the ragdoll mode
                IsDead = false; // Mark the character as alive
            }
        }
    }
}