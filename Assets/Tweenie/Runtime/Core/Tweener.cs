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
        internal void TweenStarted();
        internal void TweenPaused();
        internal void TweenStopped();
        internal void TweenCompleted();

        internal void Tick(float deltaTime);

        ITweener SetEase(AnimationCurve curve);
        ITweener SetEase(Ease ease);

        ITweener SetLoop(Loop loopMode, int count);
        ITweener SetLoop(Loop loopMode);
        ITweener SetLoopCount(int count);

        ITweener Play();
        ITweener Pause();
        ITweener Stop();
        ITweener Complete();
        
        ITweener OnStart(Action callback);
        ITweener OnPlay(Action callback);
        ITweener OnPause(Action callback);
        ITweener OnStop(Action callback);
        ITweener OnComplete(Action callback);
        ITweener OnStepComplete(Action callback);
    }

    public sealed class Tweener<T> : ITweener, ITweenData<T>
    {
        public Action<T> Param { get; set; }
        public T FromValue { get; set; }
        public T ToValue { get; set; }
        public float Duration { get; set; }
        public Func<T, T, float, T> LerpFunc { get; set; }
        
        
        // Callback is for caller

        private Action StartCallback { get; set; }
        private Action PlayCallback { get; set; }
        private Action PauseCallback { get; set; }
        private Action StopCallback { get; set; }
        private Action CompleteCallback { get; set; }
        private Action StepCompleteCallback { get; set; }
        
        private float _timer;
        private AnimationCurve _curve;
        private Loop _loopMode ;
        private int _loopCount;
        private bool _isReverse;

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

        void ITweener.Tick(float deltaTime)
        {
            if (_curve == null)
            {
                SetEase(Ease.Linear);
            }
            
            _timer += deltaTime;
            float t = _curve.Evaluate(_timer);
            t = t > 1 ? 1 : t;
            // implementation
            T newValue = LerpFunc(FromValue, ToValue, t);

            
            // Update Parameter
            Param(newValue);
            PlayCallback?.Invoke();

            if (t >= 1)
            {
                Debug.Log(_loopCount);
                // event tell Tweenie this tweener is complete
                if (_loopCount == 0)
                {
                    Complete();
                }
                else
                {
                    // do loop
                    switch (_loopMode)
                    {
                        case Loop.PingPong:
                            (FromValue, ToValue) = (ToValue, FromValue);
                            if (_isReverse)
                            {
                                StepCompleteCallback?.Invoke();
                            }
                            else
                            {
                                _loopCount--;
                            }

                            _isReverse = !_isReverse;
                            _timer = 0;
                            break;
                        
                        case Loop.Default:
                        default:
                            _loopCount--;
                            StepCompleteCallback?.Invoke();
                            _timer = 0;
                            break;
                    }

                    if (_loopCount < 0)
                    {
                        _loopCount = -1;
                    }
                }
            }
        }

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

        public ITweener Complete()
        {
            return Tweenie.Complete(this);
        }
        
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
            Param(ToValue);
            Reset();
            CompleteCallback?.Invoke();
        }


        void Reset()
        {
            _timer = 0;
            if (_isReverse)
            {
                (FromValue, ToValue) = (ToValue, FromValue);
            }

            _isReverse = false;
        }

    }
}