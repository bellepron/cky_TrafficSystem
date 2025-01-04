using cky.Reuseables.Singleton;
using cky.Reuseables.Helpers;
using cky.Reuseables.Level;
using UnityEngine;

namespace cky.Reuseables.Managers
{
    public class GameManagerAbstract : SingletonPersistent<GameManagerAbstract>
    {
        [SerializeField] LevelSettings[] levels;
        public LevelSettings levelSettings;

        int _levelIndex;

        protected override void OnPerAwake()
        {
            _levelIndex = PlayerPrefs.GetInt(PlayerPrefHelper.pPrefsLevelIndex);
            levelSettings = levels[_levelIndex % levels.Length];
        }

        protected void OnGameSuccess()
        {
            _levelIndex++;
            PlayerPrefs.SetInt(PlayerPrefHelper.pPrefsLevelIndex, _levelIndex);
        }
    }
}