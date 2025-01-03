using UnityEditor;
using UnityEngine;

namespace cky.DataSaving
{
    [CustomEditor(typeof(ScriptableObjectSaverAbstract))]
    public abstract class ScriptableObjectSaverAbstractEditor<T> : Editor where T : ScriptableObjectSaverAbstract
    {
        public override void OnInspectorGUI()
        {
            T script = (T)target;

            if (GUILayout.Button("Save"))
            {
                script.Save();
            }

            if (GUILayout.Button("Set Defaults"))
            {
                script.SetDefaults();
            }

            if (GUILayout.Button("Load"))
            {
                script.Load();
            }

            GUILayout.Space(20);
            DrawDefaultInspector();
        }
    }
}