using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YJL.Tween;
using Random = UnityEngine.Random;

public class Vector3Example : MonoBehaviour
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
        _targetPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        transform.MoveTo(_targetPosition, 5).SetEase(Ease.EaseInOut).SetLoop(Loop.PingPong);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.2f, 1.0f, 0.5f);
        Gizmos.DrawCube(_targetPosition, Vector3.one);
        Gizmos.color = new Color(1.0f, 0.2f, 0.2f, 0.5f);
        Gizmos.DrawCube(_startPosition, Vector3.one);
    }
}
