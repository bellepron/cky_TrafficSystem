using UnityEngine;

namespace cky.Managers
{
    public class GameManager : MonoBehaviour
    {
        public GameSettings gameSettings;

        private void Awake()
        {
            gameSettings.Load();
        }


    }
}