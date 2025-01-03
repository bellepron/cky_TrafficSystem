using UnityEditor;
using UnityEngine;

namespace FCG.Pedestrian
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

                myScript.LoadPedestrians();
            }
        }
    }
}
