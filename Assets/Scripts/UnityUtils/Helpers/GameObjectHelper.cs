using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityUtils.Helpers
{
    public class GameObjectHelper
    {
        /// <summary>
        /// Create a little sphere without collider. 
        /// </summary>
        /// <param name="position"> Dot position </param>
        /// <param name="color"> Dot Color </param>
        /// <returns></returns>
        public static GameObject CreateDot(Vector3 position, Color color)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            sphere.transform.position = position;
            sphere.GetComponent<MeshRenderer>().material.color = color;
            if (sphere.TryGetComponent(out Collider collider))
                Object.Destroy(collider);
            Object.Destroy(sphere, 20);
            return sphere;
        }
        
        public static T FindObjectWithTypeAndName<T>(string name) where T : Object
        {
            var objs = Object.FindObjectsOfType<T>();
            foreach (var obj in objs)
                if (obj.name == name)
                    return obj;
            throw new Exception();
        }
    }
}