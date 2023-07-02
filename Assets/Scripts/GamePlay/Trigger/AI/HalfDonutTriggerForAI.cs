using Data;
using GamePlay.BotBehaviour;
using UnityEngine;

namespace GamePlay.Trigger.AI
{
    public class HalfDonutTriggerForAI : PlatformTriggerForAI
    {
        [SerializeField] private HalfDonutBehaviourData behaviourData;

        private HalfDonutBehaviour SetupBehaviour(Enemy enemy)
        {
            behaviourData.enemy = enemy;
            return new HalfDonutBehaviour(behaviourData);
        }
        
        protected override void OnTriggerEnemy(Enemy enemy) => 
            enemy.BaseBehaviour = SetupBehaviour(enemy);
    }
}