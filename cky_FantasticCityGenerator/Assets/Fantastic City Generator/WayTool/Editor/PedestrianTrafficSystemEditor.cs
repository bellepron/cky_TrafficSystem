using UnityEditor;
using UnityEngine;

namespace FCG.Pedestrians
{
    [CustomEditor(typeof(PedestrianTrafficSystem))]
    public class PedestrianTrafficSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PedestrianTrafficSystem myScript = (PedestrianTrafficSystem)target;

            // Normal özellikleri çizdirin
            DrawDefaultInspector();

            GUILayout.Space(30);
            if (GUILayout.Button("Create"))
            {
                DestroyImmediate(GameObject.Find("PedestrianContainer"));

                myScript.LoadPedestrians(0);
            }
        }
    }
}
