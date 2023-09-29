using UnityEngine;
using UnityEditor;

namespace cky.FCG.Pedestrian.StateMachine
{
    [CustomEditor(typeof(PedestrianStateMachine))]
    public class FieldOfViewEditor : Editor
    {

        void OnSceneGUI()
        {
            PedestrianStateMachine fow = (PedestrianStateMachine)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position + fow.transform.up, Vector3.up, Vector3.forward, 360, fow.Settings.viewRadius);
            Vector3 viewAngleA = fow.DirFromAngle(-fow.Settings.viewAngle * 0.5f, false);
            Vector3 viewAngleB = fow.DirFromAngle(fow.Settings.viewAngle * 0.5f, false);
            Handles.DrawLine(fow.transform.position + fow.transform.up, fow.transform.position + fow.transform.up + viewAngleA * fow.Settings.viewRadius);
            Handles.DrawLine(fow.transform.position + fow.transform.up, fow.transform.position + fow.transform.up + viewAngleB * fow.Settings.viewRadius);
        }

    }
}