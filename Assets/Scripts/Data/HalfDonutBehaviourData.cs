using System;
using GamePlay;
using GamePlay.Trigger;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class HalfDonutBehaviourData
    {
        [HideInInspector] public Enemy enemy;
        public ObstacleTrigger[] triggers;
    }
}