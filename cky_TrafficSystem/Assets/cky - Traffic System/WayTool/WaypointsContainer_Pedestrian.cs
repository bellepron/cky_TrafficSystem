using System.Collections;
using UnityEngine;
using UnityEditor;

namespace cky.TrafficSystem
{
    public class WaypointsContainer_Pedestrian : WaypointsContainer_Abstract
    {
        public bool noUnit;

        [HideInInspector] public WpData_Pedestrian wpData;

        public override void NextWaysCloseOnly()
        {
            for (int idx = 1; idx >= 0; idx--)
            {

                if (oneway && !doubleLine && idx == 0)
                    continue;

                if (directConnectSide[idx])
                    continue;

                int n = wpData.tf01.Length;

                if (n < 1)
                    continue;

                Vector3 referencia = Node(idx, waypoints.Count - 1);

                for (int i = 0; i < n; i++)
                {
                    if (!wpData.tsActive[i])
                        continue;

                    float pathDistance = Vector3.Distance(referencia, wpData.tf01[i]);
                    if (pathDistance < 3)
                    {

                        if (TestDoNotConnectTo(wpData.tsParent[i]) || wpData.tsParent[i].TestDoNotConnectTo(this))
                            continue;

                        if (wpData.tsOneway[i] != oneway || wpData.tsOnewayDoubleLine[i] != doubleLine || wpData.tsParent[i].transform == transform)
                            continue;

                        if (idx == 0)
                        {
                            nextWay0 = new WaypointsContainer_Abstract[1];
                            nextWaySide0 = new int[1];
                            nextWay0[0] = wpData.tsParent[i];
                            nextWaySide0[0] = wpData.tsSide[i];
                        }
                        else
                        {
                            nextWay1 = new WaypointsContainer_Abstract[1];
                            nextWaySide1 = new int[1];
                            nextWay1[0] = wpData.tsParent[i];
                            nextWaySide1[0] = wpData.tsSide[i];
                        }

                        directConnectSide[idx] = wpData.tsParent[i];

                        if (oneway)
                            wpData.tsParent[i].directConnectSide[0] = this;

                        break;
                    }
                }
            }
        }

        public override void NextWays()
        {
            for (int idx = 1; idx >= 0; idx--)
            {

                if (oneway && !doubleLine && idx == 0)
                    continue;

                if (directConnectSide[idx] != null)
                    continue;


                if (idx == 0)
                {
                    nextWay0 = new WaypointsContainer_Abstract[0];
                    nextWaySide0 = new int[0];
                }
                else
                {
                    nextWay1 = new WaypointsContainer_Abstract[0];
                    nextWaySide1 = new int[0];
                }

                Vector3 referencia = Node(idx, waypoints.Count - 1);

                ArrayList arrParent = new ArrayList();
                ArrayList arrSide = new ArrayList();

                int n = wpData.tf01.Length;

                if (n < 2)
                    continue;

                arrParent.Clear();
                arrSide.Clear();

                for (int i = 0; i < n; i++)
                {
                    if (!wpData.tsActive[i])
                        continue;

                    float pathDistance = Vector3.Distance(referencia, wpData.tf01[i]);

                    if (pathDistance < limitNodeDistance && pathDistance >= 3)
                    {
                        if (!wpData.tsParent[i])
                            continue;

                        if ((wpData.tsOneway[i] && !wpData.tsOnewayDoubleLine[i]) && (wpData.tsSide[i] == 0))
                            continue;

                        if ((wpData.tsOneway[i] && !wpData.tsOnewayDoubleLine[i]) && (wpData.tsParent[i].directConnectSide[0] != null))
                            continue;

                        if (TestDoNotConnectTo(wpData.tsParent[i]) || wpData.tsParent[i].TestDoNotConnectTo(this))
                            continue;

                        if (!wpData.tsOneway[i] && wpData.tsParent[i].directConnectSide[(wpData.tsSide[i] == 1) ? 0 : 1] != null)
                            continue;

                        if (wpData.tsParent[i].transform == transform)
                            continue;

                        WaypointsContainer_Pedestrian wpc = wpData.tsParent[i];

                        //Link this path with the nearby paths
                        //If the two ends of the path are close, stay with the closest one

                        if (Vector3.Distance(referencia, wpc.Node((wpData.tsSide[i] == 1) ? 0 : 1, 0)) > pathDistance || wpData.tsOneway[i])
                            if (Vector3.Distance(Node((idx == 1) ? 0 : 1, waypoints.Count - 1), wpData.tf01[i]) > pathDistance || oneway)
                            {
                                arrParent.Add(wpData.tsParent[i]);
                                arrSide.Add(wpData.tsSide[i]);

                            }
                    }

                }

                int qt = arrParent.Count;

                if (qt < 1)
                    continue;

                WaypointsContainer_Pedestrian[] _NextWays = new WaypointsContainer_Pedestrian[qt];
                int[] _NextWaysSide = new int[qt];

                for (int i = 0; i < qt; i++)
                {
                    _NextWays[i] = (WaypointsContainer_Pedestrian)arrParent[i];
                    _NextWaysSide[i] = (int)arrSide[i];
                }

                if (idx == 0)
                {
                    nextWay0 = _NextWays;
                    nextWaySide0 = _NextWaysSide;
                }
                else
                {
                    nextWay1 = _NextWays;
                    nextWaySide1 = _NextWaysSide;
                }

            }


            widthToUse = (oneway && !doubleLine) ? 0f : Mathf.Abs(width);

            //Block path that has no exit
            bloked = ((!oneway && (nextWay0.Length < 1 || nextWay1.Length < 1)) || (oneway && (nextWay0.Length < 1 && nextWay1.Length < 1)));   // If one of my ends is not linked to another route, ban me
        }

        public override void RefreshAllWayPoints()
        {
            if (Time.time - _timer < 0.2f) return;
            _timer = Time.time;

            if (!trafficSystem)
            {
                trafficSystem = FindObjectOfType<TrafficSystem_Pedestrian>();

#if UNITY_EDITOR
                if (!trafficSystem)
                    trafficSystem = (TrafficSystem_Pedestrian)AssetDatabase.LoadAssetAtPath("Assets/cky - Traffic System/Resources/Traffic System/Traffic System - Pedestrian.prefab", (typeof(TrafficSystem_Car)));
#endif

                if (!trafficSystem)
                    Debug.LogError("Traffic System - Pedestrian.prefab was not found in 'Assets/cky - Traffic System/Resources/Traffic System'");
            }

            trafficSystem.UpdateAllWayPoints();
        }
    }

}