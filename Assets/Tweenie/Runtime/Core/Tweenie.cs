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

        /// <summary>
        ///     A dictionary that keep reference tweener under a certain tag for bulk manipulation
        /// </summary>
        private static Dictionary<object, ISet<ITweener>> _tagTweenerTable = new Dictionary<object, ISet<ITweener>>(); 


        #region Tween Setup - Frequency used type overloading

        /// <summary>
        ///     Start a tweener, type float overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <param name="tag">Tag to manage tweener in bulk</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<float> param, float fromValue, float toValue, float duration, object tag = null)
        {
            return To(param, fromValue, toValue, duration, Mathf.Lerp, tag);
        }

        /// <summary>
        ///     Start a tweener, type Vector2 overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <param name="tag">Tag to manage tweener in bulk</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Vector2> param, Vector2 fromValue, Vector2 toValue, float duration, object tag = null)
        {
            return To(param, fromValue, toValue, duration, Vector2.Lerp, tag);
        }
        
        /// <summary>
        ///     Start a tweener, type Vector3 overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <param name="tag">Tag to manage tweener in bulk</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Vector3> param, Vector3 fromValue, Vector3 toValue, float duration, object tag = null)
        {
            return To(param, fromValue, toValue, duration, Vector3.Lerp, tag);
        }

        /// <summary>
        ///     Start a tweener, type Vector4 overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <param name="tag">Tag to manage tweener in bulk</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Vector4> param, Vector4 fromValue, Vector4 toValue, float duration, object tag = null)
        {
            return To(param, fromValue, toValue, duration, Vector4.Lerp, tag);
        }

        /// <summary>
        ///     Start a tweener, type Quaternion overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <param name="tag">Tag to manage tweener in bulk</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Quaternion> param, Quaternion fromValue, Quaternion toValue, float duration, object tag = null)
        {
            return To(param, fromValue, toValue, duration, Quaternion.Lerp, tag);
        }

        /// <summary>
        ///     Start a tweener, type Coilor overload
        /// </summary>
        /// <param name="param">The param</param>
        /// <param name="fromValue">The from value</param>
        /// <param name="toValue">The to value</param>
        /// <param name="duration">The duration</param>
        /// <param name="tag">Tag to manage tweener in bulk</param>
        /// <returns>The tweener</returns>
        public static ITweener To(Action<Color> param, Color fromValue, Color toValue, float duration, object tag = null)
        {
            return To(param, fromValue, toValue, duration, Color.Lerp, tag);
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
        /// <param name="tag">Tag to manage tweener in bulk</param>
        /// <typeparam name="T">Parameter Type</typeparam>
        /// <returns>ITweener for method chaining</returns>
        public static ITweener To<T>(Action<T> param, T fromValue, T toValue, float duration, Func<T,T,float,T> lerpFunc, object tag = null)
        {
            Init();
            ITweener tweener = new Tweener<T>(param, fromValue, toValue, duration, lerpFunc, tag);
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
        ///     Play tweeners in bulk with the tag
        /// </summary>
        /// <param name="tag"></param>
        public static void PlayTweenerTag(object tag)
        {
            if (_tagTweenerTable.TryGetValue(tag, out ISet<ITweener> tweenerSet))
            {
                _toAddSet.UnionWith(tweenerSet);
            }
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
        ///     Pause tweeners in bulk with the tag
        /// </summary>
        /// <param name="tag"></param>
        public static void PauseTweenerTag(object tag)
        {
            if (_tagTweenerTable.TryGetValue(tag, out ISet<ITweener> tweenerSet))
            {
                _toPauseSet.UnionWith(tweenerSet);
            }
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
        ///     Stop tweeners in bulk with the tag
        /// </summary>
        /// <param name="tag"></param>
        public static void StopTweenerTag(object tag)
        {
            if (_tagTweenerTable.TryGetValue(tag, out ISet<ITweener> tweenerSet))
            {
                _toStopSet.UnionWith(tweenerSet);
            }
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
        
        /// <summary>
        ///     Complete tweener in bulk with the tag
        /// </summary>
        /// <param name="tag"></param>
        public static void CompleteTweenerTag(object tag)
        {
            if (_tagTweenerTable.TryGetValue(tag, out ISet<ITweener> tweenerSet))
            {
                _toCompleteSet.UnionWith(tweenerSet);
            }
        }

        /// <summary>
        ///     Remove tag from bulk editing
        /// </summary>
        /// <param name="tag"></param>
        public static void RemoveTag(object tag)
        {
            _tagTweenerTable.Remove(tag);
        }

        /// <summary>
        ///     Get tweeners under the tag
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static ISet<ITweener> GetTweenersFromTag(object tag)
        {
            return _tagTweenerTable.TryGetValue(tag, out ISet<ITweener> tweenerSet) ? tweenerSet : null;
        }
        
        /// <summary>
        ///     Update tweener from the old tag to the new tag
        /// </summary>
        /// <param name="tweener">target tweener</param>
        /// <param name="newTag">new tag</param>
        internal static void UpdateTag(ITweener tweener, object newTag)
        {
            if (tweener.Tag != null && _tagTweenerTable.TryGetValue(tweener.Tag, out ISet<ITweener> oldSet))
            {
                oldSet.Remove(tweener);
            }

            if (newTag == null)
            {
                return;
            }
            
            if (_tagTweenerTable.TryGetValue(newTag, out ISet<ITweener> newSet))
            {
                newSet.Add(tweener);
            }
            else
            {
                newSet = new HashSet<ITweener>();
                newSet.Add(tweener);
                _tagTweenerTable[newTag] = newSet;
            }
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
