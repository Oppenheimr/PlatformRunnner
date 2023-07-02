using UnityEngine;

namespace GamePlay.BotBehaviour
{
    public class StaticObstacleBehaviour : BaseBehaviour
    {
        private readonly Vector3 _enemyMoveTarget; // The target transform for enemy movement

        public StaticObstacleBehaviour(Enemy enemy, Vector3 target)
        {
            Enemy = enemy;
            _enemyMoveTarget = target;
        }

        protected sealed override void StartBehaviour()
        {
            // Set the target transform for enemy movement to the specified _enemyMoveTarget
            var pos = Enemy.transform.position;
            pos.x = _enemyMoveTarget.x;
            Enemy.Agent.destination = pos;
        }
    }
}