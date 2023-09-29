using UnityEngine;
using UnityEngine.AI;

namespace FCG.Pedestrians
{
    public class TrafficPedestrian : MonoBehaviour
    {
        //public enum StatusPedestrian
        //{
        //    transitingNormally,
        //    waitingForAnotherVehicleToPass,
        //    stoppedAtTrafficLights,
        //    bloked, // It's on a dead end street, stand still
        //    Undefined,
        //    crashed,
        //}

        [SerializeField] private TrafficPedestrianSettings _settings;

        NavMeshAgent _agent;

        //public StatusPedestrian status;

        private Vector3 mRayCenter;

        private int countWays;
        private Transform[] nodes;

        [HideInInspector]
        public int currentNode = 0;

        private float distanceToNode;


        private float speed;

        private Vector3 relativeVector;

        private float timeStoped;

        private Transform thisTr;
        private float iRC = 0;
        private float brake2 = 0;

        [HideInInspector]
        public Transform atualWay;

        [HideInInspector]
        public int sideAtual = 0;

        [HideInInspector]
        public FCGPedestrianWaypointsContainer atualWayScript;

        [HideInInspector]
        public bool nodeSteerCarefully = false;     //true if I'm turning to the side that has opposite traffic (Right hand and turning left) Or (Left hand and turning right)

        [HideInInspector]
        public bool nodeSteerCarefully2 = false;    // For double line oneway

        [HideInInspector]
        public Transform myOldWay;

        [HideInInspector]
        public int myOldSideAtual = 0;

        [HideInInspector]
        public FCGPedestrianWaypointsContainer myOldWayScript = null;

        private Vector3 _avanceNode = Vector3.zero; //private Position where an additional and momentary node can be added

        private Transform behind = null;

        //[HideInInspector]
        public Transform player;
        private Transform cameraTr;
        public Vector3 agentHeight = new Vector3(0, 2.0f, 0);

        //[HideInInspector]
        public PedestrianTrafficSystem tSystem;

        //[HideInInspector]
        public float distanceToSelfDestroy = 0; //0 = Do not autodestroy with player distance

        public float curveAdjustment = 0.0f;


        public void Configure()
        {
            //    
            float p = transform.localPosition.z + 0.6f;
            float l = transform.localPosition.x;

            //    
            Transform testC = new GameObject("RayTest").transform;
            testC.SetParent(transform);

            testC.localPosition = new Vector3(0, 0f, transform.localPosition.z + 4f);
            testC.LookAt(transform);
            testC.position += new Vector3(0, 0.8f, 0);

            if (Physics.Raycast(testC.position, testC.forward, out RaycastHit hit, 4))
            {
                Debug.DrawRay(testC.position, testC.forward * 4, Color.red);
            }
            else
            {
                Debug.LogError("Adicione um collider e então tente novamente");
            }

            DestroyImmediate(testC.gameObject);
        }


        void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            cameraTr = Camera.main.transform;

            timeStoped = Time.time;

            thisTr = transform;
            thisTr.SetParent(transform);
            thisTr.localRotation = Quaternion.identity;


            if (player && !FindObjectOfType<PedestrianTrafficSystem>())
                Debug.LogError("The Pedestrian Traffic System.prefab not found in the Hierarchy");


            if (atualWay)
                Init();
        }


        public void Init()
        {
            atualWayScript = atualWay.GetComponent<FCGPedestrianWaypointsContainer>();

            DefineNewPath();

            if (currentNode == 0) currentNode = 1;

            distanceToNode = Vector3.Distance(atualWayScript.Node(sideAtual, currentNode), thisTr.position + thisTr.forward * (curveAdjustment * 0.5f));

            InvokeRepeating(nameof(Move), 0.02f, 0.02f);

            //status = StatusPedestrian.transitingNormally;
        }

        public void ActivateSelfDestructWhenAwayFromThePlayer()
        {
            if (tSystem && player)
            {
                if (distanceToSelfDestroy == 0) distanceToSelfDestroy = _settings.distanceToSelfDestroyDefault;
                InvokeRepeating(nameof(SelfDestructWhenAwayFromThePlayer), 5f, _settings.checkingAwayFromPlayerRepeatRate);
            }
        }


        public float GetSpeed()
        {
            return speed;
        }

        public bool Get_avanceNode()
        {
            return (currentNode == 0 && nodeSteerCarefully && _avanceNode != Vector3.zero);
        }


        public Vector3 GetNodePosition()
        {
            return atualWayScript.Node(sideAtual, currentNode);
        }

