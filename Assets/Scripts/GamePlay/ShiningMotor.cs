using System.Collections;
using UnityEngine;
using UnityUtils.Extensions;

namespace GamePlay
{
    public class ShiningMotor : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 250;
        
        private void Start() => StartCoroutine(LerpMoveToMotor());
        
        private IEnumerator LerpMoveToMotor()
        {
            var position = transform.position;
            Vector3 positionA = position.AddToZAxis(-8);
            Vector3 positionB = position.AddToZAxis(8);
            
            while (true)
            {
                float timer = 0;
                while (timer < 2)
                {
                    var progress = timer / 2;
                    progress = SmoothProgress(progress);
                    transform.position = Vector3.Lerp(positionA, positionB, progress);
                    timer += Time.deltaTime;
                    
                    transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }

                (positionA, positionB) = (positionB, positionA);
            }
        }

        public float SmoothProgress(float progress)
        {
            //Maps the progress between -Pi/2 to Pi/2
            progress = Mathf.Lerp(-Mathf.PI/2, Mathf.PI/2, progress);
            //Returns a value between -1 and 1
            progress = Mathf.Sin(progress);
            //Scale the sine value between 0 and 1
            progress = (progress / 2f) + 0.5f;
            return progress;
        }
    }
}