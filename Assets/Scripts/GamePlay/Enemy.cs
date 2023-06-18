using System;
using Core;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GamePlay
{
    public class Enemy : Character
    {
        public Transform target;
        [SerializeField] private float minSpeed = 12;
        [SerializeField] private float maxSpeed = 17;
        
        
        private bool _isMove;
        private int _isMoveHash;
        
        private NavMeshAgent _agent;
        public NavMeshAgent Agent => _agent ? _agent : (_agent =  GetComponentInChildren<NavMeshAgent>());

        protected override void Awake()
        {
            Agent.speed = Random.Range(minSpeed, maxSpeed);
            _isMoveHash = Animator.StringToHash("isMove");
            base.Awake();
        }

        private void Update()
        {
            if (!GameManager.IsPlay || IsDead)
                return;
            
            Agent.destination = target.position;
            var move = Agent.acceleration > 0.1f;
            if (_isMove == move) return;
            _isMove = move;
            Animator.SetBool(_isMoveHash, _isMove);
        }
    }
}