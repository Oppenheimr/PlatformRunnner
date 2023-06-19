using UnityEngine;
using UnityUtils.BaseClasses;

namespace Core
{
    /// <summary>
    /// Manages all the sounds in the game.
    /// </summary>
    public class SoundManager : SingletonBehavior<SoundManager>
    {
        [SerializeField] private AudioSource background; // Reference to the background audio source
        [SerializeField] private AudioSource soundFX; // Reference to the sound effects audio source
        [SerializeField] private AudioClip die; // Audio clip for the "die" sound effect
        [SerializeField] private AudioClip hit; // Audio clip for the "hit" sound effect
        [SerializeField] private AudioClip win; // Audio clip for the "win" sound effect

        public void PlayDie() => Play(die); // Plays the "die" sound effect
        public void PlayHit() => Play(hit); // Plays the "hit" sound effect

        public void PlayWin()
        {
            Play(win); // Plays the "win" sound effect
            background.volume = 0.2f; // Sets the volume of the background audio source to 0.2f
        }

        private void Play(AudioClip clip)
        {
            if (!GameManager.IsPlay)
                return; // If the game is not in play, exit the method

            soundFX.clip = clip; // Assign the provided audio clip to the sound effects audio source
            soundFX.Play(); // Play the sound effect
        }
    }
}