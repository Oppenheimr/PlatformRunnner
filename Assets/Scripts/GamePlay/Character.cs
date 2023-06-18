using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay
{
    public class Character : MonoBehaviour
    {
        protected bool IsDead;
        
        private Vector3 _awakePos;
        private Quaternion _awakeRot;
        
        private Animator _animator;
        public Animator Animator => _animator ? _animator : (_animator =  GetComponentInChildren<Animator>());
        
        private Ragdoll _ragdoll;
        public Ragdoll Ragdoll => _ragdoll ? _ragdoll : (_ragdoll = GetComponentInChildren<Ragdoll>());
        
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody ? _rigidbody : (_rigidbody = GetComponentInChildren<Rigidbody>());

        private Collider _collider;
        public Collider Collider => _collider ? _collider : (_collider = GetComponentInChildren<Collider>());
        
        protected virtual void Awake()
        {
            _awakePos = transform.position;
            _awakeRot = transform.rotation;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.CompareTag("Player") && !other.transform.CompareTag("Enemy"))
                return;
            
            if(!other.transform.TryGetComponentInParent(out Character character))
                return;
            
            Vector3 direction = transform.position.ToDirection(other.transform.position);
            character.Ragdoll.AddForce(direction * 5);
            
            Ragdoll.AddForce(other.transform.position.ToDirection(transform.position) * 5);
            
            SoundManager.Instance.PlayHit();
        }

        public virtual void Die() => IsDead = true;
        

        public void Respawn(float delay)
        {
            StartCoroutine(RespawnWaiter());
            IEnumerator RespawnWaiter()
            {
                yield return new WaitForSeconds(delay);
                transform.position = _awakePos;
                transform.rotation = _awakeRot;
            
                Ragdoll.SetRagdoll(false);
                IsDead = false;
            }
        }
    }
}