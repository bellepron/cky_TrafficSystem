using System.Collections.Generic;
using UnityEngine;

namespace cky.TrafficSystem
{
    public abstract class WaypointsContainer_Abstract : MonoBehaviour
    {
        [HideInInspector] public List<Transform> waypoints = new List<Transform>();

        protected TrafficSystem_Abstract trafficSystem;

        [HideInInspector] public WaypointsContainer_Abstract[] nextWay0;
        [HideInInspector] public WaypointsContainer_Abstract[] nextWay1;
        [HideInInspector] public WaypointsContainer_Abstract[] directConnectSide = new WaypointsContainer_Abstract[2];

        [HideInInspector] public int[] nextWaySide0;
        [HideInInspector] public int[] nextWaySide1;

        [HideInInspector] public bool bloked = false;

        [HideInInspector] public Transform nodeZero0;
        [HideInInspector] public Transform nodeZero1;
        [HideInInspector] public Transform nodeZeroWay0;
        [HideInInspector] public Transform nodeZeroWay1;

        public bool noUnit;
        public bool oneway = false;
        public bool doubleLine = false;
        [Range(0.5f, 25)] public float width = 2f;
        [Range(5, 100)] public float limitNodeDistance = 30;
        protected float widthToUse = 2f;
        public WaypointsContainer_Abstract[] doNotConnectTo;

        protected Vector3 nodeBegin;
        protected Vector3 nodeEnd;

        protected Vector3 oldPosition;

        protected float _forDistanceControl = 1;

        protected float _timer;



        public abstract void NextWaysCloseOnly();

        public void ResetWay()
        {
            nextWay0 = null;
            nextWay1 = null;

            nextWay0 = new WaypointsContainer_Abstract[0];
            nextWay1 = new WaypointsContainer_Abstract[0];

            directConnectSide[0] = null;
            directConnectSide[1] = null;

            width = Mathf.Abs(width);
        }

        public abstract void NextWays();

        public bool TestDoNotConnectTo(WaypointsContainer_Abstract t)
        {
            if (doNotConnectTo == null) return false;
            if (doNotConnectTo.Length == 0) return false;

            for (int d = 0; d < doNotConnectTo.Length; d++)
            {
                if (doNotConnectTo[d] == t)
                    return true;
            }

            return false;
        }

        public abstract void RefreshAllWayPoints();



        protected float GetAngulo180(Transform origem, Vector3 target)
        {
            return Vector3.Angle(target - origem.position, origem.forward);
        }

        public void InvertNodesDirection()
        {
            widthToUse = Mathf.Abs(width);
        }

        public int GetTotalNodes()
        {
            return waypoints.Count - 1;
        }

        public Transform GetNodeZeroUnit(int side)
        {
            return (side == 0) ? nodeZero0 : nodeZero1;
        }

        public Transform GetNodeZeroWay(int side)
        {
            return (side == 0) ? nodeZeroWay0 : nodeZeroWay1;
        }

        public virtual Transform GetNodeZeroOldWay(int side)
        {
            return (side == 0) ? nodeZero0.GetComponent<ITrafficSystemUnit>().MyOldWay : nodeZero1.GetComponent<ITrafficSystemUnit>().MyOldWay;
        }

        public bool SetNodeZero(int side, Transform nodeWay, Transform nodeUnit, bool force = false)
        {
            if (side == 0)
            {
                if (nodeZero0 == null || force)
                {
                    nodeZeroWay0 = nodeWay;
                    nodeZero0 = nodeUnit;
                }
                return nodeZero0 == nodeUnit;
            }
            else
            {
                if (nodeZero1 == null || force)
                {
                    nodeZeroWay1 = nodeWay;
                    nodeZero1 = nodeUnit;
                }
                return nodeZero1 == nodeUnit;
            }
        }

        public bool UnSetNodeZero(int side, Transform unitTransform, bool force = false)
        {
            if (side == 0)
            {
                if (nodeZero0 == unitTransform || force)
                {
                    nodeZeroWay0 = null;
                    nodeZero0 = null;
                }
                return nodeZero0 == null;
            }
            else
            {
                if (nodeZero1 == unitTransform || force)
                {
                    nodeZeroWay1 = null;
                    nodeZero1 = null;
                }
                return nodeZero1 == null;
            }
        }



