using Core;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils.BaseClasses;
using UnityUtils.Extensions;

namespace UI
{
    public class DoTweenCoinAnimator : SingletonBehavior<DoTweenCoinAnimator>
    {
        private readonly Vector2 _coinEndPos = new (-50, -50);

        public void RewardCoins(Vector3 canvasPos)
        {
            var coinRects = GetCoinRects();

            Debug.Log(coinRects.Length);
            var delay = 0f;
            foreach (var coinRect in coinRects)
            {
                coinRect.position = canvasPos;
                coinRect.SetActivate(true);
                
                coinRect.DOScale(Vector3.one, 0.2f).SetDelay(delay).SetEase(Ease.OutBack);
                coinRect.DOAnchorPos(_coinEndPos, 0.5f).SetDelay(delay + 0.5f);
                coinRect.DOScale(Vector3.zero, 0.4f).SetDelay(delay + 0.85f).SetEase(Ease.OutBack).
                    OnComplete(() => { EventHandler.DispatchCoinUpdate(); });
                delay += 0.15f;
            }
        }

        private static RectTransform[] GetCoinRects()
        {
            var coins = PoolManager.GetPoolObjects(PoolType.UICoin, 5);
            var coinRects = new RectTransform[coins.Length];

            for (var i = 0; i < coins.Length; i++)
            {
                var currentCoin = coins[i].GameObject();
                coinRects[i] = currentCoin.GetComponent<RectTransform>();
            }
            
            return coinRects;
        }
    }
}