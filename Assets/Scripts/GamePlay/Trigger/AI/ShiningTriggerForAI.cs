using Data;
using GamePlay.BotBehaviour;
using UnityEngine;

namespace GamePlay.Trigger.AI
{
    public class ShiningTriggerForAI : PlatformTriggerForAI
    {
        [SerializeField] private ShiningBehaviourData behaviourData;

        private ShiningBehaviour SetupBehaviour(Enemy enemy)
        {
            behaviourData.enemy = enemy;
            return new ShiningBehaviour(behaviourData);
        }
        
        protected override void OnTriggerEnemy(Enemy enemy) => 
            enemy.BaseBehaviour = SetupBehaviour(enemy);
    }
}