        bool CheckBookAllPathOptions(FCGPedestrianWaypointsContainer wayScript, int side)
        {
            int total;
            int wSide;
            FCGPedestrianWaypointsContainer wScript;

            total = (side == 0) ? wayScript.nextWay0.Length : wayScript.nextWay1.Length;

            for (int i = 0; i < total; i++)
            {

                if (side == 0)
                {
                    wScript = wayScript.nextWay0[i];
                    wSide = wayScript.nextWaySide0[i];

                }
                else
                {
                    wScript = wayScript.nextWay1[i];
                    wSide = wayScript.nextWaySide1[i];
                }

                if (wScript)
                {
                    if ((wScript.GetNodeZeroCar(wSide) != null && wScript.GetNodeZeroCar(wSide) != transform) && wScript.GetNodeZeroOldWay(wSide) != myOldWay && (!Get_avanceNode() || !wScript.GetNodeZeroCar(wSide).GetComponent<TrafficPedestrian>().Get_avanceNode()))
                        return false;
                }
                else
                {
                    Debug.LogWarning("wScript Error");
                }
            }

            return true;
        }

        bool BookAllPathOptions(FCGPedestrianWaypointsContainer wayScript, int side, bool book = true)
        {
            int total;
            int wSide;
            FCGPedestrianWaypointsContainer wScript;

            total = (side == 0) ? wayScript.nextWay0.Length : wayScript.nextWay1.Length;

            for (int i = 0; i < total; i++)
            {
                if (side == 0)
                {
                    wScript = wayScript.nextWay0[i];
                    wSide = wayScript.nextWaySide0[i];
                }
                else
                {
                    wScript = wayScript.nextWay1[i];
                    wSide = wayScript.nextWaySide1[i];
                }

                if (book)
                {
                    bool force = wScript.GetNodeZeroCar(wSide) && wScript.GetNodeZeroCar(wSide).GetComponent<TrafficPedestrian>().Get_avanceNode();
                    if (!wScript.SetNodeZero(wSide, wayScript.transform, transform, force))
                        return false;
                }
                else
                {
                    wScript.UnSetNodeZero(wSide, transform);
                }
            }

            return true;
        }

        void Move()
        {
            //if (status == StatusPedestrian.bloked)
            //    return;

            VerificaPoints();

            var targetPoint = atualWayScript.Node(sideAtual, currentNode);
            targetPoint.y = thisTr.position.y;

            distanceToNode = Vector3.Distance(targetPoint, thisTr.position + thisTr.forward * (curveAdjustment * 0.5f));

            _agent.destination = targetPoint;
        }


        private void VerificaPoints()
        {
            if (distanceToNode < _settings.reachDistanceToNode)
            {
                if (currentNode < countWays - 1)
                {
                    currentNode++;

                    if (currentNode == 1)
                    {
                        atualWayScript.UnSetNodeZero(sideAtual, transform);  // Release the node so that the cars that were waiting for me to pass can proceed
                        //status = StatusPedestrian.transitingNormally;

                        if (nodeSteerCarefully || nodeSteerCarefully2)
                        {
                            myOldWayScript.UnSetNodeZero((myOldSideAtual == 1) ? 0 : 1, transform);
                            BookAllPathOptions(myOldWayScript, myOldSideAtual, false); //Release others nodes so that the cars that were waiting for me to pass can proceed
                        }

                        nodeSteerCarefully = false;
                        nodeSteerCarefully2 = false;
                    }
                }
                else
                {
                    int t = TestWay();

                    //True if the chosen path was the only option
                    bool verify = (sideAtual == 0) ? atualWayScript.nextWay0.Length == 1 : atualWayScript.nextWay1.Length == 1;

                    myOldWay = atualWay;
                    myOldSideAtual = sideAtual;
                    myOldWayScript = atualWayScript;

                    if (sideAtual == 0 && (!atualWayScript.oneway || atualWayScript.doubleLine))
                    {
                        sideAtual = atualWayScript.nextWaySide0[t];
                        atualWayScript = atualWayScript.nextWay0[t];
                    }
                    else
                    {
                        sideAtual = atualWayScript.nextWaySide1[t];
                        atualWayScript = atualWayScript.nextWay1[t];
                    }

                    atualWay = atualWayScript.transform;

                    //The road I'm on has no exit, so interdict the previous road that had only this road as an option
                    if (verify && atualWayScript.bloked)
                    {
                        myOldWayScript.bloked = true;

                        //status = StatusPedestrian.bloked;
                    }

                    DefineNewPath();

                    currentNode = 0;

                    float a = GetAngulo(transform, atualWayScript.Node(sideAtual, 0));

                    //if (myOldWayScript.oneway) 
                    if (myOldWayScript.oneway && !myOldWayScript.doubleLine)
                    {
                        if (myOldWayScript.doubleLine)
                            nodeSteerCarefully2 = (myOldSideAtual == 0 && a > 20 && a < 90) || (myOldSideAtual == 1 && a < 340 && a > 270);
                        else
                            nodeSteerCarefully = false;
                    }
                    else
                        nodeSteerCarefully = (atualWayScript.rightHand == 0 && a < 340 && a > 270) || (atualWayScript.rightHand != 0 && a > 20 && a < 90);

                    if (nodeSteerCarefully)
                    {
                        _avanceNode = myOldWayScript.AvanceNode(myOldSideAtual, myOldWayScript.waypoints.Count - 1, 7);
                    }
                }
            }
        }


