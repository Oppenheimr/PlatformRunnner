using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay.Trigger
{
    /// <summary>
    /// Designed to add collider feature for obstacles.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class ObstacleTrigger : MonoBehaviour
    {
        [SerializeField] protected float bounceForce = 10; // Amount of force applied to the character when bouncing off
        
        private void OnTriggerEnter(Collider other) =>
            OnEnter(other.transform); // Triggered when another collider enters this trigger
        private void OnTriggerStay(Collider other) =>
            OnEnter(other.transform); // Triggered when another collider enters this trigger
        
        
        private void OnCollisionEnter(Collision other) =>
            OnEnter(other.transform); // Triggered when another collider collides with this object
        private void OnCollisionStay(Collision other) =>
            OnEnter(other.transform); // Triggered when another collider collides with this object
        
        protected virtual void OnEnter(Transform other)
        {
            if (!other.CompareTag("Player") &&
                !other.CompareTag("Enemy")) // Check if the other object has the "Player" or "Enemy" tag
                return;

            // Try to get the Character component from the parent object of the other transform
            if (!other.TryGetComponentInParent(out Character character)) 
                return;
            
            // Activate the ragdoll mode for the character
            character.Ragdoll.SetRagdoll(true); 
            // Apply a force to the character in the calculated direction
            character.Ragdoll.AddForceRagdolled(CalculateDirection(other.position) * bounceForce);
            
            character.Die(); // Kill the character
            character.Respawn(2.5f); // Respawn the character after a delay of 2.5 seconds

            SoundManager.Instance.PlayDie(); // Play the die sound effect
        }

        protected Vector3 CalculateDirection(Vector3 other)
        {
            Vector3 thisPosition = transform.position;
            thisPosition.y = other.y;
            // Calculate the direction from this object to the other object
            return thisPosition.ToDirection(other);   
        }
    }
}