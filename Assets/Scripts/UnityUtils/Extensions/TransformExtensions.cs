using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.Extensions
{
    public static class TransformExtensions
    {
        public static void Teleport(this Transform target, Transform to)
        {
            target.transform.position = to.position;
            target.transform.rotation = to.rotation;
        }
        
        public static void Teleport(this Transform target, Vector3 position, Quaternion rotation)
        {
            target.transform.position = position;
            target.transform.rotation = rotation;
        }
        
        public static bool RaycastToTransform(this Transform thisTransform, Transform target, out RaycastHit hitInfo,
            float maxDistance = 500, bool drawGizmos = true)
        {
            if (!Physics.Raycast(thisTransform.position, thisTransform.ToDirection(target), out hitInfo, maxDistance))
                return false;
            if (drawGizmos)
                Debug.DrawLine(thisTransform.position, hitInfo.point, Color.red);
            return true;
        }

        public static bool RaycastToTransform(this Transform thisTransform, Transform target, out RaycastHit hitInfo,
            int layerMask, float maxDistance = 500, bool drawGizmos = true)
        {
            if (!Physics.Raycast(thisTransform.position, thisTransform.ToDirection(target), out hitInfo, maxDistance,
                    layerMask))
                return false;
            if (drawGizmos)
                Debug.DrawLine(thisTransform.position, hitInfo.point, Color.red);
            return true;
        }

        public static Vector3 ToDirection(this Transform thisTransform, Transform target)
            => (target.position - thisTransform.position).normalized;

        public static List<Transform> GetChildren(this Transform target)
        {
            List<Transform> children = new List<Transform>();
            for (int i = 0; i < target.transform.childCount; i++)
            {
                Transform child = target.transform.GetChild(i);
                children.Add(child);
            }

            return children;
        }

        public static void AddToPositionXAxis(this Transform transform, float add)
        {
            Vector3 pos = transform.position;
            pos.x += add;
            transform.position = pos;
        }

        public static void AddToPositionYAxis(this Transform transform, float add)
        {
            Vector3 pos = transform.position;
            pos.y += add;
            transform.position = pos;
        }

        public static void AddToPositionZAxis(this Transform transform, float add)
        {
            Vector3 pos = transform.position;
            pos.z += add;
            transform.position = pos;
        }

        public static void ChangeLocalRotationAxis(this Transform transform, Axis axis, float value)
        {
            var rotation = transform.localRotation.eulerAngles;
            switch (axis)
            {
                case Axis.X: rotation.x = value; break;
                case Axis.Y: rotation.y = value; break;
                case Axis.Z: rotation.z = value; break;
            }
            transform.localRotation = Quaternion.Euler(rotation);
        }

        public static void ChangeRotationAxis(this Transform transform, Axis axis, float value)
        {
            var rotation = transform.rotation.eulerAngles;
            switch (axis)
            {
                case Axis.X: rotation.x = value; break;
                case Axis.Y: rotation.y = value; break;
                case Axis.Z: rotation.z = value; break;
            }
            transform.rotation = Quaternion.Euler(rotation);
        }
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }
}