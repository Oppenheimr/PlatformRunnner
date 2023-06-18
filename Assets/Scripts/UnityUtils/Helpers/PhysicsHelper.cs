using UnityEngine;
using UnityUtils.Extensions;

namespace UnityUtils.Helpers
{
    public static class PhysicsHelper
    {
        /// <summary>
        /// Physics Raycast with ignore tag
        /// </summary>
        /// <param name="start"> Line start </param>
        /// <param name="end"> Line end </param>
        /// <param name="hitInfo"> Line hit info </param>
        /// <param name="layerMask"> Hittable layer/layers </param>
        /// <param name="ignoreTags"> Unhitched tags </param>
        /// <returns></returns>
        public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask, params string[] ignoreTags)
        {
            //Linecast at ve bir seye isabet ederse devam et, etmez ise false don.
            if (!Physics.Linecast(start, end, out hitInfo, layerMask))
                return false;
            
            //Eger hit collider tag == ignore tag ise tekrar ray at.
            foreach (var ignoreTag in ignoreTags)
                if (hitInfo.collider.CompareTag(ignoreTag))
                {
                    var newOrigin = PointOneUnitAheadOfTheLine(start, hitInfo.point);
                    return Linecast(newOrigin, end, out hitInfo, layerMask, ignoreTags);
                }
            //Line isabet aldi ve ignore tag icermiyor true don.
            return true;
        }
        
        /// <summary>
        /// Physics Raycast with ignore tag
        /// </summary>
        /// <param name="origin"> Line origin </param>
        /// <param name="direction"> Line direction </param>
        /// <param name="hitInfo"> Line hit info </param>
        /// <param name="maxDistance"> Line distance limit </param>
        /// <param name="layerMask"> Hittable layer/layers </param>
        /// <param name="ignoreTags"> Unhitched tags </param>
        /// <returns></returns>
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance,
            int layerMask, params string[] ignoreTags)
        {
            //Raycast at ve bir seye isabet ederse devam et, etmez ise false don.
            if (!Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask))
                return false;
            
            //Eger hit collider tag == ignore tag ise tekrar ray at.
            foreach (var ignoreTag in ignoreTags)
                if (hitInfo.collider.CompareTag(ignoreTag))
                {
                    var newDistance = maxDistance - origin.Distance(hitInfo.point);
                    var newOrigin = PointOneUnitAheadOfTheLine(origin, hitInfo.point);
                    return Raycast(newOrigin, direction, out hitInfo, newDistance, layerMask, ignoreTags);
                }
            //Ray isabet aldi ve ignore tag icermiyor true don.
            return true;
        }

        /// <summary>
        /// Physics Raycast with ignore tag
        /// </summary>
        /// <param name="origin"> Line origin </param>
        /// <param name="direction"> Line direction </param>
        /// <param name="hitInfo"> Line hit info </param>
        /// <param name="maxDistance"> Line distance limit </param>
        /// <param name="layerMask"> Hittable layer/layers </param>
        /// <param name="queryTriggerInteraction"></param>
        /// <param name="ignoreTags"> Unhitched tags </param>
        /// <returns></returns>
        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance,
            int layerMask, QueryTriggerInteraction queryTriggerInteraction, params string[] ignoreTags)
        {
            //Raycast at ve bir seye isabet ederse devam et, etmez ise false don.
            if (!Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask,
                    queryTriggerInteraction))
                return false;
            
            //Eger hit collider tag == ignore tag ise tekrar ray at.
            foreach (var ignoreTag in ignoreTags)
                if (hitInfo.collider.CompareTag(ignoreTag))
                {
                    var newDistance = maxDistance - origin.Distance(hitInfo.point);
                    var newOrigin = PointOneUnitAheadOfTheLine(origin, hitInfo.point);

                    //For Test  
                    // Debug.DrawRay(origin, direction, Color.red, 10);
                    // Debug.DrawRay(newOrigin, direction, Color.green, 10);
                    // ObjectUtils.CreateDot(hitInfo.point, Color.red);
                    // ObjectUtils.CreateDot(newOrigin, Color.green);

                    //return false;
                    return Raycast(newOrigin, direction, out hitInfo, newDistance, layerMask, ignoreTags);
                }
            //Ray isabet aldi ve ignore tag icermiyor true don.
            return true;
        }

        
        
        public static Vector3 PointOneUnitAheadOfTheLine(Vector3 pointA, Vector3 pointB)
        {
            //              A'           B'  C'
            //  -------------*------------*---*--------> 
            
            // Find the direction vector of the line passing through points A and B
            var direction = (pointB - pointA).normalized;

            // Find the point that is one unit away from point B in the direction of the line
            var pointC = pointB + (direction/1000);

            return pointC;
        }
    }
}