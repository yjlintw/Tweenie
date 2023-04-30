using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            Init();
            return _instance;
        }
    }

    public static void Init()
    {
        if (_instance == null)
        {
            // find generic instance
            _instance = FindObjectOfType<T>();
            
            // if it is null again, create a new one
            if (_instance == null)
            {
                GameObject obj = new GameObject();
                obj.name = typeof(T).Name;
                _instance = obj.AddComponent<T>();
            }
        }
    }

    public void Awake()
    {
        // create the instance
        if (_instance == null)
        {
            _instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
