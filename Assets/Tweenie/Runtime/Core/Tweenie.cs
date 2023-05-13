using System;
using System.Collections.Generic;
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

        #region Tween Setup - Frequency used type overloading
        public static ITweener To(Action<float> param, float fromValue, float toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Mathf.Lerp);
        }

        public static ITweener To(Action<Vector2> param, Vector2 fromValue, Vector2 toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Vector2.Lerp);
        }
        
        public static ITweener To(Action<Vector3> param, Vector3 fromValue, Vector3 toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Vector3.Lerp);
        }

        public static ITweener To(Action<Vector4> param, Vector4 fromValue, Vector4 toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Vector4.Lerp);
        }

        public static ITweener To(Action<Quaternion> param, Quaternion fromValue, Quaternion toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Quaternion.Lerp);
        }

        public static ITweener To(Action<Color> param, Color fromValue, Color toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Color.Lerp);
        }
        #endregion

        #region Generic Tween Setup Method
        /// <summary>
        ///     Basic To Method
        /// </summary>
        /// <param name="param">Value Setter</param>
        /// <param name="fromValue">Starting Value</param>
        /// <param name="toValue">Ending Value</param>
        /// <param name="duration">Duration from Start to End</param>
        /// <param name="lerpFunc">Interpolation Function</param>
        /// <typeparam name="T">Parameter Type</typeparam>
        /// <returns>ITweener for method chaining</returns>
        public static ITweener To<T>(Action<T> param, T fromValue, T toValue, float duration, Func<T,T,float,T> lerpFunc)
        {
            Init();
            ITweener tweener = new Tweener<T>(param, fromValue, toValue, duration, lerpFunc);
            _toAddSet.Add(tweener);
            return tweener;
        }
        #endregion

        #region Tweener Control
        /// <summary>
        ///     Play the Tweener
        /// </summary>
        /// <param name="tweener"></param>
        /// <returns></returns>
        public static ITweener Play(ITweener tweener)
        {
            _toAddSet.Add(tweener);
            return tweener;
        }

        /// <summary>
        ///     Pause the Tweener, Tweener will stop at its current location
        /// </summary>
        /// <param name="tweener"></param>
        /// <returns></returns>
        public static ITweener Pause(ITweener tweener)
        {
            _toPauseSet.Add(tweener);
            return tweener;
        }

        /// <summary>
        ///     Stop the Tweener, Tweener will jump back to its starting location
        /// </summary>
        /// <param name="tweener"></param>
        /// <returns></returns>
        public static ITweener Stop(ITweener tweener)
        {
            _toStopSet.Add(tweener);
            return tweener;
        }

        //  TODO: WARNING
        //  Of all the Tweener Control Function, only this one's direction is different than others
        //  All other tween control has the implementation in the Tweenie class, Tweener counterpart calls Tweenie's
        //  method
        //  This one has the implementation in the Tweener class, Tweenie counterpart is calling Tweener's function
        
        /// <summary>
        ///     Stop the tweener after the current step is completed
        /// </summary>
        /// <param name="tweener"></param>
        /// <returns></returns>
        public static ITweener StopAfterStepComplete(ITweener tweener)
        {
            return tweener.StopAfterStepComplete();
        }

        /// <summary>
        ///     Complete the Tweener, Tweener will jump to its ending location
        /// </summary>
        /// <param name="tweener"></param>
        /// <returns></returns>
        public static ITweener Complete(ITweener tweener)
        {
            _toCompleteSet.Add(tweener);
            return tweener;
        }
        
        #endregion
        
        #region Unity Life Cycle
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
        #endregion
    }
}
