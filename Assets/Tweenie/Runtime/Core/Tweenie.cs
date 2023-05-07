using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using YJL.Helper;

namespace YJL.Tween
{
    public sealed class Tweenie : Singleton<Tweenie>
    {
        private static ISet<ITweener> _tweenerSet = new HashSet<ITweener>();
        private static ISet<ITweener> _toAddSet = new HashSet<ITweener>();
        private static ISet<ITweener> _toRemoveSet = new HashSet<ITweener>();

        public static ITweener To(Action<float> param, float fromValue, float toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Mathf.Lerp);
        }

        public static ITweener To(Action<Vector3> param, Vector3 fromValue, Vector3 toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Vector3.Lerp);
        }

        public static ITweener To(Action<Color> param, Color fromValue, Color toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Color.Lerp);
        }

        public static ITweener To<T>(Action<T> param, T fromValue, T toValue, float duration, Func<T,T,float,T> lerpFunc)
        {
            Init();
            ITweener tweener = new Tweener<T>(param, fromValue, toValue, duration, lerpFunc);
            _toAddSet.Add(tweener);
            tweener.StopEvent += OnTweenStop;

            return tweener;
        }

        public static ITweener AddTweener(ITweener newTweener)
        {
            _toAddSet.Add(newTweener);
            return newTweener;
        }

        private static void OnTweenStop(ITweener obj)
        {
            _toRemoveSet.Add(obj);
        }


        // Unity Life Cycle
        public void Start()
        {
        }
        
        public void Update()
        {
            // Add new tween
            _tweenerSet.UnionWith(_toAddSet);
            _toAddSet.Clear();
            
            foreach (ITweener tweener in _tweenerSet)
            {
                tweener.Tick(Time.deltaTime);
            }
        }

        public void LateUpdate()
        {
            foreach (ITweener toRemove in _toRemoveSet)
            {
                _tweenerSet.Remove(toRemove);
            }
            
            _toRemoveSet.Clear();
        }
    }
}
