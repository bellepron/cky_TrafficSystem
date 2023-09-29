using UnityEngine;
using System.Collections.Generic;
using cky.UTS.Car;

public class CarWalkPath : WalkPath
{
    [SerializeField] CarSettings carSettings;

    [HideInInspector][SerializeField][Tooltip("Ignore pedestrian colliders?")] private bool _ignorePeople = false;

    [SerializeField] private string parentName = "walkingObjects";
    [SerializeField] private string formatName = "SpawnPoint (Path {0})";

    //private void Start()
    //{
    //    //if (_ignorePeople)
    //    //{
    //    //    Physics.IgnoreLayerCollision(9, 8, true);
    //    //}
    //}

    public override void DrawCurved(bool withDraw, EnumDir direct = EnumDir.Forward)
    {
        base.DrawCurved(withDraw, direction);
    }

    public override void CreateSpawnPoints()
    {
        SpawnPoints = new SpawnPoint[points.GetLength(0)];

        for (int i = 0; i < points.GetLength(0); i++)
        {
            var startPoint = _forward[i] ? points[i, 0] : points[i, points.GetLength(1) - 1];
            var nextPoint = _forward[i] ? points[i, 2] : points[i, points.GetLength(1) - 3];

            SpawnPoints[i] = SpawnPoint.CarCreate(
                string.Format(formatName, i + 1),
                startPoint,
                nextPoint,
                lineSpacing,
                i,
                _forward[i],
                this,
                3f,
                10f
            );
        }
    }

    public override void SpawnOnePeople(int w, bool forward)
    {
        List<GameObject> pfb = new List<GameObject>(walkingPrefabs);

        for (int i = pfb.Count - 1; i >= 0; i--)
        {
            if (pfb[i] == null)
            {
                pfb.RemoveAt(i);
            }
        }

        walkingPrefabs = pfb.ToArray();
        int prefabNum = UnityEngine.Random.Range(0, walkingPrefabs.Length);
        var people = gameObject;

        if (!forward)
        {
            people = Instantiate(walkingPrefabs[prefabNum], points[w, pointLength[0] - 2], Quaternion.identity) as GameObject;
        }
        else
        {
            people = Instantiate(walkingPrefabs[prefabNum], points[w, 1], Quaternion.identity) as GameObject;
        }

        var movePath = people.AddComponent<MovePath>();
        var car = people.AddComponent<CarAIController>();
        car.GetBoxSize();

        CarInitialize(ref car);

        people.transform.parent = par.transform;
        movePath.walkPath = this;

        if (!forward)
        {
            movePath.InitStartPosition(w, pointLength[0] - 3, loopPath, forward);
            people.transform.LookAt(points[w, pointLength[0] - 3]);
        }
        else
        {
            movePath.InitStartPosition(w, 1, loopPath, forward);
            people.transform.LookAt(points[w, 2]);
        }
    }

