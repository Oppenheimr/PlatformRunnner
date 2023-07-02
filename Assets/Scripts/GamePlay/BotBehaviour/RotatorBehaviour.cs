using Data;
using UnityEngine;

namespace GamePlay.BotBehaviour
{
    public class RotatorBehaviour : BaseBehaviour
    {
        private readonly RotatorBehaviourData _data;

        private Vector3 LeftStep
        {
            get
            {
                var position = Enemy.transform.position;
                position.z = 9;
                return GetNearestNavigationMeshPoint(position);
            }
        }
        
        private Vector3 LeftForwardStep
        {
            get
            {
                var position = Enemy.transform.position;
                position.x += 2f;
                position.z = 9;
                return GetNearestNavigationMeshPoint(position);
            }
        }
        
        private Vector3 RightForwardStep
        {
            get
            {
                var position = Enemy.transform.position;
                position.x += 2f;
                position.z = -9;
                return GetNearestNavigationMeshPoint(position);
            }
        }
        
        public RotatorBehaviour(RotatorBehaviourData data)
        {
            Enemy = data.enemy;
            _data = data;
        }

        protected override void StartBehaviour() => Enemy.Agent.destination = NextStepPosition;

        public override void UpdateBehaviour()
        {
            base.UpdateBehaviour();
            
            //Wait Triggers
            WaitObjectsAndSetDestination(_data.triggers, 8, BackStepPosition);

            if (IsWaiting)
                return;

            
            if (_data.rotator.position.x + 4 < Enemy.transform.position.x)
            {
                Enemy.Agent.destination = CenterNextStepPosition;
            }
            //Z is positive or negative
            else if (Enemy.transform.position.z > 0)
            {
                Enemy.Agent.destination = _data.RotatorAngle switch
                {
                    //Go forward left side
                    > 20 and < 250 => LeftForwardStep,
                    //Just Wait
                    _ => Enemy.transform.position
                };
            }
            else
            {
                
                Enemy.Agent.destination = _data.RotatorAngle switch
                {
                    //Go left side
                    > 45 and < 135 => LeftStep,
                    //Just Wait
                    > 135 and < 270 => Enemy.transform.position,
                    //Go forward right side
                    _ => RightForwardStep
                };
            }
        }
    }
}