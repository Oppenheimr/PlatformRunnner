using GamePlay.BotBehaviour;
using UnityEngine;

namespace GamePlay.Trigger.AI
{
    public class StaticTriggerForAI : PlatformTriggerForAI
    {
        private Vector3 MovePosition
        {
            get
            {
                var position = transform.position;
                position.x += 80;   
                return position;
            }
        }
        
        protected override void OnTriggerEnemy(Enemy enemy) => 
            enemy.BaseBehaviour = new StaticObstacleBehaviour(enemy, MovePosition);
        
    }
}