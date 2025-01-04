//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(HandPoserController))]
//public class HandPoserControllerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        HandPoserController posRotSetterTool = (HandPoserController)target;

//        GUILayout.Space(25);
//        if (GUILayout.Button("Set"))
//        {
//            posRotSetterTool.Set();
//        }

//        EditorUtility.SetDirty(target);
//    }
//}