        public Vector3 AvanceNode(int side, int idx, float mts = 1)
        {
            /*
            Returns a Vector3 that is a position in front of the specified node.
            The value is specified in the mts parameter. This value can be positive or negative.
            */

            if ((!oneway && side == 0) /*|| (oneway && rightHand != 0)*/)
            {
                int i = (waypoints.Count - 1) - idx;
                return waypoints[i].position - (waypoints[i].transform.forward * mts) - (waypoints[i].transform.right * ((doubleLine && side == 0) ? -widthToUse : widthToUse));
            }
            else
            {
                return waypoints[idx].position + (waypoints[idx].transform.forward * mts) + (waypoints[idx].transform.right * ((doubleLine && side == 0) ? -widthToUse : widthToUse));
            }
        }

        public Quaternion NodeRotation(int side, int idx)
        {
            if ((!oneway && side == 1) || (oneway /*&& rightHand == 0*/))
                return Quaternion.LookRotation(waypoints[idx + 1].position - waypoints[idx].position);
            else
            {
                int i = (waypoints.Count - 1) - idx;
                return Quaternion.LookRotation(waypoints[i - 1].position - waypoints[i].position);
            }
        }

        public Vector3 Node(int side, int idx, float nodeSteerCarefully = 0)
        {
            /*
             Returns a Vector3 referring to the position of the specified node
             Note that the real nodes are in the middle. The nodes that will actually be followed are relative positions, shown in red line (Gizmos)
            */

            if (oneway)
            {
                int i = idx;

                if (!doubleLine)
                    return waypoints[i].position;
                else
                    return waypoints[i].position + (((side == 1) ? (waypoints[i].transform.right * widthToUse) : (waypoints[i].transform.right * -widthToUse)));
            }
            else
            {
                int i = (side == 1) ? idx : (waypoints.Count - 1) - idx;

                if (nodeSteerCarefully > 0 && idx == 0)
                {
                    if (side == 1)
                        return (waypoints[i].position - (waypoints[i].transform.forward * nodeSteerCarefully)) + (waypoints[i].transform.right * ((side == 1) ? widthToUse : -widthToUse));
                    else
                        return (waypoints[i].position + (waypoints[i].transform.forward * nodeSteerCarefully)) + (waypoints[i].transform.right * ((side == 1) ? widthToUse : -widthToUse));
                }
                else
                {
                    if (i >= waypoints.Count)
                    {
                        Debug.Log($"AAAAAAA {i} / {waypoints.Count}");
                    }
                    else if (i < 0)
                    {
                        Debug.Log($"AAAAAAA {i} / EKSIII");
                    }
                    return waypoints[i].position + waypoints[i].transform.right * ((side == 1) ? widthToUse : -widthToUse);
                }
            }
        }

        public void GetWaypoints()
        {
            waypoints = new List<Transform>();

            Transform[] allTransforms = transform.GetComponentsInChildren<Transform>();

            for (int i = 1; i < allTransforms.Length; i++)
            {
                //TIRAR
                //allTransforms[i].name = this.name + " - " + i.ToString("00");
                waypoints.Add(allTransforms[i]);
            }

            WaypointsSetAngle();
        }


        public void WaypointsSetAngle()
        {
            int wCount = waypoints.Count;

            if (wCount > 1)
            {
                waypoints[0].LookAt(waypoints[1]);

                for (int i = 1; i < wCount - 1; i++)
                    waypoints[i].rotation = Quaternion.LookRotation(waypoints[i + 1].position - waypoints[i - 1].position);

                var ckyDir = waypoints[wCount - 1].position - waypoints[wCount - 2].position;
                if (ckyDir != Vector3.zero)
                    waypoints[wCount - 1].rotation = Quaternion.LookRotation(ckyDir);
            }
        }

