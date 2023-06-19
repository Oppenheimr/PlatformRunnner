using System;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// By creating a conveyor belt-like mechanism, it gives movement to the physics objects on it.
    /// </summary>
    public class RotatingPlatform : MonoBehaviour
    {
        [SerializeField] private float speed = 5.8f;
        [SerializeField] private Transform dragDirection;

        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody ? _rigidbody : (_rigidbody = GetComponent<Rigidbody>());

        // Perform physics-related actions
        private void FixedUpdate()
        {
            Vector3 pos = Rigidbody.position;
    
            // Move the rigidbody in the opposite direction of the dragDirection transform's forward axis
            Rigidbody.position -= dragDirection.forward * Time.fixedDeltaTime * speed;
    
            // Move the rigidbody to the stored position to avoid interference with other physics calculations
            _rigidbody.MovePosition(pos);
        }

    }
}