using System.Collections.Generic;
using UnityEngine;
using UnityUtils.Helpers;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Uc boyutlu duzlemde yapılacak her turlu aksyon ve hesaplamalara yardım etmesi icin gelistiriliyor
    /// </summary>
    public static class Vector3Extensions
    {
        public static Vector3 AddToXAxis(this Vector3 pos, float add)
        {
            pos.x += add;
            return pos;
        }
        
        public static Vector3 AddToYAxis(this Vector3 pos, float add)
        {
            pos.y += add;
            return pos;
        }
        
        public static Vector3 AddToZAxis(this Vector3 pos, float add)
        {
            pos.z += add;
            return pos;
        }
        
        public static Vector3 InvertYAxis(this Vector3 target)
        {
            target.y = -target.y;
            return target;
        }
        
        public static float Distance(this Vector3 point, Vector3 target)
            => Vector3.Distance(point,target);
        
        public static Vector3 MostNearestPoint(this List<Vector3> points, Vector3 target)
        {
            var nearestPoint = Vector3.zero;
            
            foreach (var point in points)
            {
                if (nearestPoint == Vector3.zero)
                    nearestPoint = point;
                else
                {
                    if (point.Distance(target) < nearestPoint.Distance(target))
                        nearestPoint = point;
                }
            }
            return nearestPoint;
        }
        
        public static Vector3 ToDirection(this Vector3 thisTransform, Vector3 target)
            => (target - thisTransform).normalized;

        public static bool Linecast(this Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask, params string[] ignoreTags)
            => PhysicsHelper.Linecast(start, end, out hitInfo, layerMask, ignoreTags);
        
        public static bool Raycast(this Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance,
            int layerMask, params string[] ignoreTags)
            => PhysicsHelper.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, ignoreTags);
        
        public static bool Raycast(this Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance,
            int layerMask, QueryTriggerInteraction queryTriggerInteraction, params string[] ignoreTags)
            => PhysicsHelper.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction, ignoreTags);
        
        public static Vector3 AlignToFloor(this Vector3 position)
        {
            Ray ray = new Ray(position, Vector3.down);
            return Physics.Raycast(ray, out var r, 100) ? r.point : position;
        }
        
        public static Vector3 AlignToFloor(this Vector3 position, int layer)
        {
            Ray ray = new Ray(position, Vector3.down);
            Debug.DrawRay(position, Vector3.down, Color.magenta, 5,true);
            return Physics.Raycast(ray, out var r, 1000, layer) ? r.point : position;
        }
    }
}