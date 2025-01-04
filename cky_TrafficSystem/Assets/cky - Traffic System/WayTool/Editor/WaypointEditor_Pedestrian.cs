﻿using System.Collections.Generic;
using ICON.Utilities;
using UnityEditor;
using UnityEngine;

namespace cky.TrafficSystem
{
    [CustomEditor(typeof(WaypointsContainer_Pedestrian))]

    public class WaypointEditor_Pedestrian : Editor
    {
        WaypointsContainer_Pedestrian wpScript;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            wpScript = (WaypointsContainer_Pedestrian)target;

            if (GUI.changed)
            {
                wpScript.RefreshAllWayPoints();
            }
        }


        void OnSceneGUI()
        {
            Event e = Event.current;
            wpScript = (WaypointsContainer_Pedestrian)target;

            if (e != null)
            {
                if (e.isMouse && e.shift && e.type == EventType.MouseDown)
                {

                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(ray, out hit, 5000.0f))
                    {
                        GetWaypoints();

                        Vector3 newTilePosition = hit.point;

                        GameObject wp = new GameObject(wpScript.name + " - " + (wpScript.waypoints.Count + 1).ToString("00"));

                        wp.transform.position = newTilePosition + Vector3.up * 0.1f;
                        wp.transform.SetParent(wpScript.transform);

                        GetWaypoints();

                        wpScript.RefreshAllWayPoints();
                    }

                    Selection.activeObject = wpScript.gameObject;
                }
                else if (e.isMouse && !e.shift && e.type == EventType.MouseDown)
                {
                    Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(ray, out hit, 5000.0f))
                    {

                        Vector3 newTilePosition = hit.point;

                        WaypointsContainer_Pedestrian[] tArray = GameObject.FindObjectsOfType<WaypointsContainer_Pedestrian>();

                        for (int f = 0; f < tArray.Length; f++)
                        {
                            if (tArray[f].transform.childCount > 1)
                            {

                                Transform[] points = tArray[f].transform.GetComponentsInChildren<Transform>();

                                for (int i = 0; i < points.Length; i++)
                                    if (Vector3.Distance(points[i].position, newTilePosition) < 1f)
                                    {
                                        Selection.activeObject = points[i];
                                        wpScript.RefreshAllWayPoints();
                                        return;
                                    }
                            }
                        }
                    }
                }

                if (wpScript)
                    Selection.activeGameObject = wpScript.gameObject;
            }
        }



        public void GetWaypoints()
        {
            wpScript.waypoints = new List<Transform>();

            Transform[] allTransforms = wpScript.transform.GetComponentsInChildren<Transform>();

            for (int i = 1; i < allTransforms.Length; i++)
            {


                allTransforms[i].name = wpScript.name + " - " + i.ToString("00");

                wpScript.waypoints.Add(allTransforms[i]);
                allTransforms[i].gameObject.SetIcon(LabelIcon.Yellow);

            }
            wpScript.WaypointsSetAngle();
        }
    }
}