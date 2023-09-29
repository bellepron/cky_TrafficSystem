using UnityEngine;

public class MovePath : MonoBehaviour
{
    [HideInInspector] public float walkPointThreshold = 0.5f;
    /*[HideInInspector]*/ public WalkPath walkPath;
    public int w;
    public bool forward;
    [HideInInspector] public Vector3 finishPos;
    [HideInInspector] public Vector3 nextFinishPos = Vector3.zero;
    public int targetPoint;
    [HideInInspector] public int targetPointsTotal;
    [HideInInspector] public float randXFinish;
    [HideInInspector] public float randZFinish;
    [HideInInspector] public bool loop;

    [SerializeField] private string wayName;

    public void SetWayName(string name) => wayName = name;
    public string GetWayName() => wayName;

    public void InitStartPosition(int _w, int _i, bool _loop, bool _forward)
    {
        forward = _forward;

        var _WalkPath = walkPath;
        w = _w;
        targetPointsTotal = _WalkPath.getPointsTotal(0) - 2;

        loop = _loop;

        if (loop)
        {
            if (_i < targetPointsTotal && _i > 0)
            {
                if (forward)
                {
                    targetPoint = _i + 1;
                    finishPos = _WalkPath.getNextPoint(w, _i + 1);
                }
                else
                {
                    targetPoint = _i;
                    finishPos = _WalkPath.getNextPoint(w, _i);
                }
            }
            else
            {
                if (forward)
                {
                    targetPoint = 1;
                    finishPos = _WalkPath.getNextPoint(w, 1);
                }
                else
                {
                    targetPoint = targetPointsTotal;
                    finishPos = _WalkPath.getNextPoint(w, targetPointsTotal);
                }
            }

        }
        else
        {
            if (forward)
            {
                targetPoint = _i + 1;
                finishPos = _WalkPath.getNextPoint(w, _i + 1);
            }
            else
            {
                targetPoint = _i;
                finishPos = _WalkPath.getNextPoint(w, _i);
            }
        }

    }

    public void SetLookPosition()
    {
        Vector3 targetPos = new Vector3(finishPos.x, transform.position.y, finishPos.z);
        transform.LookAt(targetPos);
    }
}