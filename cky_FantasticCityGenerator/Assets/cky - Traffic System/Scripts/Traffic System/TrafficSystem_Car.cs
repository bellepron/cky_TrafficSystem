using System.Collections.Generic;
using System.Collections;
using cky.GizmoHelper;
using UnityEngine;
using CKY_Pooling;

namespace cky.TrafficSystem
{
    public class TrafficSystem_Car : MonoBehaviour
    {
        #region Data Holder's

        [System.Serializable]
        public class WpDataCar
        {
            public bool[] tsActive;
            public Vector3[] tf01;
            public WaypointsContainer[] tsParent;
            public bool[] tsOneway;
            public bool[] tsOnewayDoubleLine;
            public int[] tsSide;
        }

        [System.Serializable]
        public class WpDataSpawnCar
        {
            public Vector3 position;
            public Quaternion rotation;
            public float locateZ;
            public int side;
            public int node;
            public WaypointsContainer wayScript;
        }

        #endregion
        struct Tags
        {
            public const string Player = "Player";
        }

        [Space(5)]
        public Transform player = null;

        WaypointsContainer[] _waypointContainers;

        [Space(10)]
        [Header("Car Prefabs")]
        public Transform[] IaCars;

        [Space(5)]
        [Header("Around")]
        [Range(0, 1000)][SerializeField] float aroundMin = 80;
        [Range(0, 1000)][SerializeField] float aroundMax = 150;
        ArrayList _spawnPoints;
        List<WpDataSpawnCar> _wpDataSpawn;
        WpDataCar _wpData = new WpDataCar();

        [Space(5)]
        [SerializeField] int nVehicles = 0;
        [SerializeField] int maxVehiclesWithPlayer = 50;

        [Space(5)]
        [SerializeField] private float intervalLoadCar = 1;
        [SerializeField] private float minNodeDistanceToCreate = 10.0f;
        [SerializeField] private float distanceToRepeat = 40.0f;

        [Space(10)]
        [SerializeField] private int skipping = 1;
        [SerializeField] bool isClassic;
        int _counterForSkipping = 0;

        bool _isGameStarted;

        [Space(15)]
        [Header("Traffic Car Checker")]
        [SerializeField] bool isTrafficCarCheckerActive = true;
        [SerializeField] float trafficCarCheckRadius = 5.0f;

        [Space(15)]
        [Header("Gizmos")]
        [SerializeField] float gizmo_CircleOffset_Y = 0.5f;
        [SerializeField] int gizmo_CircleSegmentCount = 360;
        [SerializeField] Color gizmo_Color = Color.black;

        [Space(15)]
        [SerializeField] List<TrafficCar> currentTrafficCars = new List<TrafficCar>();



        public void SetPlayerAndStart(Transform playerTr)
        {
            player = playerTr;

            LoadCars();
        }

        private void Awake()
        {
            player = GameObject.FindWithTag(Tags.Player)?.transform;
            if (player == null) player = Camera.main.transform;

            _waypointContainers = FindObjectsOfType<WaypointsContainer>();

            _isGameStarted = true;
        }

        void Start()
        {
            if (!player)
            {
                Debug.LogWarning("You have not set the player in the Traffic System on Inspector. This drastically decreases performance in big cities");
            }
            else
            {
                LoadCars();
            }
        }


