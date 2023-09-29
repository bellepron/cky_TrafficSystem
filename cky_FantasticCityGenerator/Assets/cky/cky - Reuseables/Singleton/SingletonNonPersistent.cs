using UnityEngine;

namespace cky.Reuseables.Singleton
{
    public class SingletonNonPersistent<T> : MonoBehaviour where T : class
    {
        private static T _instance;
        private void Awake()
        {
            if (Instance == null)
            {
                _instance = GetComponent<T>();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        public static T Instance
        {
            get => (T)_instance;
        }
    }
}