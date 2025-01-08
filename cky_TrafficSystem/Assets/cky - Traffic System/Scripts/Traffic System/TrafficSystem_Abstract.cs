using System.Collections.Generic;
using System.Collections;
using cky.TrafficSystem;
using cky.GizmoHelper;
using UnityEngine;
using System;

public abstract class TrafficSystem_Abstract : MonoBehaviour
{
    [field: SerializeField] public Transform Player { get; protected set; }

    [Space(10)]
    [Header("Prefabs")]
    [SerializeField] protected Transform[] prefabs;

    [Space(5)]
    [Header("Creation")]
    [SerializeField] protected float intervalLoad = 1;
    [SerializeField] protected float minNodeDistanceToCreate = 10.0f;
    [SerializeField] protected float minUnitDistanceToCreate = 10.0f;
    [SerializeField] protected float distanceToRepeat = 40.0f;
    [Range(0, 1000)][SerializeField] protected float aroundMin = 80;
    [Range(0, 1000)][SerializeField] protected float aroundMax = 150;
    protected ArrayList _spawnPoints;

    [Space(5)]
    [SerializeField] protected int skipping = 1;
    [SerializeField] protected bool isClassic;
    protected int _counterForSkipping = 0;

    [Space(5)]
    [SerializeField] protected int nUnits = 0;
    [SerializeField] protected int maxUnitsWithPlayer = 50;

    [Space(5)]
    [SerializeField] protected bool _isStarted;

    [Space(15)]
    [Header("Checker")]
    [SerializeField] protected bool isCheckerActive = true;
    [SerializeField] protected float checkRadius = 5.0f;

    [Space(15)]
    [SerializeField] public List<ITrafficSystemUnit> currentUnits = new List<ITrafficSystemUnit>();

    protected bool isOnFirstCreationPart = true;

    [Space(15)]
    [Header("Gizmos")]
    [SerializeField] float gizmo_CircleOffset_Y = 0.5f;
    [SerializeField] int gizmo_CircleSegmentCount = 360;
    [SerializeField] Color gizmo_Color = Color.black;



    public abstract void UpdateAllWayPoints();

    public void SetPlayerAndStart(Transform playerTr)
    {
        Player = playerTr;

        LoadUnits();
    }

    private void Awake()
    {
        Player = GameObject.FindWithTag(TagHelper.Player)?.transform;
        if (Player == null) Player = Camera.main.transform;
    }

    void Start()
    {
        if (!Player)
            Debug.LogWarning("You have not set the player in the Traffic System on Inspector. This drastically decreases performance in big cities");
        else
            LoadUnits();
    }

    protected abstract void LoadUnits();
    protected abstract void CreateUnits();



    protected float ckyRandom(float b) => UnityEngine.Random.Range(-b, b);
    protected bool ckyPerThousand(int percentage)
    {
        if (percentage > UnityEngine.Random.Range(0, 1000))
            return true;

        return false;
    }

    protected bool ThereIsNoUnit_InCheckRadius(Vector3 position, Transform atualWay, int sideAtual)
    {
        if (!isCheckerActive) return false;
        foreach (var unit in currentUnits)
        {
            var inRange = Vector3.Distance(unit.Position, position) < checkRadius;

            if (inRange)
            {
                if (atualWay == unit.AtualWay && sideAtual == unit.SideAtual)
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected void AddToCurrentUnits(ITrafficSystemUnit unit)
    {
        if (!currentUnits.Contains(unit)) currentUnits.Add(unit);
    }

    public void RemoveFromCurrentUnits(ITrafficSystemUnit unit)
    {
        if (currentUnits.Contains(unit)) currentUnits.Remove(unit);
    }

    public abstract void DeffineDirection();



    #region Gizmos

    void OnDrawGizmos()
    {
        if (!Player) return;

        GizmoHelper_CKY.DrawCircle(Player.transform, new Vector3(0, gizmo_CircleOffset_Y, 0), aroundMin, gizmo_CircleSegmentCount, gizmo_Color);
        GizmoHelper_CKY.DrawCircle(Player.transform, new Vector3(0, gizmo_CircleOffset_Y, 0), aroundMax, gizmo_CircleSegmentCount, gizmo_Color);
    }

    #endregion
}
