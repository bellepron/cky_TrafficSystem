using UnityEditor;
using UnityEngine;

namespace cky.Changer.FloorChange
{
    [CustomEditor(typeof(FloorCreator))]
    public class FloorCreatorEditor : Editor
    {
        //public override void OnInspectorGUI()
        //{
        //    DrawDefaultInspector();

        //    var script = (FloorCreator)target;

        //    GUILayout.Space(15);
        //    if (GUILayout.Button("Create Floor"))
        //    {
        //        script.CreateFloor();
        //    }
        //}
    }
}