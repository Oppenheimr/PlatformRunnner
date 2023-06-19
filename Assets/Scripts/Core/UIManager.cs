using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;
using UnityUtils.Extensions;
using EventHandler = Core.EventHandler;

namespace Core
{
    /// <summary>
    /// Manages the interface in the game.
    /// </summary>
    public class UIManager : SingletonBehavior<UIManager>
    {
        [SerializeField] private GameObject startButtonRoot; // Reference to the start button game object
        [SerializeField] private GameObject mainUIRoot; // Reference to the main UI game object
        [SerializeField] private GameObject paintUIRoot; // Reference to the paint UI game object
        [SerializeField] private GameObject completeUIRoot; // Reference to the complete UI game object

        [SerializeField] private DoTweenText coinText; // Reference to the coin text component for tweening

        [SerializeField]
        private DoTweenText playerPosition; // Reference to the player position text component for tweening

        [SerializeField] private DoTweenText deathCount; // Reference to the death count text component for tweening
        [SerializeField] private DoTweenText paintCount; // Reference to the paint count text component for tweening

        private void OnEnable() => EventHandler.OnCoinUpdate += OnCoinUpdate; // Subscribe to the OnCoinUpdate event

        private void OnDisable() =>
            EventHandler.OnCoinUpdate -= OnCoinUpdate; // Unsubscribe from the OnCoinUpdate event

        private void OnCoinUpdate(int coin)
        {
            GameManager.Instance.currentCoin += coin; // Update the current coin count in the GameManager
            coinText.SetText(GameManager.Instance.currentCoin
                .ToString()); // Update the coin text with the updated coin count
        }

        public void UpdateScore(int score) =>
            playerPosition.SetText($"Position #{score}"); // Update the player position text with the provided score

        public void UpdateDeath()
        {
            GameManager.Instance.deathCount++; // Increment the death count in the GameManager
            deathCount.SetText(GameManager.Instance.deathCount
                .ToString()); // Update the death count text with the updated death count
        }

        public void UpdatePaintPercent(int paintPercent) =>
            paintCount.SetText(
                $"Painted : %{paintPercent}"); // Update the paint count text with the provided paint percentage

        public void OnClickStart()
        {
            GameManager.IsPlay = true; // Set the IsPlay flag in the GameManager to true
            startButtonRoot.SetActivate(false); // Deactivate the start button game object
        }

        public void OnClickDownload() =>
            Application.OpenURL("https://www.panteon.games/"); // Open the specified URL in the default web browser

        public void OnFinish()
        {
            mainUIRoot.SetActivate(false); // Deactivate the main UI game object
            paintUIRoot.SetActivate(true); // Activate the paint UI game object
            completeUIRoot.SetActivate(false); // Deactivate the complete UI game object
        }

        public void OnComplete()
        {
            mainUIRoot.SetActivate(false); // Deactivate the main UI game object
            paintUIRoot.SetActivate(false); // Deactivate the paint UI game object
            completeUIRoot.SetActivate(true); // Activate the complete UI game object
        }
    }
}