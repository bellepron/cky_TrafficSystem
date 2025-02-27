﻿using System.Collections.Generic;
using UnityEngine;
using CKY_Pooling;
using cky.Reuseables.Extension;

namespace cky.TrafficSystem
{
    #region Data Holder's

    [System.Serializable]
    public class WpData_Car
    {
        public bool[] tsActive;
        public Vector3[] tf01;
        public WaypointsContainer_Car[] tsParent;
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
        public WaypointsContainer_Car wayScript;
    }

    #endregion

    public class TrafficSystem_Car : TrafficSystem_Abstract
    {
        WaypointsContainer_Car[] _waypointContainers;

        List<WpDataSpawnCar> _wpDataSpawn;
        WpData_Car _wpData = new WpData_Car();

        protected override void LoadUnits()
        {
            currentUnits = new List<ITrafficSystemUnit>();

            if (maxUnitsWithPlayer == 0)
            {
                Debug.LogError("You need to set the maximum number of vehicles in the Traffic System");
                return;
            }

            if (!_isStarted)
            {
                _waypointContainers = FindObjectsOfType<WaypointsContainer_Car>();
                _isStarted = true;
            }

            int n = _waypointContainers.Length;
            for (int i = 0; i < n; i++)
                if (_waypointContainers[i].transform.childCount == 0)
                    DestroyImmediate(_waypointContainers[i].gameObject);  // Destroy Empty 

            UpdateAllWayPoints();

            nUnits = currentUnits.Count;

            DeffineDirection();

            _wpDataSpawn = new List<WpDataSpawnCar>();

            n = _waypointContainers.Length;

            for (int i = 0; i < n; i++)
            {
                var _w = _waypointContainers[i];
                if (_w.noUnit) continue;

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

            if (Player && Application.isPlaying)
            {
                InvokeRepeating(nameof(CreateUnits), 0f, intervalLoad);
            }
            else
            {
                CreateUnits();
            }
        }

        protected override void CreateUnits()
        {
            if (!Player) return;

            nUnits = currentUnits.Count;

            ITrafficSystemUnit iUnit;

            int n = _wpDataSpawn.Count;

            _wpDataSpawn.Shuffle();

            var posPlayer = Player.position; posPlayer.y = 0.0f;
            for (int i = 0; i < n; i++)
            {
                if (nUnits >= maxUnitsWithPlayer)
                {
                    break;
                }
                else
                {
                    var wpDataSpawn = _wpDataSpawn[i];

                    var posData = wpDataSpawn.position; posData.y = 0.0f;
                    float dist = Vector3.Distance(posData, posPlayer);

                    if (isOnFirstCreationPart)
                    {
                        if (dist > aroundMin) continue;
                    }
                    else
                    {
                        if (dist < aroundMin || dist > aroundMax) continue;
                    }

                    var wpDataSpawn_Side = wpDataSpawn.side;
                    var wpDataSpawn_Node = wpDataSpawn.node;
                    var wpDataSpawn_WayScript = wpDataSpawn.wayScript;
                    var aw = wpDataSpawn_WayScript.transform;
                    var sa = wpDataSpawn_Side;
                    if (!ThereIsNoUnit_InCheckRadius(wpDataSpawn.position, aw, sa))
                    {
                        iUnit = CKY_PoolManager.Spawn(prefabs[Random.Range(0, prefabs.Length)], wpDataSpawn.position + Vector3.up * 0.1f, wpDataSpawn.rotation).GetComponent<ITrafficSystemUnit>();

                        AddToCurrentUnits(iUnit);

                        iUnit.TrafficSystemInit(sa, aw, wpDataSpawn_WayScript, wpDataSpawn_Node + 1, aroundMax, Player, this);

                        nUnits++;
                    }
                }
            }

            isOnFirstCreationPart = false;
        }



        public override void UpdateAllWayPoints()
        {
            _waypointContainers = FindObjectsOfType<WaypointsContainer_Car>();

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
            _wpData.tsParent = new WaypointsContainer_Car[wpcLength * 2];
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

        private void PlaceSpawnPoint(WaypointsContainer_Car f, int side, int node, float locate)
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

        public override void DeffineDirection()
        {
            //Inverse Nodes
            for (int i = 0; i < _waypointContainers.Length; i++)
                _waypointContainers[i].InvertNodesDirection();

            UpdateAllWayPoints();
        }
    }
}