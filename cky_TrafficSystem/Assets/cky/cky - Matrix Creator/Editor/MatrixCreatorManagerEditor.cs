using UnityEditor;
using UnityEngine;

namespace cky.MatrixCreation
{
    [CustomEditor(typeof(MatrixCreatorManager))]
    public class MatrixCreatorManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MatrixCreatorManager mcm = (MatrixCreatorManager)target;

            GUILayout.Space(75);
            if (GUILayout.Button("Matrix Set"))
            {
                mcm.MatrixSet();
            }
            GUILayout.Space(75);

            if (GUILayout.Button("Place Random"))
            {
                mcm.PlaceRandom();
            }
            GUILayout.Space(75);
        }
    }
}