        public Transform GetBehind() => behind;

        private void FixedUpdate()
        {
            FixedRaycasts();
        }
        void FixedRaycasts()
        {
            RaycastHit hit;
            float wdist = _settings.rayLength;

            Debug.DrawRay(thisTr.position + agentHeight, thisTr.forward * wdist, Color.yellow);

            if (Physics.Raycast(thisTr.position + agentHeight, thisTr.forward, out hit, wdist))
            {
                Debug.DrawRay(thisTr.position + agentHeight, thisTr.forward * wdist, Color.red);

                if (hit.transform.CompareTag("PeopleSemaphore"))
                {
                    _agent.isStopped = true;
                    //status = StatusPedestrian.stoppedAtTrafficLights;
                }
                else
                {
                    _agent.isStopped = false;
                    //status = StatusPedestrian.transitingNormally;
                }
            }
            else
            {
                _agent.isStopped = false;
                //status = StatusPedestrian.transitingNormally;
            }



            //if (rStop > 0 /*&& speed < 2*/)
            //{
            //    if (status == StatusPedestrian.stoppedAtTrafficLights || status == StatusPedestrian.waitingForAnotherVehicleToPass || status == StatusPedestrian.Undefined)
            //    {
            //    }
            //    else if (hit.transform.name == "Stop")
            //    {
            //        status = StatusPedestrian.stoppedAtTrafficLights;
            //    }
            //    else if (hit.transform.GetComponent<TrafficPedestrian>())
            //    {
            //        StatusPedestrian st = hit.transform.GetComponent<TrafficPedestrian>().status;
            //        status = (st == StatusPedestrian.stoppedAtTrafficLights || st == StatusPedestrian.waitingForAnotherVehicleToPass) ? st : StatusPedestrian.Undefined;
            //    }
            //}
        }


        void DefineNewPath()
        {
            nodes = new Transform[atualWay.childCount];
            int n = 0;
            foreach (Transform child in atualWay)
                nodes[n++] = child;

            countWays = nodes.Length;
        }


        int TestWay()
        {
            //Check if the path drawn for me is a good option (based on traffic) "VerifyTraffic"
            //Also check if the selected path has been blocked "CheckStoped"

            int total = 0;

            if (sideAtual == 0)
                total = atualWayScript.nextWay0.Length;
            else
                total = atualWayScript.nextWay1.Length;

            int t = Random.Range(0, total); // Sort one of the available paths

            if (total > 1) // If there are more path options to choose from
            {
                if (CheckStoped(t) || VerifyTraffic(t, 30) < 30 || VerifyNodeSteerCarefully2(t))
                {
                    //Test the paths to see which option is best

                    float maior = 0;
                    for (int i = 0; i < total; i++)
                    {
                        if (!CheckStoped(i)) // && !VerifyNodeSteerCarefully2(i))
                        {
                            float vf = VerifyTraffic(i, 30);

                            if (vf == 30)
                                return i;
                            else if (vf < 30)
                            {
                                if (vf > maior)
                                {
                                    maior = vf;
                                    t = i;
                                }
                            }
                        }
                    }
                }
            }

            return t;
        }

        private Vector3 GetNodeNextWay(int way, int node = 0)
        {
            /*
            Returns the position of the specified node, from a chosen path that is among the available paths (vlinked to my current path)
            */

            if (sideAtual == 0)
                return atualWayScript.nextWay0[way].Node(atualWayScript.nextWaySide0[way], node);
            else
                return atualWayScript.nextWay1[way].Node(atualWayScript.nextWaySide1[way], node);
        }

