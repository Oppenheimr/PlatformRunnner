using System;
using Core;
using UnityEngine;
using EventHandler = Events.EventHandler;

namespace GamePlay
{
    public class Player : Character
    {
        [SerializeField] private float speed;

        private void Update()
        {
            if(!IsDead && GameManager.IsPlay && JoystickController.Instance != null)
                Move(JoystickController.Instance.Input);
        }

        public void Move(Vector2 input)
        {
            // Create the movement vector
            var movement = new Vector3(input.y, 0, -input.x);
            movement = transform.TransformDirection(movement);

            Animator.SetFloat("Horizontal", input.x);
            Animator.SetFloat("Vertical", input.y);

            // Apply the movement to the character's position
            transform.position += movement * speed * Time.deltaTime;
        }
        
        public override void Die()
        {
            base.Die();
            UIManager.Instance.UpdateDeath();
        }
    }
}