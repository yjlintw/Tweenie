using System;
using UnityEngine;

namespace YJL.Tween
{
    /// <summary>
    ///     The ease enum
    /// </summary>
    public enum Ease
    {
        /// <summary>
        ///     The linear ease
        /// </summary>
        Linear,
        /// <summary>
        ///     The ease in out ease
        /// </summary>
        EaseInOut
    }

    /// <summary>
    /// The loop enum
    /// </summary>
    public enum Loop
    {
        /// <summary>
        ///     The default loop
        /// </summary>
        Default,
        /// <summary>
        ///     The ping pong loop
        /// </summary>
        PingPong
    }

    /// <summary>
    ///     The tween data interface
    /// </summary>
    public interface ITweenData<T>
    {
        /// <summary>
        ///     Parameter Setter
        /// </summary>
        Action<T> Param { get; set; }
        /// <summary>
        ///     The value this Tweener starts from
        /// </summary>
        T FromValue { get; set; }
        /// <summary>
        ///     The value this Tweener will be set to
        /// </summary>
        T ToValue { get; set; }
        /// <summary>
        ///     The time in seconds it will take from FromValue to ToValue
        /// </summary>
        float Duration { get; set; }
        /// <summary>
        ///     The interpolation function from FromValue to ToValue, controlled by a t(0.0f to 1.0f)
        /// </summary>
        Func<T, T, float, T> LerpFunc { get; set; }
    }


    /// <summary>
    /// The tweener interface
    /// </summary>
    public interface ITweener
    {
        #region Internal Callback Function
        // ---- Internal Fucntion for Tweenie to Signal Tweener ----
        /// <summary>
        ///     Callback function from Tweenie when this Tweener has started
        /// </summary>
        internal void TweenStarted();
        /// <summary>
        ///     Callback function from Tweenie when this Tweener has paused
        /// </summary>
        internal void TweenPaused();
        /// <summary>
        ///     Callback function from Tweenie when this Tweener has stopped
        /// </summary>
        internal void TweenStopped();
        /// <summary>
        ///     Callback function from Tweenie when this Tweener has completed
        /// </summary>
        internal void TweenCompleted();
        /// <summary>
        ///     Callback function from Tweenie when this Tweener has completed one step
        /// </summary>
        internal void TweenStepCompleted();
        /// <summary>
        ///     Callback function from Tweenie when this Tweener should continue on its loop
        /// </summary>
        internal void TweenContinue();
        #endregion
        
        #region Status Getter
        /// <summary>
        ///     Flag shows if current Tweener is completed
        /// </summary>
        public bool IsCompleted { get; }
        /// <summary>
        ///     Flag shows if current Tweener has a step completed
        /// </summary>
        public bool IsStepCompleted { get; }
        
        /// <summary>
        ///     Flag shows if the Tweener should stop after it reaches to a step complete
        /// </summary>
        public bool StopAfterStepCompleteFlag { get; }
        /// <summary>
        ///     Flag shows if the tweener should pause after it reaches to a step complete
        /// </summary>
        public bool PauseAfterStepCompleteFlag { get; }
        #endregion
        
        #region Update
        /// <summary>
        ///     Function the execute every single frame
        /// </summary>
        /// <param name="deltaTime"></param>
        internal void Tick(float deltaTime);
        #endregion
        
        #region Twenner Setters
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
        #endregion
        
        #region Tweener Controls        
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
        ///     Pause after tween complete current step
        /// </summary>
        /// <returns></returns>
        ITweener PauseAfterStepComplete();
        
        /// <summary>
        ///     Complete the tween, the param will set to ToValue
        /// </summary>
        /// <returns></returns>
        ITweener Complete();
        #endregion

        #region Callback Setter
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
        #endregion
    }

