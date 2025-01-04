using UnityEditor;
using UnityEngine;

namespace cky.Changer.WallChange
{
    [CustomEditor(typeof(WallCreator))]
    public class WallCreatorEditor : Editor
    {
        //public override void OnInspectorGUI()
        //{
        //    DrawDefaultInspector();

        //    var script = (WallCreator)target;

        //    GUILayout.Space(15);
        //    if (GUILayout.Button("Create Wall"))
        //    {
        //        script.CreateWall();
        //    }
        //}
    }
}