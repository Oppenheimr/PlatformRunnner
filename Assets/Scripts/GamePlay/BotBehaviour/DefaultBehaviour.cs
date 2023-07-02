namespace GamePlay.BotBehaviour
{
    public class DefaultBehaviour : BaseBehaviour
    {
        public DefaultBehaviour(Enemy enemy) => Enemy = enemy;
        

        protected override void StartBehaviour() => Enemy.Agent.destination = NextStepPosition;

        public override void UpdateBehaviour()
        {
            base.UpdateBehaviour();
            Enemy.Agent.destination = NextStepPosition;
        }
    }
}