using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils.Extensions;
using Object = UnityEngine.Object;

namespace GamePlay.BotBehaviour
{
    public abstract class BaseBehaviour
    {
        protected Enemy Enemy;
        protected bool IsWaiting;
        private bool _start;

        protected Vector3 CenterNextStepPosition
        {
            get
            {
                var position = Enemy.transform.position;
                position.z = 0;
                position.x += 1.2f;
                return GetNearestNavigationMeshPoint(position);
            }
        }
        
        protected Vector3 NextStepPosition
        {
            get
            {
                var position = Enemy.transform.position;
                position.x += 2f;
                return GetNearestNavigationMeshPoint(position);
            }
        }

        protected Vector3 BackStepPosition
        {
            get
            {
                var position = Enemy.transform.position;
                position.x -= 1;
                return GetNearestNavigationMeshPoint(position, -2);
            }
        }
        
        protected Vector3 GetNearestNavigationMeshPoint(Vector3 position, int positionAdd = 2)
        {
            NavMeshHit hit;
            for (int i = 0; i < 5; i++)
            {
                if (NavMesh.SamplePosition(position, out hit, 0.001f, NavMesh.AllAreas))
                    return position;
                position.x += positionAdd;
            }

            return NavMesh.SamplePosition(position, out hit, 30, NavMesh.AllAreas) 
                ? position : hit.position;
        }

        private bool OtherIsClose(Component other, float distance) =>
            other.transform.position.Distance(Enemy.transform.position) < distance;
        
        private bool OtherIsAhead(Component other) =>
            other.transform.position.x > Enemy.transform.position.x;
        
        protected abstract void StartBehaviour();

        public virtual void UpdateBehaviour()
        {
            WaitObjects(GameManager.Instance.characters, 1.7f, WaitAndStepBack);
            
            if (_start)
                return;
            
            StartBehaviour();
            _start = true;
        }

        protected void WaitObjects(Component[] objects, float distance, Action<Func<bool>> action)
        {
            foreach (var obj in objects)
            {
                if (obj == Enemy)
                    continue;

                if (!OtherIsClose(obj, distance)) 
                    continue;

                if (!OtherIsAhead(obj))
                    continue;

                action(() => OtherIsClose(obj, distance));
                break;
            }
        }
        
        protected void WaitObjectsAndSetDestination(Component[] objects, float distance, Vector3 destination)
        {
            foreach (var obj in objects)
            {
                if (obj == Enemy)
                    continue;

                if (!OtherIsClose(obj, distance)) 
                    continue;

                if (!OtherIsAhead(obj))
                    continue;

                WaitAndSetDestination(() => OtherIsClose(obj, distance), destination);
                break;
            }
        }
        


        /// <summary>
        /// condition is true : Wait ...
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="action"></param>
        protected void WaitAndAction(Func<bool> condition, Action action)
        {
            if (IsWaiting)
                return;
            
            IsWaiting = true;
            var dest = Enemy.Agent.destination;
            
            Enemy.StartCoroutine(StepWaiter());
            IEnumerator StepWaiter()
            {
                while (condition())
                {
                    action();
                    yield return new WaitForSeconds(1f);
                }
                
                IsWaiting = false;
                Move(dest);
            }
        }

        /// <summary>
        /// condition is true : Wait ...
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="destination"></param>
        protected void WaitAndSetDestination(Func<bool> condition, Vector3 destination) => 
            WaitAndAction(condition, () => Move(destination));
        
        /// <summary>
        /// condition is true : Wait ...
        /// </summary>
        /// <param name="condition"></param>
        protected void WaitAndStepBack(Func<bool> condition) => WaitAndSetDestination(condition, BackStepPosition);
        
        
        /// <summary>
        /// condition is true : Wait ...
        /// </summary>
        /// <param name="condition"></param>
        protected void WaitAndStop(Func<bool> condition) => WaitAndAction(condition, Stop);
        
        protected void Stop() => Enemy.Agent.destination = Enemy.transform.position;
        
        protected void Move(Vector3 destination) => Enemy.Agent.destination = destination;
    }
}