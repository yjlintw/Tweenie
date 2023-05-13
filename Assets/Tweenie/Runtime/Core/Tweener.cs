using System;
using UnityEngine;

namespace YJL.Tween
{
    public enum Ease
    {
        Linear,
        EaseInOut
    }

    public enum Loop
    {
        Default,
        PingPong
    }

    public interface ITweenData<T>
    {
        Action<T> Param { get; set; }
        T FromValue { get; set; }
        T ToValue { get; set; }
        float Duration { get; set; }
        Func<T, T, float, T> LerpFunc { get; set; }
    }


    public interface ITweener
    {
        // ---- Internal Fucntion for Tweenie to Signal Tweener ----
        internal void TweenStarted();
        internal void TweenPaused();
        internal void TweenStopped();
        internal void TweenCompleted();

        /// <summary>
        ///     Function the execute every single frame
        /// </summary>
        /// <param name="deltaTime"></param>
        internal void Tick(float deltaTime);

        /// <summary>
        ///     Set Easing using Animation Curve
        /// </summary>
        /// <param name="curve"></param>
        /// <returns></returns>
        ITweener SetEase(AnimationCurve curve);
        
        /// <summary>
        ///     Set Easing using built-in Easing type
        /// </summary>
        /// <param name="ease"></param>
        /// <returns></returns>
        ITweener SetEase(Ease ease);

        /// <summary>
        ///     Set Loop Mode and Loop Count
        /// </summary>
        /// <param name="loopMode"></param>
        /// <param name="count">How many times the loop will run, Loop will run infinitely if count is set to -1</param>
        /// <returns></returns>
        ITweener SetLoop(Loop loopMode, int count);
        
        /// <summary>
        ///     Set Loop Mode, Loop will run infinitely
        /// </summary>
        /// <param name="loopMode"></param>
        /// <returns></returns>
        ITweener SetLoop(Loop loopMode);
        
        /// <summary>
        ///     Set Loop Count
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        ITweener SetLoopCount(int count);

        /// <summary>
        ///     Play the tween
        /// </summary>
        /// <returns></returns>
        ITweener Play();
        
        /// <summary>
        ///     Pause the tween, the param will stay at its current value
        /// </summary>
        /// <returns></returns>
        ITweener Pause();
        
        /// <summary>
        ///     Stop the tween, the param will jump back to FromValue
        /// </summary>
        /// <returns></returns>
        ITweener Stop();
        
        /// <summary>
        ///     Stop after tween complete current step
        /// </summary>
        /// <returns></returns>
        ITweener StopAfterStepComplete();
        
        /// <summary>
        ///     Complete the tween, the param will set to ToValue
        /// </summary>
        /// <returns></returns>
        ITweener Complete();
        
        
        /// <summary>
        ///     Set Callback that will be triggered when the first frame of the Tweener starts
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ITweener OnStart(Action callback);
        
        /// <summary>
        ///     Set Callback that will be triggered each frame this tweener is playing
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ITweener OnPlay(Action callback);
        
        /// <summary>
        ///     Set Callback that will be triggered when this tweener is paused
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ITweener OnPause(Action callback);
        
        /// <summary>
        ///     Set Callback that will be triggered when this tweener is stopped
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ITweener OnStop(Action callback);
        
        /// <summary>
        ///     Set Callback that will be triggered when this tweener is completed
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ITweener OnComplete(Action callback);
        
        /// <summary>
        ///     Set callback that will be triggered when this tweener complete one step
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        ITweener OnStepComplete(Action callback);
    }

    public sealed class Tweener<T> : ITweener, ITweenData<T>
    {
        #region Tweener Data
        public Action<T> Param { get; set; }
        public T FromValue { get; set; }
        public T ToValue { get; set; }
        public float Duration { get; set; }
        public Func<T, T, float, T> LerpFunc { get; set; }
        #endregion
        
        #region Tweener Callback
        private Action StartCallback { get; set; }
        private Action PlayCallback { get; set; }
        private Action PauseCallback { get; set; }
        private Action StopCallback { get; set; }
        private Action CompleteCallback { get; set; }
        private Action StepCompleteCallback { get; set; }
        #endregion
        
        #region Private Field
        private float _timer;
        private AnimationCurve _curve;
        private Loop _loopMode ;
        private int _loopCount;
        private bool _isReverse;
        #endregion

        #region Constructor
        internal Tweener(Action<T> param, T fromValue, T toValue, float duration, Func<T, T, float, T> lerpFunc)
        {
            Param = param;
            FromValue = fromValue;
            ToValue = toValue;
            Duration = duration;
            LerpFunc = lerpFunc;
            _curve = AnimationCurve.Linear(0, 0, duration, 1);
            _loopMode = Loop.Default;
            _loopCount = 0;
            _isReverse = false;
        }
        #endregion

