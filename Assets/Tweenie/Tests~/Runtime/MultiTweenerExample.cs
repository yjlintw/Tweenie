using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YJL.Tween;

namespace YJL.Tween.Test
{
    public class MultiTweenerExample : MonoBehaviour
    {
        private void OnEnable()
        {
            Tweenie.PlayTweenerTag(this);
        }

        // Start is called before the first frame update
        void Start()
        {
            Tweenie.To(
                    x => { transform.localRotation = Quaternion.Euler(0, x, 0); },
                    0,
                    360,
                    1.0f,
                    this)
                .SetLoop(Loop.PingPong);
            Tweenie.To(
                    x => transform.position = x,
                    transform.position,
                    transform.position + new Vector3(5, 5, 5),
                    1.0f,
                    this)
                .SetLoop(Loop.PingPong);
            Tweenie.To(
                    x => transform.localScale = x,
                    new Vector3(1, 1, 1),
                    new Vector3(2f, 2f, 5f),
                    1.0f,
                    this)
                .SetLoop(Loop.PingPong);
        }

        private void OnDisable()
        {
            Tweenie.PauseTweenerTag(this);
        }

        private void OnDestroy()
        {
            Tweenie.RemoveTag(this);
        }
    }
}
