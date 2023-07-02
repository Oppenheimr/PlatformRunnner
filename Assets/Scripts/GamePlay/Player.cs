using System.Collections;
using Core;
using UI;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// Functions, controls related to the character controlled by the player.
    /// </summary>
    public class Player : Character
    {
        [HideInInspector] public bool moveWithPhysics;
        [SerializeField] private float speed;
        private bool _controllerIsActive = true;

        private void Update()
        {
            // Check if the character is not dead, the game is playing, and there is a valid JoystickController instance
            if (!IsDead && GameManager.IsPlay && JoystickController.Instance != null && _controllerIsActive)
                Move(JoystickController.Instance.Input);

            if (Input.GetKeyDown(KeyCode.K))
            {
                Time.timeScale = Time.timeScale == 1 ? 0.2f : 1;
            }
        }

        public void Move(Vector2 input)
        {
            // Create the movement vector
            var movement = new Vector3(input.y, 0, -input.x);
            movement = transform.TransformDirection(movement);

            // Update the animator parameters for horizontal and vertical movement
            Animator.SetFloat("Horizontal", input.x);
            Animator.SetFloat("Vertical", input.y);
            Animator.SetFloat("Speed", input.magnitude);

            // Apply the movement to the character's position
            if (moveWithPhysics)
                Rigidbody.velocity = movement * speed;
            else
                transform.position += movement * speed * Time.deltaTime;
        }

        public override void Die()
        {
            base.Die();
            // Update the death UI element
            UIManager.Instance.UpdateDeath();
        }
        
        public override void AddForce(Vector3 direction)
        {
            StartCoroutine(AddForceWaiter());
            IEnumerator AddForceWaiter()
            {
                _controllerIsActive = false;
                Rigidbody.AddForce(direction * 2, ForceMode.Impulse); 
                yield return new WaitForSeconds(.75f);
                Rigidbody.velocity = Vector3.zero;
                _controllerIsActive = true;
            }
        }
    }
}