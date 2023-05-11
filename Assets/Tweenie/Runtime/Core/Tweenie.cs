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
        private static ISet<ITweener> _toPauseSet = new HashSet<ITweener>();
        private static ISet<ITweener> _toStopSet = new HashSet<ITweener>();
        private static ISet<ITweener> _toCompleteSet = new HashSet<ITweener>();

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
            return tweener;
        }

        public static ITweener Play(ITweener tweener)
        {
            _toAddSet.Add(tweener);
            return tweener;
        }

        public static ITweener Pause(ITweener tweener)
        {
            _toPauseSet.Add(tweener);
            return tweener;
        }

        public static ITweener Stop(ITweener tweener)
        {
            _toStopSet.Add(tweener);
            return tweener;
        }

        public static ITweener Complete(ITweener tweener)
        {
            _toCompleteSet.Add(tweener);
            return tweener;
        }


        // Unity Life Cycle
        public void Start()
        {
        }
        
        public void Update()
        {
            // Add new tween
            foreach (ITweener tweener in _toAddSet)
            {
                if (!_tweenerSet.Contains(tweener))
                {
                    tweener.TweenStarted();
                    _tweenerSet.Add(tweener);
                }
            }
            _toAddSet.Clear();
            
            foreach (ITweener tweener in _tweenerSet)
            {
                tweener.Tick(Time.deltaTime);
            }
        }

        public void LateUpdate()
        {
            foreach (ITweener toRemove in _toStopSet)
            {
                _tweenerSet.Remove(toRemove);
                toRemove.TweenStopped();
            }
            _toStopSet.Clear();

            foreach (ITweener toPause in _toPauseSet)
            {
                _tweenerSet.Remove(toPause);
                toPause.TweenPaused();
            }
            _toPauseSet.Clear();

            foreach (ITweener toComplete in _toCompleteSet)
            {
                _tweenerSet.Remove(toComplete);
                toComplete.TweenCompleted();
            }
            _toCompleteSet.Clear();
        }
    }
}
