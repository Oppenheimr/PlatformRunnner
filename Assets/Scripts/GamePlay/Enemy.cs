using System;
using System.Collections;
using Core;
using GamePlay.BotBehaviour;
using GamePlay.Trigger;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils.Extensions;
using Random = UnityEngine.Random;

namespace GamePlay
{
    /// <summary>
    /// Deals with the controls and behavior of enemies.
    /// </summary>
    public class Enemy : Character
    {
        [SerializeField] private float minSpeed = 12; // The minimum speed of the enemy
        [SerializeField] private float maxSpeed = 17; // The maximum speed of the enemy
        
        private bool _isMove; // Flag indicating if the enemy is currently moving
        public BaseBehaviour BaseBehaviour;

        // Reference to the NavMeshAgent component
        private NavMeshAgent _agent;
        public NavMeshAgent Agent => _agent ? _agent : (_agent = GetComponentInChildren<NavMeshAgent>());

        protected override void Awake()
        {
            Agent.updateRotation = false;
            // Set a random speed for the enemy within the specified range
            Agent.speed = Random.Range(minSpeed, maxSpeed);

            BaseBehaviour = new DefaultBehaviour(this);
            base.Awake();
        }

        private void Update()
        {
            if (!GameManager.IsPlay || IsDead) // Check if the game is not in play mode or the enemy is dead
                return;
            
            // Create the movement vector
            var movement = transform.position.ToDirection(Agent.destination).normalized;
            movement.y = 0;

            // Update the animator parameters for horizontal and vertical movement
            Animator.SetFloat("Horizontal", -movement.z);
            Animator.SetFloat("Vertical", movement.x);
            Animator.SetFloat("Speed", movement.magnitude);

            BaseBehaviour?.UpdateBehaviour();
        }

        public override void AddForce(Vector3 direction)
        {
            StartCoroutine(AddForceWaiter());
            IEnumerator AddForceWaiter()
            {
                Agent.isStopped = true;
                Rigidbody.AddForce(direction * 2, ForceMode.Impulse); 
                yield return new WaitForSeconds(1);
                Rigidbody.velocity = Vector3.zero;
                Agent.isStopped = false;
            }
        }
        
        public override void Respawn(float delay)
        {
            Rigidbody.useGravity = false;
            BaseBehaviour = new DefaultBehaviour(this);
            base.Respawn(delay);
        }
    }
}