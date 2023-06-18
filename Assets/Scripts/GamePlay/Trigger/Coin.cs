using System;
using Core;
using UnityEngine;
using UnityUtils.Extensions;
using EventHandler = Events.EventHandler;

namespace GamePlay.Trigger
{
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int coinValue = 5;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            EventHandler.DispatchCoinUpdate(coinValue);
            gameObject.SetActivate();
        }
    }
}