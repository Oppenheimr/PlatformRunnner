using Data;
using GamePlay.BotBehaviour;
using UnityEngine;

namespace GamePlay.Trigger.AI
{
    public class RotatorTriggerForAI : PlatformTriggerForAI
    {
        [SerializeField] private RotatorBehaviourData behaviourData;

        private RotatorBehaviour SetupBehaviour(Enemy enemy)
        {
            behaviourData.enemy = enemy;
            return new RotatorBehaviour(behaviourData);
        }
        
        protected override void OnTriggerEnemy(Enemy enemy) => 
            enemy.BaseBehaviour = SetupBehaviour(enemy);
    }
}