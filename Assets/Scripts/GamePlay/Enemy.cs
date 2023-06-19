using System;
using Core;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GamePlay
{
    /// <summary>
    /// Deals with the controls and behavior of enemies.
    /// </summary>
    public class Enemy : Character
    {
        public Transform target; // The target transform that the enemy will move towards

        [SerializeField] private float minSpeed = 12; // The minimum speed of the enemy
        [SerializeField] private float maxSpeed = 17; // The maximum speed of the enemy

        private bool _isMove; // Flag indicating if the enemy is currently moving
        private int _isMoveHash; // Hash ID of the "isMove" parameter in the Animator controller

        private NavMeshAgent _agent;
        public NavMeshAgent Agent => _agent ? _agent : (_agent = GetComponentInChildren<NavMeshAgent>()); // Reference to the NavMeshAgent component

        protected override void Awake()
        {
            Agent.speed = Random.Range(minSpeed, maxSpeed); // Set a random speed for the enemy within the specified range
            _isMoveHash = Animator.StringToHash("isMove"); // Get the hash ID of the "isMove" parameter
            base.Awake();
        }

        private void Update()
        {
            if (!GameManager.IsPlay || IsDead) // Check if the game is not in play mode or the enemy is dead
                return;

            Agent.destination = target.position; // Set the destination of the NavMeshAgent to the target position

            var move = Agent.acceleration > 0.1f; // Check if the enemy is currently moving based on its acceleration
            if (_isMove == move)
                return;

            _isMove = move;
            Animator.SetBool(_isMoveHash, _isMove); // Set the "isMove" parameter in the Animator to control the movement animation
        }

    }
}