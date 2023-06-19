using System;
using System.Collections;
using Core;
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
        [SerializeField] private int coinValue = 5; // Value of the coin when collected

        private MeshRenderer _renderer; // Reference to the MeshRenderer component
        public MeshRenderer Renderer => _renderer ? _renderer : (_renderer = GetComponentInChildren<MeshRenderer>()); // Property to get the MeshRenderer component, caching it if available

        private Collider _collider; // Reference to the Collider component
        public Collider Collider => _collider ? _collider : (_collider = GetComponentInChildren<Collider>()); // Property to get the Collider component, caching it if available

        protected override void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) // Check if the trigger is activated by an object with the "Player" tag
                return;

            EventHandler.DispatchCoinUpdate(coinValue); // Dispatch an event to update the coin count with the coinValue
            Renderer.enabled = false; // Disable the MeshRenderer to hide the coin
            Collider.enabled = false; // Disable the Collider to prevent further collisions
            SpawnParticle(PoolType.CoinParticle, transform); // Spawn a particle effect for the collected coin
        }
    }
}