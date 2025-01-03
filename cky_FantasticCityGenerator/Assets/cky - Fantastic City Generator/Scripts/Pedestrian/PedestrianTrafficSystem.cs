using cky.FCG.Pedestrian.StateMachine;
using CKY_Pooling;
using FCG.Pedestrian.Matrix;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cky.Reuseables.Extension;

namespace FCG.Pedestrian
{
    #region Data Holder's

    [System.Serializable]
    public class WpDataPedestrian
    {
        public bool[] tsActive;
        public Vector3[] tf01;
        public FCGPedestrianWaypointsContainer[] tsParent;
        public bool[] tsOneway;
        public bool[] tsOnewayDoubleLine;
        public int[] tsSide;
    }

    [System.Serializable]
    public class WpDataSpawnPedestrian
    {
        public Vector3 position;
        public Quaternion rotation;
        public float locateZ;
        public int side;
        public int node;
        public FCGPedestrianWaypointsContainer wayScript;
    }
    #endregion

    public class PedestrianTrafficSystem : MonoBehaviour
    {
        [Space(5)]
        public Transform player = null;

        [Space(10)]
        [Header("P Spawn Point Matrix")]
        [SerializeField] P_SpawnPointMatrix P_SpawnPointMatrix;

        [Space(10)]
        [Header("Pedestrians")]
        [SerializeField] Transform[] pedestrianPrefabs;

        [Space(5)]
        [Header("Around")]
        [Range(0, 1000)][SerializeField] float aroundMax = 150;
        [Range(0, 1000)][SerializeField] float aroundMin = 80;
        [Range(0, 1000)][SerializeField] float aroundMaxStart = 100;
        [Range(0, 1000)][SerializeField] float aroundMinStart = 5;
        [Range(50, 1000)][SerializeField] float pedestrianDestroyDistance = 50;
        float _aroundMaxCurrent;
        float _aroundMinCurrent;
        bool _isFirstTime = true;

        [Space(5)]
        [SerializeField] int nPedestrians;
        [SerializeField] int maxPedestriansWithPlayer = 50;

        [Space(5)]
        [SerializeField] private float intervalLoadPedestrian = 1;
        [SerializeField] private float minNodeDistanceToCreate = 10.0f;
        //[SerializeField] bool isFOVImportant;

        [Space(10)]
        [SerializeField] bool isClassic;
        [SerializeField] private int skippingInit = 1;
        [SerializeField] private int _skipping = 1;
        private int counterForSkipping = 0;


        FCGPedestrianWaypointsContainer[] _waypointContainers;
        ArrayList _spanwsPoints;
        List<WpDataSpawnPedestrian> _wpDataSpawn;
        WpDataPedestrian _wpData = new WpDataPedestrian();


        bool _isGameStarted;

        [Space(15)]
        [SerializeField] List<PedestrianStateMachine> currentPedestrians = new List<PedestrianStateMachine>();



        private void Awake()
        {
            //player = GameObject.FindWithTag(TagHelper.PLAYER)?.transform;
            //if (player == null) player = Camera.main.transform;

            _waypointContainers = FindObjectsOfType<FCGPedestrianWaypointsContainer>();

            _isGameStarted = true;
        }

        public void SetPlayerAndStart(Transform playerTr)
        {
            player = playerTr;

            LoadPedestrians();
        }

        //void Start()
        //{
        //    if (!player)
        //    {
        //        Debug.LogWarning("You have not set the player in the Traffic System on Inspector. This drastically decreases performance in big cities");
        //    }
        //    else
        //    {
        //        LoadPedestrians();
        //    }
        //}