    /// <summary>
    /// The tweener class
    /// </summary>
    /// <seealso cref="ITweener"/>
    /// <seealso cref="ITweenData{T}"/>
    public sealed class Tweener<T> : ITweener, ITweenData<T>
    {
        #region Tweener Data
        /// <summary>
        ///     Parameter Setter
        /// </summary>
        public Action<T> Param { get; set; }
        /// <summary>
        ///     The value this Tweener starts from
        /// </summary>
        public T FromValue { get; set; }
        /// <summary>
        ///     The value this Tweener will be set to
        /// </summary>
        public T ToValue { get; set; }
        /// <summary>
        ///     The time in seconds it will take from FromValue to ToValue
        /// </summary>
        public float Duration { get; set; }
        /// <summary>
        ///     The interpolation function from FromValue to ToValue, controlled by a t(0.0f to 1.0f)
        /// </summary>
        public Func<T, T, float, T> LerpFunc { get; set; }
        #endregion
        
        #region Status Getter
        /// <summary>
        ///     Flag shows if current Tweener is completed
        /// </summary>
        public bool IsCompleted { get; private set; }
        /// <summary>
        ///     Flag shows if current Tweener has a step completed
        /// </summary>
        public bool IsStepCompleted { get; private set; }

        /// <summary>
        ///     Flag shows if the Tweener should stop after it reaches to a step complete
        /// </summary>
        public bool StopAfterStepCompleteFlag { get; private set; }
        /// <summary>
        ///     Flag shows if the tweener should pause after it reaches to a step complete
        /// </summary>
        public bool PauseAfterStepCompleteFlag { get; private set; }
        #endregion
        
        #region Tweener Callback
        /// <summary>
        ///     Callback to trigger when the Tweener has started
        /// </summary>
        private Action StartCallback { get; set; }
        /// <summary>
        ///     Callback to trigger when the Tweener has played
        /// </summary>
        private Action PlayCallback { get; set; }
        /// <summary>
        ///     Callback to trigger when the Tweener has paused
        /// </summary>
        private Action PauseCallback { get; set; }
        /// <summary>
        ///     Callback to trigger when the Tweener has stopped
        /// </summary>
        private Action StopCallback { get; set; }
        /// <summary>
        ///     Callback to trigger when the Tweener has completed
        /// </summary>
        private Action CompleteCallback { get; set; }
        /// <summary>
        ///     Callback to trigger when the Tweener has completed one step
        /// </summary>
        private Action StepCompleteCallback { get; set; }
        #endregion
        
        #region Private Field
        /// <summary>
        ///     internal timer to keep track of the time pass per animation loop
        /// </summary>
        private float _timer;
        /// <summary>
        ///     The animation curve this Tweener will follow
        /// </summary>
        private AnimationCurve _curve;
        /// <summary>
        ///     Loop mode
        /// </summary>
        private Loop _loopMode ;
        /// <summary>
        ///     How many loops this tweener still needs to play
        /// </summary>
        private int _loopCount;
        /// <summary>
        ///     Flag if this tweener is now in reverse mode
        /// </summary>
        private bool _isReverse;
        /// <summary>
        ///     Flag if this tweener is marked to destroy
        /// </summary>
        private bool _toDestroy;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="Tweener"/> class
        /// </summary>
        /// <param name="param">The param setter</param>
        /// <param name="fromValue">From value</param>
        /// <param name="toValue">To value</param>
        /// <param name="duration">Duration</param>
        /// <param name="lerpFunc">The interpolation func</param>
        internal Tweener(Action<T> param, T fromValue, T toValue, float duration, Func<T, T, float, T> lerpFunc)
        {
            _curve = AnimationCurve.Linear(0, 0, duration, 1);
            _loopMode = Loop.Default;
            _loopCount = 0;
            _isReverse = false;
            
            Param = param;
            FromValue = fromValue;
            ToValue = toValue;
            Duration = duration;
            LerpFunc = lerpFunc;
            IsCompleted = false;
            IsStepCompleted = false;
            StopAfterStepCompleteFlag = false;
            PauseAfterStepCompleteFlag = false;
        }
        #endregion

        # region Update
        /// <summary>
        /// Ticks every frame
        /// </summary>
        /// <param name="deltaTime">The delta time</param>
        void ITweener.Tick(float deltaTime)
        {
            PlayCallback?.Invoke();
            
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
            if (!TrySetParam(newValue))
            {
                return;
            }

            // Step end conditionk
            if (t >= 1)
            {
                if (_loopCount == 0)
                {
                    // loop complete
                    IsCompleted = true;
                }
                else
                {
                    IsStepCompleted = true;
                }
            }
        }
        #endregion

