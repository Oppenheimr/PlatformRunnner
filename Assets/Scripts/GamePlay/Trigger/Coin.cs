using System;
using System.Collections;
using Core;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils.Extensions;
using EventHandler = Core.EventHandler;

namespace GamePlay.Trigger
{
    /// <summary>
    /// Designed to add collider feature for coins.
    /// </summary>
    public class Coin : TriggerParticle
    {
        private MeshRenderer _renderer; // Reference to the MeshRenderer component
        // Property to get the MeshRenderer component, caching it if available
        public MeshRenderer Renderer => _renderer ? _renderer : (_renderer = GetComponentInChildren<MeshRenderer>()); 

        private Collider _collider; // Reference to the Collider component
        // Property to get the Collider component, caching it if available
        public Collider Collider => _collider ? _collider : (_collider = GetComponentInChildren<Collider>()); 

        protected override void OnTriggerEnter(Collider other)
        {
            // Check if the trigger is activated by an object with the "Player" tag
            if (!other.CompareTag("Player") && !other.CompareTag("Enemy") ) 
                return;
            
            Renderer.enabled = false; // Disable the MeshRenderer to hide the coin
            Collider.enabled = false; // Disable the Collider to prevent further collisions
            SpawnParticle(PoolType.CoinParticle, transform); // Spawn a particle effect for the collected coin
            
            if (other.CompareTag("Player"))
                DoTweenCoinAnimator.Instance.RewardCoins(
                CameraController.Instance.Camera.WorldToScreenPoint(transform.position));
        }
    }
}