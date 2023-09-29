using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using cky.UTS.Helpers;
using cky.FCG.Pedestrian;

public class SemaphoreMovementSide : MonoBehaviour
{
    private List<IStateMachine> pedestrians = new List<IStateMachine>();
    private bool arrowMoveState;
    private bool forwardMoveState;
    private bool peopleMoveState;

    public int PassersbiesOnCrosswalk => pedestrians.Count;
    public bool ArrowMoveState => arrowMoveState;
    public bool ForwardMoveState => forwardMoveState;
    public bool PeopleMoveState => peopleMoveState;

    public bool flicker { get; set; }

    [SerializeField] private ViewCarSemaphore[] carSemaphores;
    [SerializeField] private ViewPeopleSemaphore[] peopleSemaphores;


    private void Awake()
    {
        foreach (var semaphore in carSemaphores)
        {
            semaphore.OnCarGreenChanged += ChangeForwardMoveState;
            semaphore.OnArrowChanged += ChangeArrowMoveState;
        }

        foreach (var semaphore in peopleSemaphores)
        {
            semaphore.OnPeopleGreenChanged += ChangePeopleMoveState;
        }
    }

    private void ChangeForwardMoveState(bool state)
    {
        forwardMoveState = state;
    }

    private void ChangeArrowMoveState(bool state)
    {
        arrowMoveState = state;
    }

    private void ChangePeopleMoveState(bool state)
    {
        peopleMoveState = state;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IStateMachine>(out var p))
        {
            pedestrians.Add(p);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(TagHelper.TAG_PEOPLE))
        {
            if (other.TryGetComponent<IStateMachine>(out var p))
            {
                pedestrians.Add(p);

                p.IsInsideSemaphore = true;

                if (!peopleMoveState)
                {
                    p.IsRedSemaphore = true;
                }
                else
                {
                    p.IsRedSemaphore = false;
                }
            }
        }

        if (other.CompareTag(TagHelper.TAG_CAR))
        {
            if (other.TryGetComponent<CarAIController>(out var c))
            {
                c.INSIDE = true;
            }
        }

        //if (other.CompareTag(TagHelper.TAG_BCYCLE))
        //{
        //    if (other.TryGetComponent<BcycleGyroController>(out var b))
        //    {
        //        b.insideSemaphore = true;
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagHelper.TAG_CAR))
        {
            if (other.TryGetComponent<CarAIController>(out var c))
            {
                c.INSIDE = false;
            }
        }

        //if (other.CompareTag(TagHelper.TAG_BCYCLE))
        //{
        //    if (other.TryGetComponent<BcycleGyroController>(out var b))
        //    {
        //        b.insideSemaphore = false;
        //    }
        //}

        if (other.CompareTag(TagHelper.TAG_PEOPLE))
        {
            if (other.TryGetComponent<IStateMachine>(out var p))
            {
                StartCoroutine(StopInside(p));
                pedestrians.Remove(p);
            }
        }
    }

    IEnumerator StopInside(IStateMachine sm)
    {
        yield return new WaitForSeconds(0.2f);

        sm.IsInsideSemaphore = false;
        sm.IsRedSemaphore = false;

        //sm.State = sm.PreSet_State;
        //sm.ChangeState(sm.State);
    }
}