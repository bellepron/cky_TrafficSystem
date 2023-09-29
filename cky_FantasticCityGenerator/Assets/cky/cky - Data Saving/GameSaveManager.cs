using System.IO;
using UnityEngine;
//using cky.PlayerPad;

namespace cky.DataSaving
{
    [System.Serializable]
    public class GameSaveManager : MonoBehaviour
    {
        //PlayerPropertyData _playerPropertyData;

        //public void Initialize(PlayerPropertyData playerPropertyData)
        //{
        //    _playerPropertyData = playerPropertyData;
        //}

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                SaveGame();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                LoadGame();
            }
        }


        public bool IsSaveFile()
        {
            return Directory.Exists(Application.persistentDataPath + "game_save");
        }

        public void SaveGame()
        {
            if (!IsSaveFile())
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/game_save");
            }
            if (!Directory.Exists(Application.persistentDataPath + "/game_save/game_data"))
            {
                Directory.CreateDirectory(Application.persistentDataPath + "/game_save/game_data");
            }

            Debug.Log("Game Saved");
            //var json = JsonUtility.ToJson(_playerPropertyData);
            //File.WriteAllText(Application.persistentDataPath + "/game_save/game_data/game_save.txt", json);
        }

        public void LoadGame()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/game_save/game_data"))
            {
                SaveGame();
            }

            if (File.Exists(Application.persistentDataPath + "/game_save/game_data/game_save.txt"))
            {
                var file = File.ReadAllText(Application.persistentDataPath + "/game_save/game_data/game_save.txt");
                //JsonUtility.FromJsonOverwrite((string)file, _playerPropertyData);
            }

            Debug.Log("Game Loaded");
        }
    }
}