        public void LoadCars()
        {
            currentTrafficCars = new List<TrafficCar>();

            if (maxVehiclesWithPlayer == 0)
            {
                Debug.LogError("You need to set the maximum number of vehicles in the Traffic System");
                return;
            }

            if (!_isGameStarted) _waypointContainers = FindObjectsOfType<WaypointsContainer>();

            int n = _waypointContainers.Length;
            for (int i = 0; i < n; i++)
                if (_waypointContainers[i].transform.childCount == 0)
                    DestroyImmediate(_waypointContainers[i].gameObject);  // Destroy Empty 

            UpdateAllWayPoints();

            nVehicles = currentTrafficCars.Count;

            DeffineDirection();

            _wpDataSpawn = new List<WpDataSpawnCar>();

            n = _waypointContainers.Length;

            for (int i = 0; i < n; i++)
            {
                var _w = _waypointContainers[i];

                if (!_w.bloked && _w.waypoints.Count > 1)
                {
                    for (int nSide = 0; nSide <= 1; nSide++)
                    {
                        if ((!_w.oneway || _w.doubleLine) || (nSide == 1))
                        {
                            for (int node = 0; node < _w.waypoints.Count - 1; node++)
                            {
                                float dist = Vector3.Distance(_w.Node(nSide, node), _w.Node(nSide, node + 1));

                                if (isClassic)
                                {
                                    if (dist > minNodeDistanceToCreate)
                                        PlaceSpawnPoint(_w, nSide, node, dist / 2);
                                }
                                else
                                {
                                    if (dist < distanceToRepeat)
                                    {
                                        if (dist >= minNodeDistanceToCreate)
                                        {
                                            if (_counterForSkipping % skipping == 0)
                                            {
                                                _counterForSkipping = 0;
                                                PlaceSpawnPoint(_w, nSide, node, dist * (0.50f + ckyRandom(0.1f)));
                                            }
                                            _counterForSkipping++;
                                        }
                                    }
                                    else if (dist >= distanceToRepeat && dist < distanceToRepeat * 3)
                                    {
                                        PlaceSpawnPoint(_waypointContainers[i], nSide, node, dist * (0.50f + ckyRandom(0.3f)));
                                    }
                                    else if (dist >= distanceToRepeat * 3 && dist < distanceToRepeat * 4)
                                    {
                                        PlaceSpawnPoint(_w, nSide, node, dist * (0.33f + ckyRandom(0.25f)));
                                        PlaceSpawnPoint(_w, nSide, node, dist * (0.66f + ckyRandom(0.25f)));
                                    }
                                    else if (dist >= distanceToRepeat * 4 && dist < distanceToRepeat * 5)
                                    {
                                        if (ckyPerThousand(900)) PlaceSpawnPoint(_w, nSide, node, dist * (0.25f + ckyRandom(0.2f)));
                                        if (ckyPerThousand(900)) PlaceSpawnPoint(_w, nSide, node, dist * (0.50f + ckyRandom(0.2f)));
                                        if (ckyPerThousand(900)) PlaceSpawnPoint(_w, nSide, node, dist * (0.75f + ckyRandom(0.2f)));
                                    }
                                    else if (dist >= distanceToRepeat * 5)
                                    {
                                        if (ckyPerThousand(800)) PlaceSpawnPoint(_w, nSide, node, dist * (0.20f + ckyRandom(0.125f)));
                                        if (ckyPerThousand(800)) PlaceSpawnPoint(_w, nSide, node, dist * (0.40f + ckyRandom(0.125f)));
                                        if (ckyPerThousand(800)) PlaceSpawnPoint(_w, nSide, node, dist * (0.60f + ckyRandom(0.125f)));
                                        if (ckyPerThousand(800)) PlaceSpawnPoint(_w, nSide, node, dist * (0.80f + ckyRandom(0.125f)));
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (player && Application.isPlaying)
            {
                InvokeRepeating(nameof(LoadCars2), 0f, intervalLoadCar);
            }
            else
            {
                LoadCars2();
            }
        }

        public void LoadCars2()
        {
            if (!player) return;

            nVehicles = currentTrafficCars.Count;

            TrafficCar car;

            int n = _wpDataSpawn.Count;
            bool invert = (Random.Range(1, 20) < 10);

            var posPlayer = player.position; posPlayer.y = 0.0f;
            for (int j = 0; j < n; j++)
            {
                int i = (invert) ? n - 1 - j : j;

                if (nVehicles >= maxVehiclesWithPlayer)
                {
                    break;
                }
                else
                {
                    var wpDataSpawn = _wpDataSpawn[i];

                    var posData = wpDataSpawn.position; posData.y = 0.0f;
                    float dist = Vector3.Distance(posData, posPlayer);

                    if (dist < aroundMin || dist > aroundMax) continue;

                    var wpDataSpawn_Side = wpDataSpawn.side;
                    var wpDataSpawn_Node = wpDataSpawn.node;
                    var wpDataSpawn_WayScript = wpDataSpawn.wayScript;
                    var aw = wpDataSpawn_WayScript.transform;
                    var sa = wpDataSpawn_Side;
                    if (!ThereIsNoTrafficCar_InCheckRadius(wpDataSpawn.position, aw, sa))
                    {
                        car = CKY_PoolManager.Spawn(IaCars[Random.Range(0, IaCars.Length)], wpDataSpawn.position + Vector3.up * 0.1f, wpDataSpawn.rotation).GetComponent<TrafficCar>();

                        AddToCurrentTrafficCar(car);

                        car.TrafficSystemInit(sa, aw, wpDataSpawn_WayScript, wpDataSpawn_Node + 1, aroundMax, player, this);

                        nVehicles++;
                    }
                }
            }
        }



        public void UpdateAllWayPoints()
        {
            _waypointContainers = FindObjectsOfType<WaypointsContainer>();

            for (int i = 0; i < _waypointContainers.Length; i++)
            {
                _waypointContainers[i].ResetWay();
                _waypointContainers[i].GetWaypoints();
            }

            GetWpData();

            for (int i = 0; i < _waypointContainers.Length; i++)
            {
                if (_waypointContainers[i].transform.childCount > 1)
                {
                    _waypointContainers[i].wpData = _wpData;
                    _waypointContainers[i].NextWaysCloseOnly();
                    _waypointContainers[i].NextWays();
                }
            }
        }

        public void GetWpData()
        {
            var wpcLength = _waypointContainers.Length;

            _wpData.tsActive = new bool[wpcLength * 2];
            _wpData.tf01 = new Vector3[wpcLength * 2];
            _wpData.tsParent = new WaypointsContainer[wpcLength * 2];
            _wpData.tsOneway = new bool[wpcLength * 2];
            _wpData.tsOnewayDoubleLine = new bool[wpcLength * 2];
            _wpData.tsSide = new int[wpcLength * 2];

            int t = -1;

            for (int i = 0; i < wpcLength; i++)
            {
                var currentWPContainer = _waypointContainers[i];

                if (_waypointContainers[i].waypoints.Count > 1)
                {
                    t++;

                    if (!currentWPContainer.oneway || currentWPContainer.doubleLine)
                    {
                        _wpData.tsActive[t] = true;
                        _wpData.tf01[t] = currentWPContainer.Node(0, 0);
                        _wpData.tsParent[t] = currentWPContainer;
                        _wpData.tsSide[t] = 0;
                        _wpData.tsOneway[t] = currentWPContainer.oneway;
                        _wpData.tsOnewayDoubleLine[t] = currentWPContainer.oneway && currentWPContainer.doubleLine;
                    }
                    else
                        _wpData.tsActive[t] = false;

                    t++;
                    _wpData.tsActive[t] = true;
                    _wpData.tf01[t] = currentWPContainer.Node(1, 0);
                    _wpData.tsParent[t] = currentWPContainer;
                    _wpData.tsSide[t] = 1;
                    _wpData.tsOneway[t] = currentWPContainer.oneway;
                    _wpData.tsOnewayDoubleLine[t] = currentWPContainer.oneway && currentWPContainer.doubleLine;
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

        private void PlaceSpawnPoint(WaypointsContainer f, int side, int node, float locate)
        {
            _wpDataSpawn.Add(new WpDataSpawnCar
            {
                locateZ = locate,
                position = f.AvanceNode(side, node, locate),
                rotation = f.NodeRotation(side, node),
                side = side,
                node = node,
                wayScript = f
            });
        }

        public void DeffineDirection()
        {
            //Inverse Nodes
            for (int i = 0; i < _waypointContainers.Length; i++)
                _waypointContainers[i].InvertNodesDirection(0);

            UpdateAllWayPoints();
        }

        public bool ThereIsNoTrafficCar_InCheckRadius(Vector3 position, Transform atualWay, int sideAtual)
        {
            if (!isTrafficCarCheckerActive) return false;

            foreach (var c in currentTrafficCars)
            {
                var inRange = Vector3.Distance(c.transform.position, position) < trafficCarCheckRadius;

                if (inRange)
                {
                    if (atualWay == c.atualWay && sideAtual == c.sideAtual)
                    {
                        return true;
                    }

                    return true;
                }
            }

            return false;
        }

        private float ckyRandom(float b) => UnityEngine.Random.Range(-b, b);
        private bool ckyPerThousand(int percentage)
        {
            if (percentage > UnityEngine.Random.Range(0, 1000))
                return true;

            return false;
        }

        private void AddToCurrentTrafficCar(TrafficCar car)
        {
            if (!currentTrafficCars.Contains(car)) currentTrafficCars.Add(car);
        }

        public void RemoveFromCurrentTrafficCar(TrafficCar car)
        {
            if (currentTrafficCars.Contains(car)) currentTrafficCars.Remove(car);
        }



        #region Gizmos

        void OnDrawGizmos()
        {
            if (!player) return;

            GizmoHelper_CKY.DrawCircle(player.transform, new Vector3(0, gizmo_CircleOffset_Y, 0), aroundMin, gizmo_CircleSegmentCount, gizmo_Color);
            GizmoHelper_CKY.DrawCircle(player.transform, new Vector3(0, gizmo_CircleOffset_Y, 0), aroundMax, gizmo_CircleSegmentCount, gizmo_Color);
        }

        #endregion
    }
}