using System.Linq;
using GamePlay;
using GamePlay.Trigger;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace Core
{
    /// <summary>
    /// It is the most general manager who looks over the whole game and controls the game.
    /// </summary>
    public class GameManager : SingletonBehavior<GameManager>
    {
        public Player player; // Reference to the player object
        public static bool IsPlay; // Flag indicating if the game is currently in play
        public int currentCoin; // Current number of coins collected
        public int deathCount; // Total number of deaths

        [SerializeField] private FinishTrigger finishTrigger; // Reference to the finish trigger object
        [SerializeField] private Enemy[] enemy; // Array of enemy objects
        private Character[] _characters; // Array of characters (including player and enemies)

        public static Transform PlayerTransform =>
            Instance.player.transform; // Static property returning the player's transform

        public static Vector3 PlayerPos =>
            Instance.player.transform.position; // Static property returning the player's position

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Portrait; // Set the screen orientation to portrait mode

            IsPlay = false; // Initialize the IsPlay flag as false
            _characters =
                new Character[enemy.Length + 1]; // Create a new array for characters with a length of enemy.Length + 1
            for (var i = 0; i < enemy.Length; i++)
                _characters[i] = enemy[i]; // Copy enemy objects to the characters array
            _characters[enemy.Length] = player; // Assign the player object as the last element of the characters array
        }

        private void LateUpdate()
        {
            if (!IsPlay)
                return; // If the game is not in play, exit the method

            _characters =
                _characters.OrderByDescending(x => x.transform.position.x)
                    .ToArray(); // Sort the characters array based on their x-position in descending order

            for (var i = 0; i < _characters.Length; i++)
            {
                if (_characters[i] != player) continue; // Skip if the current character is not the player
                UIManager.Instance
                    .UpdateScore(i +
                                 1); // Update the score UI based on the player's position in the sorted characters array
                break; // Exit the loop
            }
        }

        public void OnComplete()
        {
            for (var i = 0; i < 4; i++)
                finishTrigger.SpawnWinParticle(); // Spawn win particles 4 times

            UIManager.Instance.OnComplete(); // Trigger the completion UI event
        }
    }
}