    public override void SpawnPeople() // Editör
    {
        if (carSettings == null)
        {
            carSettings = Resources.Load<CarSettings>("CarSettings/DefaultAICarSettings");
        }

        List<GameObject> pfb = new List<GameObject>(walkingPrefabs);

        for (int i = pfb.Count - 1; i >= 0; i--)
        {
            if (pfb[i] == null)
            {
                pfb.RemoveAt(i);
            }
        }

        walkingPrefabs = pfb.ToArray();

        if (points == null) DrawCurved(false);

        if (par == null)
        {
            par = new GameObject();
            par.transform.parent = gameObject.transform;
            par.name = parentName;
        }

        int pathPointCount;

        if (!loopPath)
        {
            pathPointCount = pointLength[0] - 2;
        }
        else
        {
            pathPointCount = pointLength[0] - 1;
        }

        if (pathPointCount < 2) return;

        var pCount = loopPath ? pointLength[0] - 1 : pointLength[0] - 2;

        for (int wayIndex = 0; wayIndex < numberOfWays; wayIndex++)
        {
            _distances = new float[pCount];

            float pathLength = 0f;

            for (int i = 1; i < pCount; i++)
            {
                Vector3 vector;
                if (loopPath && i == pCount - 1)
                {
                    vector = points[wayIndex, 1] - points[wayIndex, pCount];
                }
                else
                {
                    vector = points[wayIndex, i + 1] - points[wayIndex, i];
                }

                pathLength += vector.magnitude;
                _distances[i] = pathLength;
            }

            bool forward = false;

            switch (direction)
            {
                case EnumDir.Forward:
                    forward = true;
                    break;
                case EnumDir.Backward:
                    forward = false;
                    break;
                case EnumDir.HugLeft:
                    forward = (wayIndex + 2) % 2 == 0;
                    break;
                case EnumDir.HugRight:
                    forward = (wayIndex + 2) % 2 != 0;
                    break;
                case EnumDir.WeaveLeft:
                    forward = wayIndex != 1 && wayIndex != 2 && (wayIndex - 1) % 4 != 0 && (wayIndex - 2) % 4 != 0;
                    break;
                case EnumDir.WeaveRight:
                    forward = wayIndex == 1 || wayIndex == 2 || (wayIndex - 1) % 4 == 0 || (wayIndex - 2) % 4 == 0;
                    break;
            }

            int peopleCount = Mathf.FloorToInt((Density * pathLength) / _minimalObjectLength * 0.2f);
            float segmentLen = _minimalObjectLength + (pathLength - (peopleCount * _minimalObjectLength)) / peopleCount;

            int[] pickList = CommonUtils.GetRandomPrefabIndexes(peopleCount, ref walkingPrefabs);

            Vector3[] pointArray = new Vector3[_distances.Length];

            for (int i = 1; i < _distances.Length; i++)
            {
                pointArray[i - 1] = points[wayIndex, i];
            }

            pointArray[_distances.Length - 1] = loopPath ? points[wayIndex, 1] : points[wayIndex, _distances.Length];

            for (int peopleIndex = 0; peopleIndex < peopleCount; peopleIndex++)
            {
                var people = gameObject;
                var randomShift = UnityEngine.Random.Range(-segmentLen / 3f, segmentLen / 3f) + (wayIndex * segmentLen);
                var finalRandomDistance = (peopleIndex + 1) * segmentLen + randomShift;

                var routePosition = GetRoutePosition(pointArray, finalRandomDistance, pCount, loopPath);
                Vector3 or;

                RaycastHit[] rrr = Physics.RaycastAll(or = new Vector3(routePosition.x, routePosition.y + 10000, routePosition.z), Vector3.down, Mathf.Infinity);

                bool isSemaphore = false;

                for (int i = 0; i < rrr.Length; i++)
                {
                    var col = rrr[i].collider;
                    if (col.GetComponent<SemaphoreSystem>() != null ||
                        col.GetComponent<SemaphoreMovementSide>() != null ||
                        col.GetComponent<TSemaphoreSystem>() != null)
                    {
                        isSemaphore = true;
                    }
                }

                if (isSemaphore) continue;

                float dist = 0;
                int bestCandidate = 0;

                rrr = Physics.RaycastAll(or = new Vector3(routePosition.x, routePosition.y + highToSpawn, routePosition.z), Vector3.down, Mathf.Infinity);

                for (int i = 0; i < rrr.Length; i++)
                {
                    var rrrPoint = rrr[0].point;
                    if (dist < Vector3.Distance(rrrPoint, or))
                    {
                        bestCandidate = i;
                        dist = Vector3.Distance(rrrPoint, or);
                    }
                }

                if (rrr.Length > 0)
                {
                    routePosition.y = rrr[bestCandidate].point.y;
                }

                people = Instantiate(walkingPrefabs[pickList[peopleIndex]], routePosition, Quaternion.identity) as GameObject;

                var movePath = people.AddComponent<MovePath>();
                var car = people.AddComponent<CarAIController>();
                car.GetBoxSize();

                CarInitialize(ref car);

                people.transform.parent = par.transform;
                movePath.walkPath = this;

                movePath.InitStartPosition(wayIndex,
                    GetRoutePoint((peopleIndex + 1) * segmentLen + randomShift, wayIndex, pCount, forward, loopPath), loopPath, forward);

                movePath.SetLookPosition();

                if (people.TryGetComponent<AddTrailer>(out var addTrailer))
                {
                    addTrailer.Init();
                }
            }
        }
    }

    private void CarInitialize(ref CarAIController carAIController)
    {
        carAIController.settings = carSettings;
        carAIController.allow = allow;
        carAIController.SetMovePathName(transform.name);
    }
}