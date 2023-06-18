using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils.Extensions;
using Random = UnityEngine.Random;

namespace UnityUtils.BaseClasses
{
    public class BaseMonoBehaviour : MonoBehaviour
    {
        protected bool TryGetComponentsInChildren<T>(out List<T> comp) => gameObject.TryGetComponentsInChildren(out comp); 
    
        private RectTransform _rectTransform;
        public RectTransform RectTransform => _rectTransform ??= GetComponent<RectTransform>(); 
        

        /// <summary>
        /// Nesne içerisindeki tüm alt nesnelerin layerlarini degistirir.
        /// </summary>
        /// <param name="transform">Silinecek transform</param>
        public void SetLayerAllChildren(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.gameObject.layer = layer;
                if(child.childCount > 0) SetLayerAllChildren(child, layer);
            }
        }

        public List<Transform> GetAllChildren(Transform targetTransform)
        {
            
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < targetTransform.childCount; i++)
            {
                var child = targetTransform.GetChild(i);
                children.Add(child);
                if (child.childCount > 0)
                {
                    List<Transform> childrenFromChildren = GetAllChildren(child);
                    children.AddRange(childrenFromChildren);
                }
            }
            return children;
        }
    
        public List<Transform> GetAllChildren(MonoBehaviour target)
        {
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < target.transform.childCount; i++)
            {
                var child = target.transform.GetChild(i);
                children.Add(child);
                if (child.childCount > 0)
                {
                    List<Transform> childrenFromChildren = GetAllChildren(child);
                    children.AddRange(childrenFromChildren);
                }
            }
            return children;
        }
    
        /// <summary>
        /// Nesne içerisindeki tüm alt nesneleri siler.
        /// </summary>
        /// <param name="obj">Silinecek obje</param>
        public void DestroyChildren(MonoBehaviour obj) => DestroyChildren(obj.transform);
    


        /// <summary>
        /// Nesne içerisindeki tüm alt nesneleri siler.
        /// </summary>
        /// <param name="transform">Silinecek transform</param>
        public void DestroyChildren(Transform transform)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                if (!child.IsDestroyed())
                {
                    Destroy(child.gameObject);
                }
            }
        }



        /// <summary>
        /// Bu fonksyon MonoBehaviour Classından gelen SetActive fonksyonundan farkli olarak objeleri CanvasRenderer ile yavasca kaybedebilme yetenegindedir.
        /// olusturulan animasyon her zaman 25 fps de calisir.
        /// </summary>
        /// <param name="active"></param>
        /// <param name="fadeTime">Saniye cinsinden, From second</param>
        /// <param name="renderer"></param>
        /// <param name="setActivate"></param>
        public void RendererFade(CanvasRenderer renderer, bool active, float fadeTime, bool setActivate = false)
        {
            //obje aktive edilecek ve deaktifse aktive edilsin...
            if (setActivate && active && !renderer.gameObject.activeSelf) renderer.gameObject.SetActive(true);
            if (gameObject.activeSelf) StartCoroutine(CustomSetActive());

            IEnumerator CustomSetActive()
            {
                //adim sayisi
                float increaseAmount = 0.04f / fadeTime;

                if (active)
                {
                    for (float i = 0; i < 1; i += increaseAmount)
                    {
                        renderer.SetAlpha(i);
                        yield return new WaitForSeconds(0.04f);
                    }
                }
                else
                {
                    increaseAmount *= -1;
                    for (float i = 1; i > 0; i += increaseAmount)
                    {
                        renderer.SetAlpha(i);
                        yield return new WaitForSeconds(0.04f);
                    }
                }
            }

            //obje deaktive edilecek ve aktifse deaktive edilsin
            if (setActivate && !active && renderer.gameObject.activeSelf) renderer.gameObject.SetActive(false);
        }

        /// <summary>
        /// Bu fonksyon MonoBehaviour Classından gelen SetActive fonksyonundan farkli olarak objeleri CanvasRenderer ile yavasca kaybedebilme yetenegindedir.
        /// olusturulan animasyon her zaman 25 fps de calisir.
        /// </summary>
        /// <param name="active"></param>
        /// <param name="fadeTime">Saniye cinsinden, From second</param>
        /// <param name="renderer"></param>
        /// <param name="setActivate"></param>
        public void RendererFade(CanvasGroup renderer, bool active, float fadeTime, bool setActivate = false)
        {
            //obje aktive edilecek ve deaktifse aktive edilsin...
            if (setActivate && active && !renderer.gameObject.activeSelf) renderer.gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(CustomSetActive());

            IEnumerator CustomSetActive()
            {
                //yield return new WaitForSeconds(0.04f);
                //adim sayisi
                float increaseAmount = 0.04f / fadeTime;

                if (active)
                {
                    renderer.alpha = 0;
                    for (float i = 0; i < 1; i += increaseAmount)
                    {
                        renderer.alpha = i;
                        yield return new WaitForSeconds(0.04f);
                    }

                    renderer.alpha = 1; //buraya girmiyor
                }
                else
                {
                    renderer.alpha = 1;
                    increaseAmount *= -1;
                    for (float i = 1; i > 0; i += increaseAmount)
                    {
                        renderer.alpha = i;
                        yield return new WaitForSeconds(0.04f);
                    }

                    renderer.alpha = 0;
                }
            }

            //obje deaktive edilecek ve aktifse deaktive edilsin
            if (setActivate && !active && renderer.gameObject.activeSelf) renderer.gameObject.SetActive(false);
        }

        /// <summary>
        /// Canvas Renderer iceren bir objenin (UI için) alfa degerini animasyonla degistirir.
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="targetOpacity"></param>
        public void RendererChange(CanvasRenderer renderer, float targetOpacity)
        {
            StartCoroutine(CustomSetActive());

            IEnumerator CustomSetActive()
            {
                float increaseAmount = 0.04f;
                if (targetOpacity < renderer.GetAlpha()) increaseAmount *= -1;

                for (float i = 0; Math.Abs(targetOpacity - renderer.GetAlpha()) > 0.1f; i += increaseAmount)
                {
                    renderer.SetAlpha(i);
                    yield return new WaitForSeconds(0.04f);
                }
            }
        }

        public T SelectRandom<T>(IList<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        public int NumberOfDigit(float number)
        {
            int digit = 0;
            while (number > 1)
            {
                number /= 10;
                digit++;
            }

            return digit;
        }

        public Vector3? AlignToFloor(Vector3 position)
        {
            RaycastHit r; //Atilacak cizginin tanimi
            Ray ray = new Ray(position, Vector3.down); //Cizginin baslangici ve yönünün tanimi
            if (Physics.Raycast(ray, out r, 100)) // Cizginin atilmasi
            {
                return r.point; //Carptigi yer
            }

            return null;
        }

        /// <summary>
        /// Bu fonksiyon gonderilen listede ki boollarin tum hepsi true ise true doner 
        /// </summary>
        /// <returns></returns>
        protected bool ListIsTrue(List<bool> boolList)
        {
            for (int i = 0; i < boolList.Count; i++)
                if (!boolList[i])
                    return false;
            return true;
        }

        protected Color SetAlpha(Color color, float alpha)
        {
            return new Color() { r = color.r, g = color.g, b = color.b, a = alpha };
        }

        public void SetActive(bool active)
        {
            SetActive(this.gameObject, active);
        }

        public void SetActive(GameObject gameObject, bool active)
        {
            if (gameObject == null) return;
            if (gameObject.activeSelf != active)
                gameObject.SetActive(active);
        }
        public void SetActive(List<GameObject> gameObjects, bool active)
        {
            foreach (var gameObject in gameObjects.Where(gameObject => gameObject.activeSelf != active))
                gameObject.SetActive(active);
        }
    }
}