using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityUtils.Extensions;
using Object = UnityEngine.Object;

namespace GamePlay.Trigger
{
    public class FinishTrigger : MonoBehaviour
    {
        [SerializeField] private Transform characterPoint;
        
        private void OnTriggerEnter(Collider other)
        {
            if(!other.TryGetComponentInParent(out Character character))
                return;

            character.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            SoundManager.Instance.PlayWin();
            
            
            if (other.CompareTag("Player"))
            {
                GameManager.IsPlay = false;
                UIManager.Instance.OnFinish();
                ((Player)character).Move(new Vector2(0,0));
                other.transform.Teleport(characterPoint);
            }
            else if (other.CompareTag("Enemy"))
            {

                character.Rigidbody.AddForce(Vector3.up * 450, ForceMode.Impulse);
                character.Rigidbody.AddForce(Vector3.forward * 150, ForceMode.Impulse);
                StartCoroutine(SetActiveWaiter(other));
            }
            
            
        }

        private IEnumerator SetActiveWaiter(Object target)
        {
            yield return new WaitForSeconds(3);
            target.SetActivate(false);
        }
    }
}