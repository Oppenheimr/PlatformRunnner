using System;
using UnityEngine;

namespace GamePlay
{
    //Conveyor Belt
    public class RotatingPlatform : MonoBehaviour
    {
        [SerializeField] private float speed = 5.8f;
        [SerializeField] private Transform dragDirection;
        
        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>());

        //Physic actions
        private void FixedUpdate()
        {
            Vector3 pos = Rigidbody.position;
            Rigidbody.position -= dragDirection.forward * Time.fixedDeltaTime * speed;
            _rigidbody.MovePosition(pos);
        }
    }
}