        private bool CheckStoped(int way)
        {
            //Checks if a path is blocked (due to no exit or other reason)

            if (sideAtual == 0)
                return atualWayScript.nextWay0[way].bloked;
            else
                return atualWayScript.nextWay1[way].bloked;
        }


        /*
        bool VerifyDoubleOneWayOption(int t)
        {
            //Vector3 node0 = GetNodeNextWay(t, 0);
            //nodeSteerCarefully2 = (myOldSideAtual == 0 && a > 20 && a < 90) || (myOldSideAtual == 1 && a < 340 && a > 270);
            return true;    
        }
        */

        bool VerifyNodeSteerCarefully2(int t)
        {
            if (atualWayScript.oneway && atualWayScript.doubleLine)
            {
                float a = GetAngulo(transform, GetNodeNextWay(t, 0));
                return (sideAtual == 0 && a > 20 && a < 90) || (sideAtual == 1 && a < 340 && a > 270);

            }

            return false;
        }


        float VerifyTraffic(int t, float mts = 12)
        {
            //Checks if the specified path is a good choice, it may be congested

            RaycastHit hit2;

            Vector3 node0 = GetNodeNextWay(t, 0) + new Vector3(0, 0.5f, 0);
            Vector3 node1 = GetNodeNextWay(t, 1) + new Vector3(0, 0.5f, 0);


            if (Physics.Raycast(node0, node1 - node0, out hit2, mts))
                if (hit2.transform.GetComponent<TrafficPedestrian>())
                {
                    if (hit2.transform.GetComponent<TrafficPedestrian>().GetSpeed() < 8)
                        return hit2.distance;
                    else
                        return mts - 1;
                }

            return mts;
        }

        private int countC = 0;

        void SelfDestructWhenAwayFromThePlayer()
        {
            var a = thisTr.position; a.y = 0;
            var b = player.position; b.y = 0;
            var dist = Vector3.Distance(a, b);
            if (dist > distanceToSelfDestroy && /*(Time.time > timeStoped + _settings.timeToStayStill)&*/ !InTheFieldOfVision(thisTr.position, cameraTr))
            {
                DestroyObject();
            }

            //if (speed < 2 && status != StatusPedestrian.stoppedAtTrafficLights && (Time.time > timeStoped + _settings.timeToStayStill) & InTheFieldOfVision(thisTr.position, cameraTr))
            //{
            //    DestroyObject();
            //}
            //else if (Vector3.Distance(thisTr.position, player.position) < distanceToSelfDestroy || InTheFieldOfVision(thisTr.position, cameraTr))
            //{
            //    countC = 0;
            //}
            //else
            //{
            //    Debug.Log("? ? ?");
            //    countC++;

            //    if (countC >= 2)
            //    {
            //        DestroyObject();
            //    }
            //}
        }

        public void SelfDestructWhenAwayFromThePlayerInit()
        {
            if (tSystem && player)
            {
                var a = thisTr.position; a.y = 0;
                var b = player.position; b.y = 0;
                var dist = Vector3.Distance(a, b);
                if (dist > distanceToSelfDestroy && !InTheFieldOfVision(transform.position, cameraTr))
                {
                    DestroyObject();
                }
                else
                {
                    ActivateSelfDestructWhenAwayFromThePlayer();
                }
            }
        }

        private void DestroyObject()
        {
            tSystem.nPedestrians--;
            Destroy(this.gameObject);
        }


        bool InTheFieldOfVision(Vector3 source, Transform target)
        {
            // the IACar wants to disappear without it being seen by the camera/player

            RaycastHit obsRay2;
            Vector3 rayStartPos = source + agentHeight;
            Vector3 targetPos = target.position;

            if (Physics.Linecast(rayStartPos, targetPos, out obsRay2)) //, ~(1 << LayerMask.NameToLayer("Lattice"))))
            {
                var oRay2Tr = obsRay2.transform;
                if (oRay2Tr == target || oRay2Tr.root == target)
                {
                    //Debug.DrawLine(rayStartPos, targetPos, Color.red);
                    return true;
                }
                else
                {
                    //Debug.DrawLine(rayStartPos, targetPos, Color.green);
                    return false;
                }
            }
            else
            {
                //Debug.DrawLine(rayStartPos, targetPos, Color.blue);
                return true;
            }
        }


        private float GetAngulo(Transform origem, Vector3 target)
        {
            float r;

            GameObject compass = new GameObject("Compass");
            compass.transform.parent = origem;
            compass.transform.localPosition = new Vector3(0, 0, 0);

            compass.transform.LookAt(target);
            r = compass.transform.localEulerAngles.y;

            DestroyImmediate(compass);
            return r;
        }
    }
}