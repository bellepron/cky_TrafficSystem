using UnityEngine;
using UnityEditor;

namespace cky.TrafficSystem
{
    [CustomEditor(typeof(TrafficSystem_Car))]
    public class CarTrafficSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TrafficSystem_Car myScript = (TrafficSystem_Car)target;

            // Normal özellikleri çizdirin
            DrawDefaultInspector();

            GUILayout.Space(30);
            if (GUILayout.Button("Create"))
            {
                DestroyImmediate(GameObject.Find("CarContainer"));

                myScript.LoadCars();
            }
        }
    }
}
