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
                _tweener = Run();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                _tweener?.SetLoop(Loop.PingPong).Play();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                _tweener?.SetLoop(Loop.Default).Play();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _tweener?.SetLoop(0).Play();
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
            return Tweenie.To(x =>
                    {
                        Vector3 newPos = startPos;
                        newPos.x = startPos.x + 5f * Mathf.Sin(x);
                        newPos.z = startPos.z + 5f * Mathf.Cos(x);
                        transform.position = newPos;
                    }, 
                0, 
                Mathf.PI * 2, 
                duration)
                .SetEase(Ease.Linear)
                .SetLoop(Loop.Default)
                .OnComplete(OnTweenerComplete);
        }
        

        public void OnTweenerComplete()
        {
            Debug.Log("Tween Complete");
        }
    }
}
