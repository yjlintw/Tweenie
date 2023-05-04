using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YJL.Helper;

namespace YJL.Tween
{
    public class Tweenie : Singleton<Tweenie>
    {
        private static ISet<ITweener> _tweenerSet = new HashSet<ITweener>();
        private static ISet<ITweener> _toRemoveSet = new HashSet<ITweener>();

        public static ITweener To(Func<float, float> param, float fromValue, float toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Mathf.Lerp);
        }

        public static ITweener To(Func<Vector3, Vector3> param, Vector3 fromValue, Vector3 toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Vector3.Lerp);
        }

        public static ITweener To(Func<Color, Color> param, Color fromValue, Color toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Color.Lerp);
        }

        public static ITweener To<T>(Func<T, T> param, T fromValue, T toValue, float duration, Func<T,T,float,T> lerpFunc)
        {
            Init();
            ITweener tweener = new Tweener<T>(param, fromValue, toValue, duration, lerpFunc);
            _tweenerSet.Add(tweener);
            tweener.CompleteEvent += OnTweenComplete;

            return tweener;
        }

        private static void OnTweenComplete(ITweener obj)
        {
            _toRemoveSet.Add(obj);
        }


        // Unity Life Cycle
        public void Start()
        {
        }
        
        public void Update()
        {
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
                toRemove.Callback?.Invoke();
            }
            
            _toRemoveSet.Clear();
        }
    }
}
