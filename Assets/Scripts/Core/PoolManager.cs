using System;
using System.Linq;
using UnityEngine;
using UnityUtils.BaseClasses;
using UnityUtils.Extensions;
using Object = UnityEngine.Object;

namespace Core
{
    /// <summary>
    /// Manages objects that need "ObjectPooling" design in the game.
    /// </summary>
    public class PoolManager : SingletonBehavior<PoolManager>
    {
        //Serialized field for the pools array to be visible in the Inspector
        [SerializeField] private Pool[] pools;

        private void Awake()
        {
            // Disable all objects in the pool at the start
            foreach (var pool in pools)
            foreach (var poolObject in pool.poolObjects)
                poolObject.SetActivate(false);
        }

        public Object GetParticle(PoolType type)
        {
            for (int i = 0; i < pools.Length; i++)
            {
                if (pools[i].type != type)
                    continue; // Move to the next pool if the pool's type doesn't match the requested type

                pools[i].poolObjects = pools[i].poolObjects.TakeOrdinaryAddToEnd(0); // Add a new empty object to the pool's object array
                var particle = pools[i].poolObjects[0]; // Get the first object
                particle.SetActivate(true); // Activate the object
                return particle; // Return the object
            }

            throw new Exception("Type not available");
        }
    }

    [Serializable]
    public struct Pool
    {
        public Object[] poolObjects;
        public PoolType type;
    }

    public enum PoolType
    {
        CoinParticle,
        WinParticle,
        HitParticle,
    }
}