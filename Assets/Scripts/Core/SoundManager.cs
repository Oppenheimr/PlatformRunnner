using UnityEngine;
using UnityUtils.BaseClasses;

namespace Core
{
    public class SoundManager : SingletonBehavior<SoundManager>
    {
        [SerializeField] private AudioSource background;
        [SerializeField] private AudioSource soundFX;
        [SerializeField] private AudioClip die;
        [SerializeField] private AudioClip hit;
        [SerializeField] private AudioClip win;

        public void PlayDie() => Play(die);
        public void PlayHit() => Play(hit);

        public void PlayWin()
        {
            Play(win);
            background.volume = 0.2f;
        }

        private void Play(AudioClip clip)
        {
            if (!GameManager.IsPlay)
                return;
            
            soundFX.clip = clip;
            soundFX.Play();
        }
    }
}