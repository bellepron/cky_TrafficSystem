using UnityEngine;

namespace cky.Reuseables.Singleton
{
    public class SingletonPersistent<T> : MonoBehaviour where T : class
    {
        private static T _instance;
        private void Awake()
        {
            OnPerAwake();

            if (_instance == null)
            {
                _instance = GetComponent<T>();
                DontDestroyOnLoad(gameObject);
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

        protected virtual void OnPerAwake() { }

        private void OnApplicationQuit()
        {
            Destroy(gameObject);
        }
    }
}