        #region Tweener Control
        /// <summary>
        /// Plays this Tweener
        /// </summary>
        /// <returns>The tweener</returns>
        public ITweener Play()
        {
            IsCompleted = false;
            IsStepCompleted = false;
            StopAfterStepCompleteFlag = false;
            PauseAfterStepCompleteFlag = false;
            return Tweenie.Play(this);
        }

        /// <summary>
        /// Pauses this Tweener
        /// </summary>
        /// <returns>The tweener</returns>
        public ITweener Pause()
        {
            return Tweenie.Pause(this);
        }

        /// <summary>
        /// Stops this Tweener
        /// </summary>
        /// <returns>The tweener</returns>
        public ITweener Stop()
        {
            return Tweenie.Stop(this);
        }

        /// <summary>
        /// Stops the after step complete
        /// </summary>
        /// <returns>The tweener</returns>
        public ITweener StopAfterStepComplete()
        {
            StopAfterStepCompleteFlag = true;
            return this;
        }

        /// <summary>
        /// Pauses this Tweener after step complete
        /// </summary>
        /// <returns>The tweener</returns>
        public ITweener PauseAfterStepComplete()
        {
            PauseAfterStepCompleteFlag = true;
            return this;
        }

        /// <summary>
        /// Completes this Tweener
        /// </summary>
        /// <returns>The tweener</returns>
        public ITweener Complete()
        {
            return Tweenie.Complete(this);
        }

        
        #endregion
        
        #region Modify Tweener Settings
        /// <summary>
        /// Sets the ease using the specified curve
        /// </summary>
        /// <param name="curve">The curve</param>
        /// <returns>The tweener</returns>
        public ITweener SetEase(AnimationCurve curve)
        {
            _curve = curve;
            return this;
        }

        /// <summary>
        /// Sets the ease using the specified ease type
        /// </summary>
        /// <param name="easeType">The ease type</param>
        /// <returns>The tweener</returns>
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

        /// <summary>
        /// Sets the loop using the specified loop mode
        /// </summary>
        /// <param name="loopMode">The loop mode</param>
        /// <returns>The tweener</returns>
        public ITweener SetLoop(Loop loopMode)
        {
            return SetLoop(loopMode, -1);
        }
        
        /// <summary>
        /// Sets the loop using the specified loop mode
        /// </summary>
        /// <param name="loopMode">The loop mode</param>
        /// <param name="count">The count</param>
        /// <returns>The tweener</returns>
        public ITweener SetLoop(Loop loopMode, int count)
        {
            _loopMode = loopMode;
            _loopCount = count;
            return this;
        }

        /// <summary>
        /// Sets the loop count using the specified count
        /// </summary>
        /// <param name="count">The count</param>
        /// <returns>The tweener</returns>
        public ITweener SetLoopCount(int count)
        {
            _loopCount = count;
            return this;
        }
        #endregion

        #region Callbacks
        /// <summary>
        ///     Set callback that will be triggered when the tweener has started
        /// </summary>
        /// <param name="callback">The callback</param>
        /// <returns>The tweener</returns>
        public ITweener OnStart(Action callback)
        {
            StartCallback = callback;
            return this;
        }

        /// <summary>
        ///     Set callback that will be triggered when the tweener is playing
        /// </summary>
        /// <param name="callback">The callback</param>
        /// <returns>The tweener</returns>
        public ITweener OnPlay(Action callback)
        {
            PlayCallback = callback;
            return this;
        }

        /// <summary>
        ///     Set callback that will be triggered when the tweener is paused
        /// </summary>
        /// <param name="callback">The callback</param>
        /// <returns>The tweener</returns>
        public ITweener OnPause(Action callback)
        {
            PauseCallback = callback;
            return this;
        }

