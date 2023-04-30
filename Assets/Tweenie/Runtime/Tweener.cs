using System;

namespace YJL.Tween
{
    public interface ITweenData<T>
    {
        Func<T, T> Param { get; set; }
        T FromValue { get; set; }
        T ToValue { get; set; }
        float Duration { get; set; }
        Func<T, T, float, T> LerpFunc { get; set; }
    }

    public interface ITweener
    {
        Action<ITweener> CompleteEvent { get; set; }
        Action Callback { get; set; }

        internal void Tick(float deltaTime);
        void OnComplete(Action callback);
    }

    public class Tweener<T> : ITweener, ITweenData<T>
    {
        public Func<T, T> Param { get; set; }
        public T FromValue { get; set; }
        public T ToValue { get; set; }
        public float Duration { get; set; }
        public Func<T, T, float, T> LerpFunc { get; set; }
        public Action<ITweener> CompleteEvent { get; set; }
        public Action Callback { get; set; }
        
        private float _timer;

        internal Tweener(Func<T, T> param, T fromValue, T toValue, float duration, Func<T, T, float, T> lerpFunc)
        {
            Param = param;
            FromValue = fromValue;
            ToValue = toValue;
            Duration = duration;
            LerpFunc = lerpFunc;
        }

        void ITweener.Tick(float deltaTime)
        {
            _timer += deltaTime;
            
            // TODO: update this with Animation Curve
            float t = _timer / Duration;
            t = t > 1 ? 1 : t;
            
            // implementation
            T newValue = LerpFunc(FromValue, ToValue, t);

            Param(newValue);

            if (t >= 1)
            {
                CompleteEvent?.Invoke(this);
            }
        }

        public void OnComplete(Action callback)
        {
            Callback = callback;
        }
    }
}