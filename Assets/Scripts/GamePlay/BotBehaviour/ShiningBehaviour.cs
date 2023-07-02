using Data;
using UnityEngine;

namespace GamePlay.BotBehaviour
{
    public class ShiningBehaviour : BaseBehaviour
    {
        private readonly ShiningBehaviourData _data;
        
        public ShiningBehaviour(ShiningBehaviourData data)
        {
            Enemy = data.enemy;
            _data = data;
        }

        protected override void StartBehaviour() => Enemy.Agent.destination = NextStepPosition;

        public override void UpdateBehaviour()
        {
            base.UpdateBehaviour();
            
            //Wait Triggers
            WaitObjectsAndSetDestination(_data.triggers, 6, BackStepPosition);
            
            if (IsWaiting)
                return;

            Enemy.Agent.destination = NextStepPosition;
        }
    }
}