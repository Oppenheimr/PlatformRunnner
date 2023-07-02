using System;
using GamePlay;
using GamePlay.Trigger;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class ShiningBehaviourData
    {
        [HideInInspector] public Enemy enemy;
        public ObstacleTrigger[] triggers;
    }
}