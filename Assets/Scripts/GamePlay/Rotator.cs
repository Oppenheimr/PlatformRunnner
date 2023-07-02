using System;
using GamePlay.Trigger;
using UnityEngine;

namespace GamePlay
{
    public class Rotator : MonoBehaviour        
    {
        [SerializeField] private float rotationSpeed = 10f;

        private Rigidbody _rigidBody;
        private Rigidbody Rigidbody => _rigidBody ? _rigidBody : (_rigidBody = GetComponent<Rigidbody>());
        
        private Quaternion targetRotation;
        private Vector3 _initialPosition;
        
        private void Start()
        {
            // Rigidbody bileşenini al
            targetRotation = Rigidbody.rotation;
            _initialPosition = transform.position;
        }
        
        //private void FixedUpdate() => transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        private void FixedUpdate()
        {
            
            // Hedef dönüş rotasyonunu güncelle
            targetRotation *= Quaternion.Euler(Vector3.up * rotationSpeed * Time.deltaTime);

            // Rigidbody'yi hedef rotasyona dön
            Rigidbody.MoveRotation(targetRotation);
        }

        private void LateUpdate() => transform.position = _initialPosition;
    }
}