        public bool BookNodeZero(TrafficCar trafficCar)
        {
            if (trafficCar.SideAtual == 0)
            {
                if (SetNodeZero(0, trafficCar.MyOldWay, trafficCar.transform))
                    return true;
                else
                {
                    if (trafficCar.NodeSteerCarefully2 == false && trafficCar.NodeSteerCarefully == false && nodeZero0.GetComponent<TrafficCar>().Get_avanceNode() && trafficCar.GetBehind() != nodeZero0)
                    {
                        SetNodeZero(0, trafficCar.MyOldWay, trafficCar.transform, true);
                    }

                    //The starting node of the path is already reserved for another car. So the car that called this procedure must wait.
                    return ((nodeZero0 == trafficCar.transform) || (nodeZeroWay0 == trafficCar.MyOldWay && trafficCar.MyOldSideAtual == nodeZero0.GetComponent<TrafficCar>().MyOldSideAtual));
                }
            }
            else
            {
                if (SetNodeZero(1, trafficCar.MyOldWay, trafficCar.transform))
                    return true;
                else
                {
                    if (trafficCar.NodeSteerCarefully == false && nodeZero1.GetComponent<TrafficCar>().Get_avanceNode() && trafficCar.GetBehind() != nodeZero1)
                    {
                        SetNodeZero(1, trafficCar.MyOldWay, trafficCar.transform, true);
                    }

                    //The starting node of the path is already reserved for another car. So the car that called this procedure must wait.                
                    return ((nodeZero1 == trafficCar.transform) || (nodeZeroWay1 == trafficCar.MyOldWay && trafficCar.MyOldSideAtual == nodeZero1.GetComponent<TrafficCar>().MyOldSideAtual));
                }
            }
        }



#if UNITY_EDITOR