        public void UpdateAllWayPoints()
        {
            _waypointContainers = FindObjectsOfType<FCGPedestrianWaypointsContainer>();

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
            _wpData.tsParent = new FCGPedestrianWaypointsContainer[wpcLength * 2];
            _wpData.tsOneway = new bool[wpcLength * 2];
            _wpData.tsOnewayDoubleLine = new bool[wpcLength * 2];
            _wpData.tsSide = new int[wpcLength * 2];

            int t = -1;

            for (int i = 0; i < wpcLength; i++)
            {
                var currentWPContainer = _waypointContainers[i];
                if (currentWPContainer.waypoints.Count > 1)
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

        public void LoadPedestrians() // One time
        {
            P_SpawnPointMatrix.Create();

            currentPedestrians = new List<PedestrianStateMachine>();
            _skipping = skippingInit;

            if (maxPedestriansWithPlayer == 0)
            {
                Debug.LogError("You need to set the maximum number of pedestrians in the Traffic System");
                return;
            }

            if (!_isGameStarted) _waypointContainers = FindObjectsOfType<FCGPedestrianWaypointsContainer>();

            int n = _waypointContainers.Length;
            for (int i = 0; i < n; i++)
                if (_waypointContainers[i].transform.childCount == 0)
                    DestroyImmediate(_waypointContainers[i].gameObject);  // Destroy Empty 

            UpdateAllWayPoints();

            nPedestrians = currentPedestrians.Count;

            DeffineDirection();

            _wpDataSpawn = new List<WpDataSpawnPedestrian>();

            n = _waypointContainers.Length;

            for (int i = 0; i < n; i++)
            {
                var _w = _waypointContainers[i];
                if (_w.noPedestrian) continue;

                if (!_w.bloked && _w.waypoints.Count > 1)
                {
                    for (int nSide = 0; nSide <= 1; nSide++)
                    {
                        if ((!_w.oneway || _w.doubleLine) || (nSide == 1))
                        {
                            for (int node = 0; node < _w.waypoints.Count - 1; node++)
                            {
                                float dist = Vector3.Distance(_w.Node(nSide, node), _w.Node(nSide, node + 1));

                                if (dist >= minNodeDistanceToCreate)
                                {
                                    if (isClassic)
                                    {
                                        PlaceSpawnPoint(_w, nSide, node, dist * 0.5f);
                                    }
                                    else
                                    {
                                        if (counterForSkipping % _skipping == 0)
                                        {
                                            counterForSkipping = 0;
                                            PlaceSpawnPoint(_w, nSide, node, dist * 0.5f);

                                            _skipping = UnityEngine.Random.Range(1, skippingInit + 1);
                                        }
                                        counterForSkipping++;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (Application.isPlaying)
            {
                InvokeRepeating(nameof(LoadPedestrians2_CR), 0f, intervalLoadPedestrian);
            }
            else
            {
                _isFirstTime = false;
                LoadPedestrians2();
                _isFirstTime = true;
            }
        }

        private void PlaceSpawnPoint(FCGPedestrianWaypointsContainer f, int side, int node, float locate)
        {
            var wpDSP = new WpDataSpawnPedestrian
            {
                locateZ = locate,
                position = f.AvanceNode(side, node, locate),
                rotation = f.NodeRotation(side, node),
                side = side,
                node = node,
                wayScript = f
            };

            P_SpawnPointMatrix.AssignItemToCell(wpDSP);

            _wpDataSpawn.Add(wpDSP);
        }

        public void DeffineDirection()
        {
            //Inverse Nodes
            for (int i = 0; i < _waypointContainers.Length; i++)
                _waypointContainers[i].InvertNodesDirection();

            UpdateAllWayPoints();
        }




        [Space(15)]
        [Header("Traffic Pedestrian Checker")]
        [SerializeField] bool isPedestrianCheckerActive = true;
        [SerializeField] float pedestrianCheckRadius = 5.0f;
        public bool ThereIsNoTrafficPedestrianInCheckRadius(Vector3 position, Transform atualWay, int sideAtual)
        {
            if (!isPedestrianCheckerActive) return false;
            foreach (var p in currentPedestrians)
            {
                var inRange = Vector3.Distance(p.transform.position, position) < pedestrianCheckRadius;

                if (inRange)
                {
                    if (atualWay == p.atualWay && sideAtual == p.sideAtual)
                    {
                        return true;
                    }
                }
            }

            return false;
        }














        List<WpDataSpawnPedestrian> _playerAroundSpawnPoints = new List<WpDataSpawnPedestrian>();
        List<WpDataSpawnPedestrian> _playerAroundSpawnPointsAvailable = new List<WpDataSpawnPedestrian>();
        private void FindCurrentSpawnPoints()
        {
            P_SpawnPointMatrix.ToggleCellAndNeighbours(player.transform, ref _playerAroundSpawnPoints);
        }

        public void LoadPedestrians2() // Renaissence
        {
            if (!player) return;

            nPedestrians = currentPedestrians.Count;
            if (nPedestrians >= maxPedestriansWithPlayer) return;

            _aroundMinCurrent = _isFirstTime ? aroundMinStart : aroundMin;
            _aroundMaxCurrent = _isFirstTime ? aroundMaxStart : aroundMax;

            FindCurrentSpawnPoints();

            _playerAroundSpawnPointsAvailable.Clear();

            int n = _playerAroundSpawnPoints.Count;
            for (int j = 0; j < n; j++)
            {
                int i = j;
                var wpDataSpawn = _playerAroundSpawnPoints[i];

                if (IsSpawnPointWillAddTo_PlayerAroundAvailable(wpDataSpawn))
                {
                    _playerAroundSpawnPointsAvailable.Add(wpDataSpawn);
                }
            }

            _playerAroundSpawnPointsAvailable.Shuffle();

            n = _playerAroundSpawnPointsAvailable.Count;
            PedestrianStateMachine p;
            for (int i = 0; i < n; i++)
            {
                if (currentPedestrians.Count >= maxPedestriansWithPlayer)
                {
                    break;
                }
                else
                {
                    var wpDataSpawn = _playerAroundSpawnPointsAvailable[i];

                    if (!ThereIsNoTrafficPedestrianInCheckRadius(wpDataSpawn.position, wpDataSpawn.wayScript.transform, wpDataSpawn.side))
                    {
                        p = CKY_PoolManager.Spawn(pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Length)], wpDataSpawn.position, wpDataSpawn.rotation).GetComponent<PedestrianStateMachine>();

                        AddToCurrentPedestrians(p);

                        p.TrafficSystemInit(wpDataSpawn.side, wpDataSpawn.wayScript.transform, wpDataSpawn.wayScript, wpDataSpawn.node + 1, pedestrianDestroyDistance, player, this);

                        nPedestrians++;
                    }
                }
            }

            if (nPedestrians > 0) _isFirstTime = false;

        }
        private bool IsSpawnPointWillAddTo_PlayerAroundAvailable(WpDataSpawnPedestrian wpDataSpawn)
        {
            var posData = wpDataSpawn.position; posData.y = 0.0f;
            var posPlayer = player.position; posPlayer.y = 0.0f;
            float dist = Vector3.Distance(posData, posPlayer);

            if (dist < _aroundMinCurrent || dist > _aroundMaxCurrent) return false;
            //if (isFOVImportant) if (InTheFieldOfVision(posPlayer, posData)) return false;

            return true;
        }








        public void LoadPedestrians2_CR() // Renaissence
        {
            if (!player) return;

            nPedestrians = currentPedestrians.Count;
            if (nPedestrians >= maxPedestriansWithPlayer) return;

            _aroundMinCurrent = _isFirstTime ? aroundMinStart : aroundMin;
            _aroundMaxCurrent = _isFirstTime ? aroundMaxStart : aroundMax;

            FindCurrentSpawnPoints();

            _playerAroundSpawnPointsAvailable.Clear();

            int n = _playerAroundSpawnPoints.Count;
            for (int j = 0; j < n; j++)
            {
                int i = j;
                var wpDataSpawn = _playerAroundSpawnPoints[i];

                if (IsSpawnPointWillAddTo_PlayerAroundAvailable(wpDataSpawn))
                {
                    _playerAroundSpawnPointsAvailable.Add(wpDataSpawn);
                }
            }


            _playerAroundSpawnPointsAvailable.Shuffle();

            StartCoroutine(C());

            if (nPedestrians > 0) _isFirstTime = false;

        }

        IEnumerator C()
        {
            var v = new WaitForSeconds(0.06f);
            yield return null;

            var n = _playerAroundSpawnPointsAvailable.Count;
            PedestrianStateMachine p;
            for (int i = 0; i < n; i++)
            {
                if (nPedestrians >= maxPedestriansWithPlayer)
                {
                    yield break;
                }
                else
                {
                    if (i >= n)
                    {
                        yield break;
                    }

                    var wpDataSpawn = _playerAroundSpawnPointsAvailable[i];

                    if (!ThereIsNoTrafficPedestrianInCheckRadius(wpDataSpawn.position, wpDataSpawn.wayScript.transform, wpDataSpawn.side))
                    {
                        p = CKY_PoolManager.Spawn(pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Length)], wpDataSpawn.position, wpDataSpawn.rotation).GetComponent<PedestrianStateMachine>();

                        AddToCurrentPedestrians(p);

                        p.TrafficSystemInit(wpDataSpawn.side, wpDataSpawn.wayScript.transform, wpDataSpawn.wayScript, wpDataSpawn.node + 1, pedestrianDestroyDistance, player, this);

                        nPedestrians++;

                        yield return v;
                    }
                }
            }
        }

        private void AddToCurrentPedestrians(PedestrianStateMachine p)
        {
            if (!currentPedestrians.Contains(p)) currentPedestrians.Add(p);
        }

        public void RemoveFromCurrentPedestrians(PedestrianStateMachine p)
        {
            if (currentPedestrians.Contains(p)) currentPedestrians.Remove(p);
        }













        #region Gizmos

        [Space(25)]
        [Header("Gizmos")]
        [SerializeField] float aroundCircleOffset_Y = 0.5f;
        [SerializeField] Color arounCircleColor = Color.black;
        void OnDrawGizmos()
        {
            if (!player) return;

            Gizmos.color = Color.black;
            DrawCircle(player.transform.position + new Vector3(0, aroundCircleOffset_Y, 0), aroundMin);
            DrawCircle(player.transform.position + new Vector3(0, aroundCircleOffset_Y, 0), aroundMax);
        }
        void DrawCircle(Vector3 center, float radius)
        {
            int segments = 50;
            float angleStep = 360f / segments;
            float currentAngle = 0f;
            Vector3 previousPoint = Vector3.zero;
            Vector3 firstPoint = Vector3.zero;

            for (int i = 0; i <= segments; i++)
            {
                float x = center.x + Mathf.Cos(Mathf.Deg2Rad * currentAngle) * radius;
                float z = center.z + Mathf.Sin(Mathf.Deg2Rad * currentAngle) * radius;
                Vector3 currentPoint = new Vector3(x, center.y, z);

                if (i > 0)
                {
                    Gizmos.DrawLine(previousPoint, currentPoint);
                }
                else
                {
                    firstPoint = currentPoint;
                }

                previousPoint = currentPoint;
                currentAngle += angleStep;
            }

            Gizmos.DrawLine(previousPoint, firstPoint);
        }

        #endregion

    }
}