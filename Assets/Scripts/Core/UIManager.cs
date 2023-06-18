using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.BaseClasses;
using UnityUtils.Extensions;
using EventHandler = Events.EventHandler;

namespace Core
{
    public class UIManager : SingletonBehavior<UIManager>
    {
        [SerializeField] private GameObject startButtonRoot;
        [SerializeField] private GameObject mainUIRoot;
        [SerializeField] private GameObject paintUIRoot;
        [SerializeField] private GameObject completeUIRoot;
        
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI playerPosition;
        [SerializeField] private TextMeshProUGUI deathCount;
        [SerializeField] private TextMeshProUGUI paintCount;

        private void OnEnable() => EventHandler.OnCoinUpdate += OnCoinUpdate;
        private void OnDisable() => EventHandler.OnCoinUpdate -= OnCoinUpdate;

        private void OnCoinUpdate(int coin)
        {
            GameManager.Instance.currentCoin += coin;
            coinText.text = GameManager.Instance.currentCoin.ToString();
        }

        public void UpdateScore(int score) => playerPosition.text = "Position #" + score;
        
        public void UpdateDeath()
        {
            GameManager.Instance.deathCount++;
            deathCount.text = GameManager.Instance.deathCount.ToString();
        }
        
        public void UpdatePaintPercent(int paintPercent) => paintCount.text = $"Painted : %{paintPercent}";

        public void OnClickStart()
        {
            GameManager.IsPlay = true;
            startButtonRoot.SetActivate(false);
        }

        public void OnClickDownload() => Application.OpenURL("https://www.panteon.games/");

        public void OnFinish()
        {
            mainUIRoot.SetActivate(false);
            paintUIRoot.SetActivate(true);
            completeUIRoot.SetActivate(false);
        }

        public void OnComplete()
        {
            mainUIRoot.SetActivate(false);
            paintUIRoot.SetActivate(false);
            completeUIRoot.SetActivate(true);
        }
    }
}