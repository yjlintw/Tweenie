using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YJL.Tween
{
    public static class TweenExtension
    {
        public static ITweener MoveTo(this Transform transform, Vector3 toValue, float duration)
        {
            return Tweenie.To(x => transform.position = x, transform.position, toValue, duration);
        }

    }
}
