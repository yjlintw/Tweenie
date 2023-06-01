using System;
using System.Collections.Generic;
using UnityEngine;
using YJL.Helper;

namespace YJL.Tween
{
    /// <summary>
    /// The tweenie class
    /// </summary>
    /// <seealso cref="Singleton{Tweenie}"/>
    public sealed class Tweenie : Singleton<Tweenie>
    {
        /// <summary>
        ///     A set of tweeners that will run in this current frame
        /// </summary>
        private static ISet<ITweener> _tweenerSet = new HashSet<ITweener>();
        /// <summary>
        ///     A set of tweeners that will be added to run at the start of this frame
        /// </summary>
        private static ISet<ITweener> _toAddSet = new HashSet<ITweener>();
        /// <summary>
        ///     A set of tweeners that will be paused at the end of this frame
        /// </summary>
        private static ISet<ITweener> _toPauseSet = new HashSet<ITweener>();
        /// <summary>
        ///     A set of tweeners that will be stopped at the end of this frame
        /// </summary>
        private static ISet<ITweener> _toStopSet = new HashSet<ITweener>();
        /// <summary>
        ///     A set of tweeners that will be completed at the end of this frame
        /// </summary>
        private static ISet<ITweener> _toCompleteSet = new HashSet<ITweener>();

        #region Tween Setup - Frequency used type overloading
        /// <summary>
        ///     Start a tweener, type float overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<float> param, float fromValue, float toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Mathf.Lerp);
        }

        /// <summary>
        ///     Start a tweener, type Vector2 overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Vector2> param, Vector2 fromValue, Vector2 toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Vector2.Lerp);
        }
        
        /// <summary>
        ///     Start a tweener, type Vector3 overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Vector3> param, Vector3 fromValue, Vector3 toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Vector3.Lerp);
        }

        /// <summary>
        ///     Start a tweener, type Vector4 overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Vector4> param, Vector4 fromValue, Vector4 toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Vector4.Lerp);
        }

        /// <summary>
        ///     Start a tweener, type Quaternion overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Quaternion> param, Quaternion fromValue, Quaternion toValue, float duration)
        {
            return To(param, fromValue, toValue, duration, Quaternion.Lerp);
        }

        /// <summary>
        ///     Start a tweener, type Coilor overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <returns>The tweener</returns>
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
        ///     Pauses the tweener after the current step is completed
        /// </summary>
        /// <param name="tweener">The tweener</param>
        /// <returns>The tweener</returns>
        public static ITweener PauseAfterStepComplete(ITweener tweener)
        {
            return tweener.PauseAfterStepComplete();
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
        /// <summary>
        ///     Unity's update function
        /// </summary>
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

            // remove tweens that already marked as stop
            _tweenerSet.ExceptWith(_toStopSet);
            
            foreach (ITweener tweener in _tweenerSet)
            {
                tweener.Tick(Time.deltaTime);
                if (tweener.IsCompleted)
                {
                    _toCompleteSet.Add(tweener);
                    continue;
                }
                if (!tweener.IsStepCompleted) continue;
                
                tweener.TweenStepCompleted();
                // Handle condition when one tweener step completed
                if (tweener.StopAfterStepCompleteFlag)
                {
                    _toStopSet.Add(tweener);
                }
                else if (tweener.PauseAfterStepCompleteFlag)
                {
                    _toPauseSet.Add(tweener);
                }
                else
                {
                    tweener.TweenContinue();
                }
            }
        }

        /// <summary>
        ///     Unity's Late Update function
        /// </summary>
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
        
        #region Editor Related

        /// <summary>
        ///     Gets the number of running tweener
        /// </summary>
        /// <returns>The int</returns>
        public static int GetNumberOfRunningTweener()
        {
            return Tweenie._tweenerSet.Count;
        }
        #endregion
    }
}