        /// <summary>
        ///     Set callback that will be triggered when the tweener is stopped
        /// </summary>
        /// <param name="callback">The callback</param>
        /// <returns>The tweener</returns>
        public ITweener OnStop(Action callback)
        {
            StopCallback = callback;
            return this;
        }
        
        /// <summary>
        ///     Set callback that will be triggered when the tweener is completed
        /// </summary>
        /// <param name="callback">The callback</param>
        /// <returns>The tweener</returns>
        public ITweener OnComplete(Action callback)
        {
            CompleteCallback = callback;
            return this;
        }

        /// <summary>
        ///     Set callback that will be triggered when the tweener completed one step
        /// </summary>
        /// <param name="callback">The callback</param>
        /// <returns>The tweener</returns>
        public ITweener OnStepComplete(Action callback)
        {
            StepCompleteCallback = callback;
            return this;
        }
        #endregion
        
        #region Internal - Tweenie to Tweener Events
        /// <summary>
        /// Tween callback method when the tweener has started
        /// </summary>
        void ITweener.TweenStarted()
        {
            StartCallback?.Invoke();
        }

        /// <summary>
        /// Tween callback method when the tweener has paused
        /// </summary>
        void ITweener.TweenPaused()
        {
            PauseCallback?.Invoke();
        }

        /// <summary>
        /// Tween callback method when the tweener has stopped
        /// </summary>
        void ITweener.TweenStopped()
        {
            if (_toDestroy)
            {
                return;
            }
            TrySetParam(FromValue);
            Reset();
            StopCallback?.Invoke();
        }
        
        /// <summary>
        /// Tween callback method when the tweener has completed
        /// </summary>
        void ITweener.TweenCompleted()
        {
            if (_loopMode == Loop.PingPong)
            {
                TrySetParam(_isReverse ? FromValue : ToValue);
            }
            else
            {
                TrySetParam(ToValue);
            }
            Reset();
            CompleteCallback?.Invoke();
        }

        /// <summary>
        /// Tween callback method when the tweener has complete one step
        /// </summary>
        void ITweener.TweenStepCompleted()
        {
            StepCompleteCallback?.Invoke();
        }
            
        /// <summary>
        /// Tween callback method when the tweener should continue in the loop
        /// </summary>
        void ITweener.TweenContinue()
        {
            // TODO:
            //  1. Thinking a better way to handle different loop Mode
            //  2. At PingPong mode,
            //      a.  when counts as step complete? when it reach to ToValue and FromValue every time,
            //          or only when it reach to ToValue
            //      b.  what counts as one complete loop, every time it reach to ToValue or FromValue, or 
            //          a From-To-From counts as one loop?
            
            IsStepCompleted = false;
            // fixed time loop
            switch (_loopMode)
            {
                case Loop.PingPong:
                    // only when it is doing the reverse route count as a loop
                    if (!_isReverse)
                    {
                        _loopCount--;
                    }

                    // reverse the direction
                    _isReverse = !_isReverse;
                    // reset timer;
                    _timer = 0;
                    break;
                
                case Loop.Default:
                default:
                    // reach end point, loop count - 1
                    _loopCount--;
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

        #endregion

        #region Private Method

        /// <summary>
        /// Try set the param
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>false if exception thrown, true if value is set correctly</returns>
        bool TrySetParam(T value)
        {
            try
            {
                Param(value);
            }
            catch (MissingReferenceException e)
            {
                Debug.Log($"The parameter this tweener is setting is missing. " +
                          "Don't worry. Tweenie handles it. This tweener will be destroy in next frame.\n" +
                          $"{e.Message}");
                Destroy();
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                Destroy();
                return false;
            }

            return true;
        }
        /// <summary>
        /// Resets this instance
        /// </summary>
        void Reset()
        {
            _timer = 0;
            _isReverse = false;
            IsCompleted = false;
            IsStepCompleted = false;
            StopAfterStepCompleteFlag = false;
            PauseAfterStepCompleteFlag = false;
        }
        
        /// <summary>
        /// Destroys this instance
        /// </summary>
        void Destroy()
        {
            _toDestroy = true;
            Stop();
        }
        #endregion

    }
}