using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindFirstObjectByType(typeof(T));

                        if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                        {
                            Debug.LogError($"[Singleton] Something went really wrong - there should never be more than 1 singleton! Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = $"{typeof(T)} (Singleton)";
                        }
                        else
                        {
                            Debug.Log($"[Singleton] Using instance already created: {_instance.gameObject.name}");
                        }
                    }
                }
            }

            return _instance;
        }
    }

    protected virtual void OnDestroy()
    {
        // Herhangi bir ek temizlik iþlemi
    }
}
