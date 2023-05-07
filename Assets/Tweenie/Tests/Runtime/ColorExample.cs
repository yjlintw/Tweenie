using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YJL.Tween;

public class ColorExample : MonoBehaviour
{
    private Renderer _renderer;
    // Start is called before the first frame update
    void Start()
    {

        _renderer = GetComponent<Renderer>();
        if (_renderer == null) return;
        Tweenie.To(x => _renderer.material.color = x, Color.white, new Color(0,0,0,0), 1)
            .SetEase(Ease.Linear)
            .SetLoop(Loop.Default, -1);
    }
}
