using UnityEngine;
using Random = UnityEngine.Random;

namespace YJL.Tween.Test
{
    public class Tester : MonoBehaviour
    {
        public float duration;
        private ITweener _tweener;


        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (_tweener == null)
                {
                    _tweener = Run();
                }
                else
                {
                    _tweener?.Play();
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _tweener?.SetLoop(Loop.PingPong, 2).Play();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _tweener?.SetLoop(Loop.Default).Play();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _tweener?.SetLoopCount(0).Play();
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                _tweener?.Complete();
            }

            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                _tweener?.Pause();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                _tweener?.Stop();
                _tweener = null;
            }
        }

        public ITweener Run()
        {
            Vector3 startPos = transform.position;
            Vector3 origin = new Vector3(startPos.x, startPos.y, startPos.z - 5f);
            return Tweenie.To(x =>
                    {
                        Vector3 newPos = startPos;
                        newPos.x = origin.x + 5f * Mathf.Sin(x);
                        newPos.z = origin.z + 5f * Mathf.Cos(x);
                        transform.position = newPos;
                    }, 
                0, 
                Mathf.PI, 
                duration)
                .SetEase(Ease.Linear)
                .SetLoop(Loop.Default)
                .OnStart(() => Debug.Log("Tween Start"))
                .OnPause(() => Debug.Log("Tween Pause"))
                .OnStepComplete(() => Debug.Log("Tween Step Complete"))
                .OnComplete(() => Debug.Log("Tween Complete"))
                .OnStop(() => Debug.Log("Tween Stop"));
        }
    }
}
