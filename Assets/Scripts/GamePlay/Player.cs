using Core;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// Functions, controls related to the character controlled by the player.
    /// </summary>
    public class Player : Character
    {
        [SerializeField] private float speed;

        private void Update()
        {
            // Check if the character is not dead, the game is playing, and there is a valid JoystickController instance
            if (!IsDead && GameManager.IsPlay && JoystickController.Instance != null)
                Move(JoystickController.Instance.Input);
        }

        public void Move(Vector2 input)
        {
            // Create the movement vector
            var movement = new Vector3(input.y, 0, -input.x);
            movement = transform.TransformDirection(movement);

            // Update the animator parameters for horizontal and vertical movement
            Animator.SetFloat("Horizontal", input.x);
            Animator.SetFloat("Vertical", input.y);

            // Apply the movement to the character's position
            transform.position += movement * speed * Time.deltaTime;
        }

        public override void Die()
        {
            base.Die();
            // Update the death UI element
            UIManager.Instance.UpdateDeath();
        }
    }
}