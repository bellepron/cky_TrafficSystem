#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.IO;

namespace cky.DataSaving
{
    public abstract class ScriptableObjectSaverAbstract : ScriptableObject
    {
        public string saveName;

        public bool IsSaveFile()
        {
            return File.Exists(GetSavePath());
        }

        public void Save()
        {
            string savePath = GetSavePath();

            if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            }

            var json = JsonUtility.ToJson(this);
            File.WriteAllText(savePath, json);
            Debug.Log($"Data saved to {savePath}");

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void Load()
        {
            string savePath = GetSavePath();

            if (File.Exists(savePath))
            {
                var file = File.ReadAllText(savePath);
                JsonUtility.FromJsonOverwrite(file, this);
                Debug.Log($"Data loaded from {savePath}");

#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
            else
            {
                //Debug.LogWarning($"Save file not found at {savePath}");
                Save();
            }
        }

        private string GetSavePath()
        {
            return Path.Combine(Application.persistentDataPath, "game_save", "game_data", $"{saveName}.txt");
        }

        public abstract void SetDefaults();
    }
}