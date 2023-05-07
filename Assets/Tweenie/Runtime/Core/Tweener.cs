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
        Action<ITweener> StopEvent { get; set; }
        Action StartCallback { get; set; }
        Action PlayCallback { get; set; }
        Action CompleteCallback { get; set; }

        internal void Tick(float deltaTime);

        ITweener SetEase(AnimationCurve curve);
        ITweener SetEase(Ease ease);

        ITweener SetLoop(Loop loopMode, int time);
        ITweener SetLoop(Loop loopMode);
        ITweener SetLoop(int time);

        ITweener Play();
        ITweener Stop();
        
        ITweener OnStart(Action callback);
        ITweener OnPlay(Action callback);
        ITweener OnComplete(Action callback);
    }

    public sealed class Tweener<T> : ITweener, ITweenData<T>
    {
        public Action<T> Param { get; set; }
        public T FromValue { get; set; }
        public T ToValue { get; set; }
        public float Duration { get; set; }
        public Func<T, T, float, T> LerpFunc { get; set; }
        public Action<ITweener> StopEvent { get; set; }
        public Action StartCallback { get; set; }
        public Action PlayCallback { get; set; }
        public Action CompleteCallback { get; set; }

        private bool _hasStarted;
        private float _timer;
        private AnimationCurve _curve;
        private Loop _loopMode ;
        private int _loopTime;

        internal Tweener(Action<T> param, T fromValue, T toValue, float duration, Func<T, T, float, T> lerpFunc)
        {
            Param = param;
            FromValue = fromValue;
            ToValue = toValue;
            Duration = duration;
            LerpFunc = lerpFunc;
            _hasStarted = false;
            _curve = AnimationCurve.Linear(0, 0, duration, 1);
            _loopMode = Loop.Default;
            _loopTime = 0;
        }

        void ITweener.Tick(float deltaTime)
        {
            if (_curve == null)
            {
                SetEase(Ease.Linear);
            }
            if (!_hasStarted)
            {
                StartCallback?.Invoke();
                _hasStarted = true;
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
                // event tell Tweenie this tweener is complete
                if (_loopTime == 0)
                {
                    StopEvent?.Invoke(this);
                    CompleteCallback?.Invoke();
                }
                else
                {
                    // do loop
                    _loopTime = _loopTime > 0 ? _loopTime - 1 : -1;

                    switch (_loopMode)
                    {
                        case Loop.PingPong:
                            (FromValue, ToValue) = (ToValue, FromValue);
                            _timer = 0;
                            break;
                        
                        case Loop.Default:
                        default:
                            _timer = 0;
                            break;
                    }
                    
                }
            }
        }

        public ITweener Play()
        {
            Tweenie.AddTweener(this);
            return this;
        }

        public ITweener Stop()
        {
            StopEvent?.Invoke(this);
            return this;
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
        
        public ITweener SetLoop(Loop loopMode, int time)
        {
            _loopMode = loopMode;
            _loopTime = time;
            return this;
        }

        public ITweener SetLoop(int time)
        {
            _loopTime = time;
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

        public ITweener OnComplete(Action callback)
        {
            CompleteCallback = callback;
            return this;
        }
    }
}