        # region Update
        void ITweener.Tick(float deltaTime)
        {
            if (_curve == null)
            {
                SetEase(Ease.Linear);
            }
            
            // Interpolation
            _timer += deltaTime;
            float t = _curve.Evaluate(_timer);
            t = t > 1 ? 1 : t;
            T newValue = _isReverse ? LerpFunc(ToValue, FromValue, t) : LerpFunc(FromValue, ToValue, t);

            
            // Update Parameter
            Param(newValue);
            PlayCallback?.Invoke();

            // TODO:
            //  1. Thinking a better way to handle different loop Mode
            //  2. At PingPong mode,
            //      a.  when counts as step complete? when it reach to ToValue and FromValue every time,
            //          or only when it reach to ToValue
            //      b.  what counts as one complete loop, every time it reach to ToValue or FromValue, or 
            //          a From-To-From counts as one loop?
            
            
            // Step end conditionk
            if (t >= 1)
            {
                if (_loopCount == 0)
                {
                    // loop complete
                    Complete();
                }
                else
                {
                    // fixed time loop
                    switch (_loopMode)
                    {
                        case Loop.PingPong:
                            // only when it is doing the reverse route count as a loop
                            if (!_isReverse)
                            {
                                _loopCount--;
                            }

                            // each time it reach the endpoint, send a step complete event
                            StepCompleteCallback?.Invoke();
                            
                            // reverse the direction
                            _isReverse = !_isReverse;
                            
                            // reset timer;
                            _timer = 0;
                            break;
                        
                        case Loop.Default:
                        default:
                            // reach end point, loop count - 1
                            _loopCount--;
                            
                            // send step complete event
                            StepCompleteCallback?.Invoke();
                            
                            // reset timer
                            _timer = 0;
                            break;
                    }

                    // infinite loop
                    if (_loopCount < 0)
                    {
                        _loopCount = -1;
                    }
                }
            }
        }
        #endregion

        #region Tweener Control
        public ITweener Play()
        {
            return Tweenie.Play(this);
        }

        public ITweener Pause()
        {
            return Tweenie.Pause(this);
        }

        public ITweener Stop()
        {
            return Tweenie.Stop(this);
        }

        public ITweener StopAfterStepComplete()
        {
            _loopCount = 0;
            return this;
        }

        public ITweener Complete()
        {
            return Tweenie.Complete(this);
        }
        
        #endregion
        
        #region Modify Tweener Settings
        public ITweener SetEase(AnimationCurve curve)
        {
            _curve = curve;
            return this;
        }

        public ITweener SetEase(Ease easeType)
        {
            switch (easeType)
            {
                case Ease.EaseInOut:
                    _curve = AnimationCurve.EaseInOut(0, 0, Duration, 1);
                    break;
                default:
                case Ease.Linear:
                    _curve = AnimationCurve.Linear(0, 0, Duration, 1);
                    break;
            }

            return this;
        }

        public ITweener SetLoop(Loop loopMode)
        {
            return SetLoop(loopMode, -1);
        }
        
        public ITweener SetLoop(Loop loopMode, int count)
        {
            _loopMode = loopMode;
            _loopCount = count;
            return this;
        }

        public ITweener SetLoopCount(int count)
        {
            _loopCount = count;
            return this;
        }
        #endregion

        #region Callbacks
        public ITweener OnStart(Action callback)
        {
            StartCallback = callback;
            return this;
        }

        public ITweener OnPlay(Action callback)
        {
            PlayCallback = callback;
            return this;
        }

        public ITweener OnPause(Action callback)
        {
            PauseCallback = callback;
            return this;
        }

        public ITweener OnStop(Action callback)
        {
            StopCallback = callback;
            return this;
        }
        
        public ITweener OnComplete(Action callback)
        {
            CompleteCallback = callback;
            return this;
        }

        public ITweener OnStepComplete(Action callback)
        {
            StepCompleteCallback = callback;
            return this;
        }
        #endregion
        
        #region Internal - Tweenie to Tweener Events
        void ITweener.TweenStarted()
        {
            StartCallback?.Invoke();
        }

        void ITweener.TweenPaused()
        {
            PauseCallback?.Invoke();
        }

        void ITweener.TweenStopped()
        {
            Param(FromValue);
            Reset();
            StopCallback?.Invoke();
        }
        
        void ITweener.TweenCompleted()
        {
            if (_loopMode == Loop.PingPong)
            {
                Param(_isReverse ? FromValue : ToValue);
            }
            else
            {
                Param(ToValue);
            }
            Reset();
            CompleteCallback?.Invoke();
        }
        #endregion

        #region Private Method
        void Reset()
        {
            _timer = 0;
            _isReverse = false;
        }
        #endregion

    }
}