using Core;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay.Trigger
{
    public class RotatorTrigger : ObstacleTrigger
    {
        private Rotator _rotator;
        private Rotator Rotator => _rotator ? _rotator : (_rotator = GetComponentInParent<Rotator>());
        
        protected override void OnEnter(Transform other)
        {
            if (!other.CompareTag("Player") &&
                !other.CompareTag("Enemy")) // Check if the other object has the "Player" or "Enemy" tag
                return;

            // Try to get the Character component from the parent object of the other transform
            if (!other.TryGetComponentInParent(out Character character))
                return;
            
            character.AddForce(CalculateDirection(other.position) * bounceForce);
            SoundManager.Instance.PlayHit(); // Play the die sound effect
        }

        private Vector3 Direction(Transform other) => Rotator.transform.rotation.eulerAngles.y switch
        {
            < 90 => other.right,
            < 180 => - other.forward,
            < 270 => - other.right,
            _ => other.forward,
            
            //< 90 => transform.position.ToDirection(other.position).normalized + other.right,
            //< 180 => transform.position.ToDirection(other.position).normalized - other.forward,
            //< 270 => transform.position.ToDirection(other.position).normalized - other.right,
            //_ => transform.position.ToDirection(other.position).normalized + other.forward,
        };
    }
}