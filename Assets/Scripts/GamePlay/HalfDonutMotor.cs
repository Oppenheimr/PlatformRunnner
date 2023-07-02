using System;
using System.Collections;
using UnityEngine;

namespace GamePlay
{
    public class HalfDonutMotor : MonoBehaviour
    {
        [SerializeField] private float speed = 10;

        private Rigidbody _rigidbody;
        public Rigidbody Rigidbody => _rigidbody ? _rigidbody : (_rigidbody = GetComponentInChildren<Rigidbody>());
        
        private void Awake() => StartCoroutine(Motor());

        private IEnumerator Motor()
        {
            while (true)
            {
                if (transform.position.x is > 0.35f or < -0.15f)
                    Rigidbody.velocity = Vector3.zero;
                
                if (Math.Abs(transform.position.x - 0.35f) < 0.01f || Math.Abs(transform.position.x - (-0.15f)) < 0.01f)
                    Rigidbody.velocity = Vector3.zero;

                yield return RightForce();
                yield return LeftForce();

                yield return new WaitForEndOfFrame();
            }
        }


        private IEnumerator LeftForce()
        {
            while (transform.localPosition.x > -0.05f)
            {
                Rigidbody.AddForce(-transform.right * speed, ForceMode.Force);
                yield return new WaitForEndOfFrame();
            }
        }
        
        private IEnumerator RightForce()
        {
            while (transform.localPosition.x < 0.12f)
            {
                Rigidbody.AddForce(transform.right * speed, ForceMode.Force);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}