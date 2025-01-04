using UnityEngine;
using UnityEditor;

namespace cky.Tools
{
    [CustomEditor(typeof(PosRotSetterTool))]
    public class PosRotSetterToolEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            PosRotSetterTool posRotSetterTool = (PosRotSetterTool)target;

            GUILayout.Space(25);
            if (GUILayout.Button("Set Position and Rotation"))
            {
                posRotSetterTool.Set();
            }
            GUILayout.Space(25);
        }
    }
}
