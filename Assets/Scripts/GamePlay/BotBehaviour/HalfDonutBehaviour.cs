using Data;
using UnityEngine;

namespace GamePlay.BotBehaviour
{
    public class HalfDonutBehaviour : BaseBehaviour
    {
        private readonly HalfDonutBehaviourData _data;
        
        private Vector3 StepPosition
        {
            get
            {
                var position = Enemy.transform.position;
                position.x += 1.2f;
                position.z = 9;
                return GetNearestNavigationMeshPoint(position);
            }
        }
        
        private Vector3 BackAndSafe
        {
            get
            {
                var position = BackStepPosition;
                position.z = 9;
                return GetNearestNavigationMeshPoint(position, -2);
            }
        }
        
        public HalfDonutBehaviour(HalfDonutBehaviourData data)
        {
            Enemy = data.enemy;
            _data = data;
        }

        protected override void StartBehaviour() => Enemy.Agent.destination = StepPosition;

        public override void UpdateBehaviour()
        {
            base.UpdateBehaviour();
            
            //Wait Triggers
            WaitObjectsAndSetDestination(_data.triggers, 5, BackAndSafe);
            
            if (IsWaiting)
                return;

            Enemy.Agent.destination = StepPosition;
        }
    }
}