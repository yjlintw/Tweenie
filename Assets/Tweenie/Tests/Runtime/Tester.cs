using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace YJL.Tween.Test
{
    public class Tester : MonoBehaviour
    {
        public float from;
        public float to;

        public Vector3 toPosition;
        public float duration;

        public float myValue;

        public bool inProgress = false;

        // Update is called once per frame

        [ContextMenu("Run")]
        public void Run()
        {
            Vector3 newPosition = new Vector3(Random.value * 10f, Random.value * 10f, Random.value * 10f);
            Tweenie.To(x => myValue = x, myValue, newPosition.magnitude, duration);
            Tweenie.To(x => transform.position = x, 
                transform.position, 
                newPosition, 
                duration)
                .SetEase(Ease.EaseInOut)
                .OnComplete(OnTweenerComplete);
            inProgress = true;
        }


        public void OnTweenerComplete()
        {
            inProgress = false;
            Debug.Log("Tween Complete");
            Run();
        }
    }
}
