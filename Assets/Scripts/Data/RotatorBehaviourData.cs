using System;
using GamePlay;
using GamePlay.Trigger;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class RotatorBehaviourData
    {
        [HideInInspector] public Enemy enemy;
        public ObstacleTrigger[] triggers;
        public Transform rotator;

        public float RotatorAngle => rotator.rotation.eulerAngles.y;
    }
}