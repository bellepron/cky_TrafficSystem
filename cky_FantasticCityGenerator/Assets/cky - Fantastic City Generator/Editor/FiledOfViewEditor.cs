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

            var c0 = fow.transform.position + fow.transform.up;
            var viewRadius = fow.Settings.viewRadius;
            var viewAngle = fow.Settings.viewAngle;

            Handles.DrawWireArc(c0, Vector3.up, Vector3.forward, 360, viewRadius);
            Vector3 viewAngleA = fow.DirFromAngle(-viewAngle * 0.5f, false);
            Vector3 viewAngleB = fow.DirFromAngle(viewAngle * 0.5f, false);

            Handles.DrawLine(c0, c0 + viewAngleA * viewRadius);
            Handles.DrawLine(c0, c0 + viewAngleB * viewRadius);
        }

    }
}