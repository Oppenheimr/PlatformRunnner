using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using UnityEngine;
using UnityUtils.BaseClasses;

namespace Core
{
    public class GameManager : SingletonBehavior<GameManager>
    {
        public Player player;
        public static bool IsPlay;
        public int currentCoin;
        public int deathCount;

        [SerializeField] private Enemy[] enemy;
        private Character[] _characters;
        
        public static Transform PlayerTransform => Instance.player.transform;
        public static Vector3 PlayerPos => Instance.player.transform.position;

        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Portrait;
            
            IsPlay = false;
            _characters = new Character[enemy.Length + 1];
            for (var i = 0; i < enemy.Length; i++)
                _characters[i] = enemy[i];
            _characters[enemy.Length] = player;
        }

        private void LateUpdate()
        {
            if (!IsPlay)
                return;
            
            _characters = _characters.OrderByDescending(x => x.transform.position.x).ToArray();

            for (var i = 0; i < _characters.Length; i++)
            {
                if (_characters[i] != player) continue;
                UIManager.Instance.UpdateScore(i + 1);
                break;
            }
        }
    }
}