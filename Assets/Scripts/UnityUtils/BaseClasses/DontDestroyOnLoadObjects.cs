using System;
using UnityEngine;
using UnityUtils.Extensions;
using Object = UnityEngine.Object;

namespace UnityUtils.BaseClasses
{
    public class DontDestroyOnLoadObjects<T> : SingletonBehavior<DontDestroyOnLoadObjects<T>> where T : Object
    {
        protected virtual void Awake() => gameObject.DontDestroyOnLoadIfSingle<T>();
    }
}