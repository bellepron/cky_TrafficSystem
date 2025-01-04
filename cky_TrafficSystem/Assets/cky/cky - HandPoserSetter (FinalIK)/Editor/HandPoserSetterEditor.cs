using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HandPoserSetter))]
public class HandPoserSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HandPoserSetter posRotSetterTool = (HandPoserSetter)target;

        GUILayout.Space(25);
        if (GUILayout.Button("Save"))
        {
            posRotSetterTool.Save();
        }
        GUILayout.Space(25);
        if (GUILayout.Button("Load"))
        {
            posRotSetterTool.Load();
        }
        GUILayout.Space(25);

        EditorUtility.SetDirty(target);
    }
}