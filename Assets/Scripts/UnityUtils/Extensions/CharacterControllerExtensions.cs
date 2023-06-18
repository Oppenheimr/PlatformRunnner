using System.Collections;
using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class CharacterControllerExtensions
    {
        public static void AddForce(this CharacterController controller, Vector3 dir, float force)
        {
            dir.Normalize(); //dir vectorunu birim vectore donustururur
            if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
            var impact = dir.normalized * force / 2;
            var gravityImpact = new Vector3(0, -0.98f, 0);
            float gravityTime = 0;
            int detectionActivateCounter = 0;

            var mb = new MonoBehaviour();

            mb.StopCoroutine(AddForceCounter());
            mb.StartCoroutine(AddForceCounter());

            IEnumerator AddForceCounter()
            {
                bool loop = true;

                while (loop)
                {
                    if (impact.magnitude > 0.1f)
                        controller.Move(impact * Time.deltaTime); //horizontal movement
                    controller.Move(gravityImpact * (gravityTime) / 5); //gravity

                    // consumes the impact energy each cycle:
                    impact = Vector3.Lerp(impact, Vector3.zero,
                        5 * Time.deltaTime); //yatay hareketin buyuklugunu zamanla azaltmak icin
                    gravityTime += Time.deltaTime; // Klasik fizik kanunlarina gore serberst dusus hızı yani ; V = g . t
                    yield return
                        new WaitForSeconds(1f /
                                           60); //saniyenin 60ta biri bekletilerek hareketi 60fps de yapmasini saglar

                    if (detectionActivateCounter >
                        20) //ilk 20 dongude kontrol etmemek icin, cunki hareketin baslangicinda yerde olabiliriz ve muhtemelen oyleyiz...
                    {
                        if (controller.isGrounded || controller.detectCollisions)
                            loop = false; //Karakter yere değiyor mu ve bir yere carpti mi ?
                    }
                    else detectionActivateCounter++;
                }
                Object.Destroy(mb, 1);
            }
        }
    }
}