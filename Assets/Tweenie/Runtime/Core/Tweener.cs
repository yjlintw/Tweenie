﻿using System;

namespace YJL.Tween
{
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
        Action<ITweener> CompleteEvent { get; set; }
        Action StartCallback { get; set; }
        Action PlayCallback { get; set; }
        Action CompleteCallback { get; set; }

        internal void Tick(float deltaTime);
        ITweener OnStart(Action callback);
        ITweener OnPlay(Action callback);
        ITweener OnComplete(Action callback);
    }

    public class Tweener<T> : ITweener, ITweenData<T>
    {
        public Action<T> Param { get; set; }
        public T FromValue { get; set; }
        public T ToValue { get; set; }
        public float Duration { get; set; }
        public Func<T, T, float, T> LerpFunc { get; set; }
        public Action<ITweener> CompleteEvent { get; set; }
        public Action StartCallback { get; set; }
        public Action PlayCallback { get; set; }
        public Action CompleteCallback { get; set; }

        private bool _hasStarted;
        private float _timer;

        internal Tweener(Action<T> param, T fromValue, T toValue, float duration, Func<T, T, float, T> lerpFunc)
        {
            Param = param;
            FromValue = fromValue;
            ToValue = toValue;
            Duration = duration;
            LerpFunc = lerpFunc;
            _hasStarted = false;
        }

        void ITweener.Tick(float deltaTime)
        {
            if (!_hasStarted)
            {
                StartCallback?.Invoke();
                _hasStarted = true;
            }
            _timer += deltaTime;
            
            // TODO: update this with Animation Curve
            float t = _timer / Duration;
            t = t > 1 ? 1 : t;
            
            // implementation
            T newValue = LerpFunc(FromValue, ToValue, t);

            Param(newValue);
            
            PlayCallback?.Invoke();

            if (t >= 1)
            {
                // event tell Tweenie this tweener is complete
                CompleteEvent?.Invoke(this);
                CompleteCallback?.Invoke();
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