        void OnDrawGizmos()
        {
            if (!oneway) doubleLine = false;

            bool isPlay = Application.isPlaying;
            int wCount = waypoints.Count;


            if (transform.childCount != waypoints.Count)
            {
                RefreshAllWayPoints();
                return;
            }

            WaypointsSetAngle();

            if (transform.childCount < 1) return;

            if (!isPlay && wCount > 1)
            {
                if ((waypoints[wCount - 1].localPosition != nodeEnd) || (waypoints[0].localPosition != nodeBegin))
                {
                    if (UnityEditor.Selection.activeGameObject)
                        if (UnityEditor.Selection.activeGameObject.transform.parent == this.transform)
                            RefreshAllWayPoints();
                }

                nodeBegin = waypoints[0].localPosition;
                nodeEnd = waypoints[wCount - 1].localPosition;
            }

            widthToUse = (oneway && !doubleLine) ? 0.1f : Mathf.Abs(width);

            for (int i = 0; i < wCount; i++)
            {
                Gizmos.color = new Color(0.0f, 0.7f, 0.7f, 1.0f);
                Gizmos.DrawSphere(waypoints[i].transform.position, 0.6f);

                if (wCount < 2) return;

                if (i < wCount - 1)
                {
                    if (oneway && !doubleLine)
                        Gizmos.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);

                    //central line
                    Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);


                    Vector3 offset = waypoints[i].transform.right * widthToUse;
                    Vector3 offsetTo = waypoints[i + 1].transform.right * widthToUse;

                    // White line
                    if (i == 0)
                    {
                        Gizmos.color = Gizmos.color = new Color(1f, 1f, 1f, 0.7f);
                        for (int t = 0; t < nextWay0.Length; t++)
                            //if (!oneway || rightHand != 0) // rightHand=0 olduðu için hep true cky.
                            Gizmos.DrawLine(Node(0, wCount - 1), nextWay0[t].Node(nextWaySide0[t], 0));

                        if (!isPlay)
                        {
                            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

                            if (oneway)
                            {
                                //int m = (rightHand == 0) ? -1 : 1; // rightHand=0 olduðu için hep true cky.
                                int m = -1;
                                if (doubleLine)
                                {
                                    Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position + (waypoints[i].right * widthToUse * 0.8f) + waypoints[i].forward * m);
                                    Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position + (waypoints[i].right * widthToUse * 1.2f) + waypoints[i].forward * m);
                                    Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i].position - (waypoints[i].right * widthToUse * 0.8f) + waypoints[i].forward * m);
                                    Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i].position - (waypoints[i].right * widthToUse * 1.2f) + waypoints[i].forward * m);
                                }
                                else
                                {
                                    Gizmos.DrawLine(waypoints[i].position, waypoints[i].position + (waypoints[i].right * 0.4f) + waypoints[i].forward * m);
                                    Gizmos.DrawLine(waypoints[i].position, waypoints[i].position - (waypoints[i].right * 0.4f) + waypoints[i].forward * m);
                                }
                            }
                            else
                            {
                                Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position + (waypoints[i].right * widthToUse * 0.8f) + waypoints[i].forward * -1);
                                Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position + (waypoints[i].right * widthToUse * 1.2f) + waypoints[i].forward * -1);
                                Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i].position - (waypoints[i].right * widthToUse * 0.8f) + waypoints[i].forward * 1);
                                Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i].position - (waypoints[i].right * widthToUse * 1.2f) + waypoints[i].forward * 1);
                            }
                        }
                    }

                    if (!oneway || doubleLine)
                    {
                        Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                        Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position - offset);

                        Gizmos.color = new Color(1.0f, (oneway && doubleLine) ? 0.5f : 0.0f, 0.0f, 0.9f);

                        Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i + 1].position + offsetTo);
                        Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i + 1].position - offsetTo);
                    }
                }
                else
                {
                    // Last node

                    Vector3 offset = waypoints[i].transform.right * widthToUse;

                    Gizmos.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
                    Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position - offset);

                    if (!isPlay)
                    {
                        Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

                        if (oneway)
                        {
                            //int m = (rightHand == 0) ? -1 : 1;
                            int m = -1; // rightHand=0 olduðu için hep true cky.
                            if (doubleLine)
                            {
                                Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position + (waypoints[i].right * widthToUse * 0.8f) + waypoints[i].forward * m);
                                Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position + (waypoints[i].right * widthToUse * 1.2f) + waypoints[i].forward * m);
                                Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i].position - (waypoints[i].right * widthToUse * 0.8f) + waypoints[i].forward * m);
                                Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i].position - (waypoints[i].right * widthToUse * 1.2f) + waypoints[i].forward * m);
                            }
                            else
                            {
                                Gizmos.DrawLine(waypoints[i].position, waypoints[i].position + (waypoints[i].right * 0.4f) + waypoints[i].forward * m);
                                Gizmos.DrawLine(waypoints[i].position, waypoints[i].position - (waypoints[i].right * 0.4f) + waypoints[i].forward * m);
                            }
                        }
                        else
                        {
                            Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position + (waypoints[i].right * widthToUse * 0.8f) + waypoints[i].forward * -1);
                            Gizmos.DrawLine(waypoints[i].position + offset, waypoints[i].position + (waypoints[i].right * widthToUse * 1.2f) + waypoints[i].forward * -1);
                            Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i].position - (waypoints[i].right * widthToUse * 0.8f) + waypoints[i].forward * 1);
                            Gizmos.DrawLine(waypoints[i].position - offset, waypoints[i].position - (waypoints[i].right * widthToUse * 1.2f) + waypoints[i].forward * 1);
                        }
                    }

                    Gizmos.color = Gizmos.color = new Color(1f, 1f, 1f, 0.7f);

                    for (int t = 0; t < nextWay1.Length; t++)
                    {
                        Gizmos.DrawLine(Node(1, i), nextWay1[t].Node(nextWaySide1[t], 0));

                        if (doubleLine)
                            Gizmos.DrawLine(Node(0, i), nextWay1[t].Node(nextWaySide1[t], 0));
                    }
                }
            }

            /*
            Gizmos.color = Color.cyan;

            if (rightHand == 0 && nodeZeroCar1)
                Gizmos.DrawLine(Node(1,0) , nodeZeroCar1.position);
            else if (rightHand != 0 && nodeZeroCar0)
                Gizmos.DrawLine(Node(0, 0), nodeZeroCar0.position);

            if (rightHand == 0 && nodeZeroCar0)
                Gizmos.DrawLine(Node(0,0), nodeZeroCar0.position);
            else if (rightHand != 0 && nodeZeroCar1)
                Gizmos.DrawLine(Node(1,0), nodeZeroCar1.position);
            */
        }

#endif
    }
}