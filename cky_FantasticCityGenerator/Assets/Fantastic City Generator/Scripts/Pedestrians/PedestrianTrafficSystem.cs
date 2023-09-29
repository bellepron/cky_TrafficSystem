using cky.FCG.Pedestrian.StateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FCG.Pedestrians
{
    public class PedestrianTrafficSystem : MonoBehaviour
    {
        public Transform player = null;

        [Header("Traffic Light:  0=Right  1=Left  2=Japan")]
        [Range(0, 2)]
        public int trafficLightHand = 0;

        [Space(10)]
        [SerializeField] GameObject[] pedestrianPrefabs;

        public int nPedestrians;
        [SerializeField] int maxPedestriansWithPlayer = 50;

        [Range(100, 1000)]
        [SerializeField] float around = 150;

        ArrayList spanwsPoints;

        bool _isFirstTime = true;

        [System.Serializable]
        public class WpData
        {
            public bool[] tsActive;
            public Vector3[] tf01;
            public FCGPedestrianWaypointsContainer[] tsParent;
            public bool[] tsOneway;
            public bool[] tsOnewayDoubleLine;
            public int[] tsSide;
        }

        WpData _wpData = new WpData();

        [System.Serializable]
        public class WpDataSpawn
        {
            public Vector3 position;
            public Quaternion rotation;
            public float locateZ;
            public int side;
            public int node;
            public FCGPedestrianWaypointsContainer wayScript;
        }

        List<WpDataSpawn> _wpDataSpawn;

        [SerializeField] private float intervalLoadPedestrian = 2;

        public void UpdateAllWayPoints()
        {
            FCGPedestrianWaypointsContainer[] tArray = GameObject.FindObjectsOfType<FCGPedestrianWaypointsContainer>();

            for (int f = 0; f < tArray.Length; f++)
            {
                tArray[f].ResetWay();
                tArray[f].GetWaypoints();
            }

            GetWpData();

            for (int f = 0; f < tArray.Length; f++)
                if (tArray[f].transform.childCount > 1)
                    tArray[f].wpData = _wpData;


            for (int f = 0; f < tArray.Length; f++)
                if (tArray[f].transform.childCount > 1)
                    tArray[f].NextWaysCloseOnly();

            for (int f = 0; f < tArray.Length; f++)
                if (tArray[f].transform.childCount > 1)
                    tArray[f].NextWays();


        }
        public void GetWpData()
        {

            FCGPedestrianWaypointsContainer[] ts = FindObjectsOfType<FCGPedestrianWaypointsContainer>();

            _wpData.tsActive = new bool[ts.Length * 2];
            _wpData.tf01 = new Vector3[ts.Length * 2];
            _wpData.tsParent = new FCGPedestrianWaypointsContainer[ts.Length * 2];
            _wpData.tsOneway = new bool[ts.Length * 2];
            _wpData.tsOnewayDoubleLine = new bool[ts.Length * 2];
            _wpData.tsSide = new int[ts.Length * 2];

            int t = -1;

            for (int i = 0; i < ts.Length; i++)
            {

                if (ts[i].waypoints.Count > 1)
                {
                    t++;

                    if (!ts[i].oneway || ts[i].doubleLine)
                    {
                        _wpData.tsActive[t] = true;
                        _wpData.tf01[t] = ts[i].Node(0, 0);
                        _wpData.tsParent[t] = ts[i];
                        _wpData.tsSide[t] = 0;
                        _wpData.tsOneway[t] = ts[i].oneway;
                        _wpData.tsOnewayDoubleLine[t] = ts[i].oneway && ts[i].doubleLine;
                    }
                    else
                        _wpData.tsActive[t] = false;

                    t++;
                    _wpData.tsActive[t] = true;
                    _wpData.tf01[t] = ts[i].Node(1, 0);
                    _wpData.tsParent[t] = ts[i];
                    _wpData.tsSide[t] = 1;
                    _wpData.tsOneway[t] = ts[i].oneway;
                    _wpData.tsOnewayDoubleLine[t] = ts[i].oneway && ts[i].doubleLine;

                }
                else
                {
                    t++;
                    _wpData.tsActive[t] = false;
                    t++;
                    _wpData.tsActive[t] = false;
                }



            }

        }


        private Transform downTowmPosition;

        void Start()
        {
            /*
             if (!player)
                 if(GameObject.FindGameObjectWithTag("MainCamera"))
                     player = GameObject.FindGameObjectWithTag("MainCamera").transform;
            */

            player = GameObject.FindWithTag("Player").transform;

            if (GameObject.Find("DTPosition"))
                downTowmPosition = GameObject.Find("DTPosition").transform;
            else
                downTowmPosition = null;

            LoadPedestrians(trafficLightHand);
        }

        [SerializeField] private float ckyMinDist = 10.0f;
        [SerializeField] private float ckyDist = 40.0f;

        public void LoadPedestrians(int right_Hand)
        {
            if (maxPedestriansWithPlayer == 0)
            {
                Debug.LogError("You need to set the maximum number of vehicles in the Traffic System");
                return;
            }

            FCGPedestrianWaypointsContainer[] ts = FindObjectsOfType<FCGPedestrianWaypointsContainer>();

            int n = ts.Length;
            for (int i = 0; i < n; i++)
                if (ts[i].transform.childCount == 0)
                    DestroyImmediate(ts[i].gameObject);  // Destroy Empty 

            UpdateAllWayPoints();

            /*
            if (!player)
                if(GameObject.FindGameObjectWithTag("MainCamera"))
                    player = GameObject.FindGameObjectWithTag("MainCamera").transform;
            */

            if (!player)
            {
                Debug.LogWarning("You have not set the player in the Traffic System on Inspector. This drastically decreases performance in big cities");
            }

            //DestroyImmediate(GameObject.Find("PedestrianContainer"));
            GameObject PedestrianContainer = GameObject.Find("PedestrianContainer");

            if (!PedestrianContainer)
            {
                PedestrianContainer = new GameObject("PedestrianContainer");
                nPedestrians = 0;
            }
            else
            {
                nPedestrians = PedestrianContainer.transform.childCount;
            }

            trafficLightHand = right_Hand;

            DeffineDirection(right_Hand);

            _wpDataSpawn = new List<WpDataSpawn>();

            ts = FindObjectsOfType<FCGPedestrianWaypointsContainer>();
            n = ts.Length;

            for (int i = 0; i < n; i++)
            {
                if (!ts[i].bloked && ts[i].waypoints.Count > 1)
                {
                    for (int nSide = 0; nSide <= 1; nSide++)
                    {
                        if ((!ts[i].oneway || ts[i].doubleLine) || (nSide == 1 && trafficLightHand == 0) || (nSide == 0 && trafficLightHand != 0))
                        {
                            for (int node = 0; node < ts[i].waypoints.Count - 1; node++)
                            {
                                float dist = Vector3.Distance(ts[i].Node(nSide, node), ts[i].Node(nSide, node + 1));

                                //// Bu vardý.
                                //if (dist > ckyDist) // TODO: cky 20ydi.
                                //    PlaceSpawnPoint(ts[i], nSide, node, dist / 2);

                                // Bu yoktu.
                                if (dist < ckyDist)
                                {
                                    if (dist >= ckyMinDist)
                                    {
                                        /*if (ckyPerThousand(800))*/
                                        if (ahmet % ahmetLimit == 0)
                                        {
                                            ahmet = 0;
                                            PlaceSpawnPoint(ts[i], nSide, node, dist * (0.50f + ckyRandom(0.1f)));
                                        }
                                        ahmet++;
                                    }
                                }
                                else if (dist >= ckyDist && dist < ckyDist * 3)
                                {
                                    PlaceSpawnPoint(ts[i], nSide, node, dist * (0.50f + ckyRandom(0.3f)));
                                }
                                else if (dist >= ckyDist * 3 && dist < ckyDist * 4)
                                {
                                    PlaceSpawnPoint(ts[i], nSide, node, dist * (0.33f + ckyRandom(0.25f)));
                                    PlaceSpawnPoint(ts[i], nSide, node, dist * (0.66f + ckyRandom(0.25f)));
                                }
                                else if (dist >= ckyDist * 4 && dist < ckyDist * 5)
                                {
                                    if (ckyPerThousand(900)) PlaceSpawnPoint(ts[i], nSide, node, dist * (0.25f + ckyRandom(0.2f)));
                                    if (ckyPerThousand(900)) PlaceSpawnPoint(ts[i], nSide, node, dist * (0.50f + ckyRandom(0.2f)));
                                    if (ckyPerThousand(900)) PlaceSpawnPoint(ts[i], nSide, node, dist * (0.75f + ckyRandom(0.2f)));
                                }
                                else if (dist >= ckyDist * 5)
                                {
                                    if (ckyPerThousand(800)) PlaceSpawnPoint(ts[i], nSide, node, dist * (0.20f + ckyRandom(0.125f)));
                                    if (ckyPerThousand(800)) PlaceSpawnPoint(ts[i], nSide, node, dist * (0.40f + ckyRandom(0.125f)));
                                    if (ckyPerThousand(800)) PlaceSpawnPoint(ts[i], nSide, node, dist * (0.60f + ckyRandom(0.125f)));
                                    if (ckyPerThousand(800)) PlaceSpawnPoint(ts[i], nSide, node, dist * (0.80f + ckyRandom(0.125f)));
                                }
                            }
                        }
                    }
                }
            }



            if (!Application.isPlaying)
                _isFirstTime = true;


            if (player && Application.isPlaying)
            {
                InvokeRepeating(nameof(LoadPedestrians2), 0f, intervalLoadPedestrian);
            }
            else
                LoadPedestrians2();

        }

        private int ahmet = 0;
        [SerializeField] private int ahmetLimit = 4;
        private float ckyRandom(float b) => UnityEngine.Random.Range(-b, b);
        private bool ckyPerThousand(int percentage)
        {
            if (percentage > UnityEngine.Random.Range(0, 1000))
                return true;

            return false;
        }

        private void PlaceSpawnPoint(FCGPedestrianWaypointsContainer f, int side, int node, float locate)
        {
            _wpDataSpawn.Add(new WpDataSpawn { locateZ = locate, position = f.AvanceNode(side, node, locate), rotation = f.NodeRotation(side, node), side = side, node = node, wayScript = f });
        }


        public void LoadPedestrians2()
        {
            if (!player && !_isFirstTime)
                return;

            if (_isFirstTime && player && nPedestrians > 0)
            {

                PedestrianStateMachine[] vcles = FindObjectsOfType<PedestrianStateMachine>();
                int nvcles = vcles.Length;
                for (int i = 0; i < nvcles; i++)
                {
                    var tPedestrian = vcles[i].GetComponent<PedestrianStateMachine>();

                    tPedestrian.distanceToSelfDestroy = around;
                    tPedestrian.player = player;
                    tPedestrian.tSystem = this;
                }
            }

            GameObject PedestrianContainer = GameObject.Find("PedestrianContainer");
            if (PedestrianContainer)
                nPedestrians = PedestrianContainer.transform.childCount;
            else
                nPedestrians = 0;


            if (_isFirstTime && nPedestrians > 0)
            {
                _isFirstTime = false;
                return;
            }

            if (player && nPedestrians >= maxPedestriansWithPlayer)
                return;

            GameObject human;

            int n = _wpDataSpawn.Count;

            int _nPedestrians = nPedestrians;

            bool invert = (Random.Range(1, 20) < 10);

            Transform test = new GameObject("verify").transform;

            for (int j = 0; j < n; j++)
            {
                int i = (invert) ? n - 1 - j : j;
                //int i = j;

                if (player && nPedestrians >= maxPedestriansWithPlayer)
                {
                    break;
                }
                else
                {
                    if (player)
                    {
                        float dist = Vector3.Distance(_wpDataSpawn[i].position, player.position);

                        if (player && (dist > around || (!_isFirstTime && dist < 80)))
                            continue;

                        if (!_isFirstTime && InTheFieldOfVision(player.position, _wpDataSpawn[i].position))
                            continue;
                    }

                    bool go = false;
                    RaycastHit obsRay2;

                    // Check for a human at the spawn point
                    if (_isFirstTime)
                        go = true; //|| !player;
                    else
                        go = !Physics.Linecast(_wpDataSpawn[i].wayScript.Node(_wpDataSpawn[i].side, _wpDataSpawn[i].node + 1) + Vector3.up * 1f,
                                               _wpDataSpawn[i].wayScript.Node(_wpDataSpawn[i].side, _wpDataSpawn[i].node) + Vector3.up * 1f, out obsRay2);

                    if (go)
                    {
                        human = (GameObject)Instantiate(pedestrianPrefabs[Mathf.Clamp(Random.Range(0, pedestrianPrefabs.Length), 0, pedestrianPrefabs.Length - 1)], _wpDataSpawn[i].position, _wpDataSpawn[i].rotation); ;
                        human.transform.SetParent(PedestrianContainer.transform);

                        var tPedestrian = human.GetComponent<PedestrianStateMachine>();
                        tPedestrian.sideAtual = (_wpDataSpawn[i].wayScript.oneway && _wpDataSpawn[i].wayScript.doubleLine && _wpDataSpawn[i].wayScript.rightHand != 0) ? ((_wpDataSpawn[i].side == 1) ? 0 : 1) : _wpDataSpawn[i].side;
                        tPedestrian.atualWay = _wpDataSpawn[i].wayScript.transform;
                        tPedestrian.atualWayScript = _wpDataSpawn[i].wayScript;
                        tPedestrian.currentNode = _wpDataSpawn[i].node + 1;

                        if (player)
                        {
                            tPedestrian.distanceToSelfDestroy = around;
                            tPedestrian.player = player;
                            tPedestrian.tSystem = this;
                        }

                        nPedestrians++;
                    }
                }
            }

            if (Application.isPlaying)
                Destroy(test.gameObject);
            else
                DestroyImmediate(test.gameObject);


            if (nPedestrians > 0)
            {
                _isFirstTime = false;
            }
            else
            {
                FCGPedestrianWaypointsContainer[] _ts = FindObjectsOfType<FCGPedestrianWaypointsContainer>();

                if (_ts.Length == 0)
                {
                    Debug.Log("Need to generate the city again to use the updated traffic system");
                }
            }
        }


        private void Pause(Vector3 position)
        {
#if UNITY_EDITOR
            if (GameObject.Find("CubePause"))
            {
                Debug.Log("Paused");
                GameObject.Find("CubePause").transform.position = position;
                UnityEditor.EditorApplication.isPaused = true;
            }
#endif
        }
        /*
        private float DistanceToDownTown()
        {
            if (downTowmPosition)
                return Mathf.Lerp(0.5f, 1.2f,  Vector3.Distance(player.position, downTowmPosition.position);

            return 1;
        }
        */

        bool InTheFieldOfVision(Vector3 source, Vector3 target)
        {
            RaycastHit obsRay2;

            return (!(Physics.Linecast(source + Vector3.up * 4f, target + Vector3.up * 4f, out obsRay2)) || !(Physics.Linecast(source + Vector3.up * 1f, target + Vector3.up * 1f, out obsRay2)));
        }

        public void DeffineDirection(int hand_Right)
        {
            trafficLightHand = hand_Right;

            //Inverse Traffic-Lights 
            TFShiftHand2[] TLs = FindObjectsOfType<TFShiftHand2>();
            if (TLs.Length == 0 && GameObject.Find("Traffic-Light-T"))
            {
                Debug.LogError("It is not compatible with the previous traffic system.\nTo use the new system you need to generate the city again");
                UpdateAllWayPoints();
                return;
            }

            for (int i = 0; i < TLs.Length; i++)
                TLs[i].RightHand(trafficLightHand);

            //Inverse Nodes
            FCGPedestrianWaypointsContainer[] ts = FindObjectsOfType<FCGPedestrianWaypointsContainer>();
            for (int i = 0; i < ts.Length; i++)
                ts[i].InvertNodesDirection(trafficLightHand);

            GameObject[] roadMark = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name.Equals("Road-Mark")).ToArray();
            for (int i = 0; i < roadMark.Length; i++)
                if (roadMark[i].transform.Find("RoadMark"))
                    roadMark[i].transform.Find("RoadMark").gameObject.SetActive(trafficLightHand == 0);

            roadMark = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name.Equals("Road-Mark-Rev")).ToArray();
            for (int i = 0; i < roadMark.Length; i++)
                if (roadMark[i].transform.Find("RoadMarkRev"))
                    roadMark[i].transform.Find("RoadMarkRev").gameObject.SetActive(trafficLightHand != 0);

            UpdateAllWayPoints();
        }
    }
}