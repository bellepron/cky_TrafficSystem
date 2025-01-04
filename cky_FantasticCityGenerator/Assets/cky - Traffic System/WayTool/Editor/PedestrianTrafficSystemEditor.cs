using UnityEngine;
using UnityEditor;

namespace cky.TrafficSystem
{
    [CustomEditor(typeof(TrafficSystem_Pedestrian))]
    public class PedestrianTrafficSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TrafficSystem_Pedestrian myScript = (TrafficSystem_Pedestrian)target;

            // Normal özellikleri çizdirin
            DrawDefaultInspector();

            GUILayout.Space(30);
            if (GUILayout.Button("Create"))
            {
                DestroyImmediate(GameObject.Find("PedestrianContainer"));

                myScript.LoadPedestrians();
            }
        }
    }
}
