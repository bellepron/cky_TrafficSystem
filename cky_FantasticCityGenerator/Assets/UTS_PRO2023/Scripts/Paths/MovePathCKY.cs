using UnityEngine;

public class MovePathCKY : MonoBehaviour
{
    Transform[] _exitWay;
    Transform[] _entryWay;
    int _index;
    bool _isEntryWay;

    CarAIController CarAIController;

    public void Init(Transform[] exitWay, Transform[] entryWay, bool isEntryWay)
    {
        _exitWay = exitWay;
        _entryWay = entryWay;
        _isEntryWay = isEntryWay;
    }

    private void Start()
    {
        if (TryGetComponent<CarAIController>(out var carAIController))
        {
            CarAIController = carAIController;
            carAIController.movePathCKY = this;
        }
        else
        {
            Debug.LogWarning("Hey I couln't reach CarAIController.cs");
        }
    }

    public void IncreaseIndex()
    {
        _index++;

        if (_isEntryWay)
        {
            if (_index == _entryWay.Length - 1)
            {
                CarAIController.movePathCKY = null;
                CarAIController.Enablee(false);
            }
        }
        else
        {
            if (_index == _exitWay.Length - 1)
            {
                CarAIController.movePathCKY = null;
            }
        }
    }

    public Vector3 TargetPosition()
    {
        if (_isEntryWay)
        {
            return _entryWay[_index].position;
        }
        else
        {
            return _exitWay[_index].position;
        }
    }

    //public void UpdateManually()
    //{

    //}
}