using GamePlay.BotBehaviour;

namespace GamePlay.Trigger.AI
{
    public class DefaultPlatformTriggerAI : PlatformTriggerForAI
    {
        protected override void OnTriggerEnemy(Enemy enemy)
        {
            enemy.BaseBehaviour = new DefaultBehaviour(enemy);
            enemy.Agent.speed *= 1.2f;
        }
    }
}