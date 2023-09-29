using cky.UTS.Car;
using cky.UTS.Helpers;
using UnityEngine;

[RequireComponent(typeof(CarMove))]
public class CarAIController : MonoBehaviour
{
    public Vector3 bcSize;
    public BoxCollider bc;
    public VehiclesAllow allow;
    private Rigidbody rigbody;
    private MovePath movePath;
    private CarMove carMove;
    [SerializeField] private float curMoveSpeed;
    [SerializeField] private float angleBetweenPoint;
    private float targetSteerAngle;
    private float upTurnTimer;
    private bool moveBrake;
    private bool isACar;
    private bool isABike;
    private bool hasTrailer;

    private float startSpeed;
    private bool insideSemaphore;
    public bool tempStop;

    public CarSettings settings;

    [SerializeField] private float moveSpeed;

    public float START_SPEED { get { return startSpeed; } private set { } }
    public bool INSIDE { get { return insideSemaphore; } set { insideSemaphore = value; } }
    public bool TEMP_STOP { get { return tempStop; } private set { } }

    public MovePathCKY movePathCKY;

    public void GetBoxSize()
    {
        BoxCollider[] box = GetComponentsInChildren<BoxCollider>();
        bc = isACar ? box[0] : box[1];
        bcSize = bc.bounds.size;
    }

    public void SetMovePathName(string name) => GetComponent<MovePath>().SetWayName(name);

    private void Awake()
    {
        rigbody = GetComponent<Rigidbody>();
        movePath = GetComponent<MovePath>();
        carMove = GetComponent<CarMove>();
    }

    private void Start()
    {
        // cky
        var randomness = Random.Range(-settings.speedRandomness, settings.speedRandomness);
        moveSpeed = settings.moveSpeedBase * (1 + randomness);
        moveSpeed = Mathf.Clamp(moveSpeed, 0, settings.maxSpeed);

        movePath.walkPointThreshold = settings.nextPointThreshold;

        startSpeed = moveSpeed;

        WheelCollider[] wheelColliders = GetComponentsInChildren<WheelCollider>();

        if (wheelColliders.Length > 2)
        {
            isACar = true;
        }
        else
        {
            isABike = true;
        }

        if (GetComponent<AddTrailer>())
        {
            hasTrailer = true;
        }
    }
    bool test;
    private void Update()
    {
        if (!enablee) MoveSpeedZero();

        PushRay();

        if (carMove != null && isACar) carMove.Move(curMoveSpeed, 0, 0);
    }

    private void FixedUpdate()
    {
        if (!enablee) MoveSpeedZero();

        if (movePathCKY != null) GetPathCKY();
        else GetPath();

        Drive();

        if (moveBrake)
        {
            moveSpeed = startSpeed * 0.5f;
        }
    }

    private bool enablee = true;
    public void Enablee(bool v) => enablee = v;
    private void MoveSpeedZero() => moveSpeed = 0;

    private void GetPathCKY()
    {
        var tr = transform;
        var rbTr = rigbody.transform;
        var movePathCkyTargetPos = movePathCKY.TargetPosition();
        var targetPos = new Vector3(movePathCkyTargetPos.x, rbTr.position.y, movePathCkyTargetPos.z);
        var reachPointDistance = Vector3.Distance(Vector3.ProjectOnPlane(GetMiddleRay(tr.position, tr.forward, tr.right, tr.up, bc.transform, bc.center), Vector3.up),
            Vector3.ProjectOnPlane(movePathCkyTargetPos, Vector3.up));

        if (reachPointDistance < settings.getPathReachDistance_CKY)
        {
            movePathCKY.IncreaseIndex();
        }

        targetPos.y = rbTr.position.y;

        if (!isACar)
        {
            Vector3 targetVector = targetPos - rbTr.position;

            if (targetVector != Vector3.zero)
            {
                Quaternion look = Quaternion.identity;

                look = Quaternion.Lerp(rbTr.rotation, Quaternion.LookRotation(targetVector),
                    Time.fixedDeltaTime * 4f);

                look.x = rbTr.rotation.x;
                look.z = rbTr.rotation.z;

                rbTr.rotation = look;
            }
        }
    }

    private void GetPath()
    {
        var tr = transform;
        var rbTr = rigbody.transform;
        var targetPos = new Vector3(movePath.finishPos.x, rbTr.position.y, movePath.finishPos.z);
        var richPointDistance = Vector3.Distance(Vector3.ProjectOnPlane(GetMiddleRay(tr.position, tr.forward, tr.right, tr.up, bc.transform, bc.center), Vector3.up),
            Vector3.ProjectOnPlane(movePath.finishPos, Vector3.up));

        if (richPointDistance < settings.getPathReachDistance_Way && ((movePath.loop) || (!movePath.loop && movePath.targetPoint > 0 && movePath.targetPoint < movePath.targetPointsTotal)))
        {
            if (movePath.forward)
            {
                if (movePath.targetPoint < movePath.targetPointsTotal)
                {
                    targetPos = movePath.walkPath.getNextPoint(movePath.w, movePath.targetPoint + 1);
                }
                else
                {
                    targetPos = movePath.walkPath.getNextPoint(movePath.w, 0);
                }

                targetPos.y = rbTr.position.y;
            }
            else
            {
                if (movePath.targetPoint > 0)
                {
                    targetPos = movePath.walkPath.getNextPoint(movePath.w, movePath.targetPoint - 1);
                }
                else
                {
                    targetPos = movePath.walkPath.getNextPoint(movePath.w, movePath.targetPointsTotal);
                }

                targetPos.y = rbTr.position.y;
            }
        }


        if (!isACar)
        {
            Vector3 targetVector = targetPos - rbTr.position;

            if (targetVector != Vector3.zero)
            {
                Quaternion look = Quaternion.identity;

                look = Quaternion.Lerp(rbTr.rotation, Quaternion.LookRotation(targetVector),
                    Time.fixedDeltaTime * 4f);

                look.x = rbTr.rotation.x;
                look.z = rbTr.rotation.z;

                rbTr.rotation = look;
            }
        }

        if (richPointDistance < settings.getPathReachDistance_WayForBrake)
        {
            if (movePath.nextFinishPos != Vector3.zero)
            {
                Vector3 targetDirection = movePath.nextFinishPos - tr.position;
                angleBetweenPoint = (Mathf.Abs(Vector3.SignedAngle(targetDirection, tr.forward, Vector3.up)));

                if (angleBetweenPoint > settings.maxAngleToMoveBreak)
                {
                    moveBrake = true;
                }
            }
        }
        else
        {
            moveBrake = false;
        }

        if (richPointDistance > movePath.walkPointThreshold)
        {
            if (Time.deltaTime > 0)
            {
                Vector3 velocity = movePath.finishPos - rbTr.position;

                if (!isACar)
                {
                    velocity.y = rigbody.velocity.y;
                    rigbody.velocity = new Vector3(velocity.normalized.x * curMoveSpeed, velocity.y, velocity.normalized.z * curMoveSpeed);
                }
                else
                {
                    velocity.y = rigbody.velocity.y;
                }
            }
        }
        else if (richPointDistance <= movePath.walkPointThreshold && movePath.forward)
        {
            if (movePath.targetPoint != movePath.targetPointsTotal)
            {
                GameObject go = null;

                foreach (var item in movePath.walkPath.pathPointTransform)
                {
                    if (item.transform.position == movePath.finishPos)
                    {
                        if (item.GetComponent<WalkPath>())
                        {
                            go = item;
                            break;
                        }
                    }
                }

                bool x = Random.Range(0, 2) == 0;

                movePath.targetPoint++;
                movePath.finishPos = movePath.walkPath.getNextPoint(movePath.w, movePath.targetPoint);

                if (movePath.targetPoint != movePath.targetPointsTotal)
                {
                    movePath.nextFinishPos = movePath.walkPath.getNextPoint(movePath.w, movePath.targetPoint + 1);
                }
            }
            else if (movePath.targetPoint == movePath.targetPointsTotal)
            {
                if (movePath.loop)
                {
                    movePath.finishPos = movePath.walkPath.getStartPoint(movePath.w);

                    movePath.targetPoint = 0;
                }
                else
                {
                    movePath.walkPath.SpawnPoints[movePath.w].AddToSpawnQuery(new MovePathParams { });
                    Destroy(gameObject);
                }
            }

        }
        else if (richPointDistance <= movePath.walkPointThreshold && !movePath.forward)
        {
            if (movePath.targetPoint > 0)
            {
                movePath.targetPoint--;

                movePath.finishPos = movePath.walkPath.getNextPoint(movePath.w, movePath.targetPoint);

                if (movePath.targetPoint > 0)
                {
                    movePath.nextFinishPos = movePath.walkPath.getNextPoint(movePath.w, movePath.targetPoint - 1);
                }
            }
            else if (movePath.targetPoint == 0)
            {
                if (movePath.loop)
                {
                    movePath.finishPos = movePath.walkPath.getNextPoint(movePath.w, movePath.targetPointsTotal);

                    movePath.targetPoint = movePath.targetPointsTotal;
                }
                else
                {
                    movePath.walkPath.SpawnPoints[movePath.w].AddToSpawnQuery(new MovePathParams { });
                    Destroy(gameObject);
                }
            }
        }
    }

    private void Drive()
    {
        CarWheels wheels = GetComponent<CarWheels>();

        if (tempStop)
        {
            if (hasTrailer)
            {
                curMoveSpeed = Mathf.Lerp(curMoveSpeed, 0.0f, Time.fixedDeltaTime * (settings.speedDecrease * 2.5f));
            }
            else
            {
                curMoveSpeed = Mathf.Lerp(curMoveSpeed, 0, Time.fixedDeltaTime * settings.speedDecrease);
            }

            if (curMoveSpeed < 0.15f)
            {
                curMoveSpeed = 0.0f;
            }
        }
        else
        {
            curMoveSpeed = Mathf.Lerp(curMoveSpeed, moveSpeed, Time.fixedDeltaTime * settings.speedIncrease);
        }

        CarOverturned();

        if (isACar)
        {
            for (int wheelIndex = 0; wheelIndex < wheels.WheelColliders.Length; wheelIndex++)
            {
                if (wheels.WheelColliders[wheelIndex].transform.localPosition.z > 0)
                {
                    Transform tr = transform;

                    if (movePathCKY == null)
                        wheels.WheelColliders[wheelIndex].steerAngle = Mathf.Clamp(CarWheelsRotation.AngleSigned(tr.forward, movePath.finishPos - tr.position, tr.up), -30.0f, 30.0f);
                    else
                        wheels.WheelColliders[wheelIndex].steerAngle = Mathf.Clamp(CarWheelsRotation.AngleSigned(tr.forward, movePathCKY.TargetPosition() - tr.position, tr.up), -90.0f, 90.0f);
                }
            }
        }

        if (rigbody.velocity.magnitude > curMoveSpeed)
        {
            rigbody.velocity = rigbody.velocity.normalized * curMoveSpeed;
        }
    }

    private void CarOverturned()
    {
        WheelCollider[] wheels = GetComponent<CarWheels>().WheelColliders;

        bool removal = false;
        int number = wheels.Length;

        foreach (var item in wheels)
        {
            if (!item.isGrounded)
            {
                number--;
            }
        }

        if (number == 0)
        {
            removal = true;
        }

        if (removal)
        {
            upTurnTimer += Time.deltaTime;
        }
        else
        {
            upTurnTimer = 0;
        }

        if (upTurnTimer > 3)
        {
            Destroy(gameObject);
        }
    }

    private void PushRay()
    {
        RaycastHit hit;

        var trPos = transform.position;
        var trForward = transform.forward;
        var trRight = transform.right;
        var trUp = transform.up;
        var bcTransform = bc.transform;
        var bcCenter = bc.center;

        Ray fwdRay = new Ray(GetMiddleRay(trPos, trForward, trRight, trUp, bcTransform, bcCenter), trForward * 20);
        Ray LRay = new Ray(GetLeftRay(trPos, trForward, trRight, trUp, bcTransform, bcCenter), trForward * 20);
        Ray RRay = new Ray(GetRightRay(trPos, trForward, trRight, trUp, bcTransform, bcCenter), trForward * 20);

        if (Physics.Raycast(fwdRay, out hit, 20) || Physics.Raycast(LRay, out hit, 20) || Physics.Raycast(RRay, out hit, 20))
        {
            float distance = Vector3.Distance(GetMiddleRay(trPos, trForward, trRight, trUp, bcTransform, bcCenter), hit.point);

            var hitTransform = hit.transform;

            if (hitTransform.CompareTag(TagHelper.TAG_CAR))
            {
                var isTrailer = hitTransform.GetComponentInParent<ParentOfTrailer>();

                var car = isTrailer ? isTrailer.PAR : hitTransform.gameObject;

                if (car.TryGetComponent<MovePath>(out var MP))
                {
                    //GameObject car = hitTransform.GetComponentInChildren<ParentOfTrailer>() ? hitTransform.GetComponent<ParentOfTrailer>().PAR : hit.transform.gameObject;

                    if (car != null)
                    {
                        if (MP.w == movePath.w)
                        {
                            ReasonsStoppingCars.CarInView(car, rigbody, distance, startSpeed, ref moveSpeed, ref tempStop, settings.distanceToCar);
                        }
                    }
                }
                else
                {
                    ReasonsStoppingCars.PlayerCarInView(hitTransform, distance, startSpeed, ref moveSpeed, ref tempStop);
                }
            }
            else if (hitTransform.CompareTag(TagHelper.TAG_BCYCLE))
            {
                ReasonsStoppingCars.BcycleGyroInView(hitTransform.GetComponentInChildren<BcycleGyroController>(), rigbody, distance, startSpeed, ref moveSpeed, ref tempStop);
            }
            else if (hitTransform.CompareTag(TagHelper.TAG_PEOPLESEMAPHORE))
            {
                ReasonsStoppingCars.SemaphoreInView(hitTransform.GetComponent<SemaphoreMovementSide>(), allow, distance, startSpeed, insideSemaphore, ref moveSpeed, ref tempStop, settings.distanceToSemaphore);
            }
            else if (hitTransform.CompareTag(TagHelper.TAG_PLAYER) || hitTransform.CompareTag(TagHelper.TAG_PEOPLE))
            {
                ReasonsStoppingCars.PlayerInView(hitTransform, distance, startSpeed, ref moveSpeed, ref tempStop);
            }
            else
            {
                if (!moveBrake)
                {
                    moveSpeed = startSpeed;
                }
                tempStop = false;
            }
        }
        else
        {
            if (!moveBrake)
            {
                moveSpeed = startSpeed;
            }

            tempStop = false;
        }
    }

    private Vector3 GetMiddleRay(Vector3 trPos, Vector3 trForward, Vector3 trRight, Vector3 trUp, Transform bcTransform, Vector3 bcCenter)
    {
        var forwardRay = trPos;
        forwardRay += trForward * (bcSize.z * 0.5f + bcTransform.localPosition.z);
        forwardRay += trUp * (bcTransform.localPosition.y + bcCenter.y);

        return forwardRay;
    }

    private Vector3 GetLeftRay(Vector3 trPos, Vector3 trForward, Vector3 trRight, Vector3 trUp, Transform bcTransform, Vector3 bcCenter)
    {
        var leftRay = trPos;
        leftRay += -transform.right * (bcSize.x * 0.5f + bcTransform.localPosition.x);
        leftRay += transform.forward * (bcSize.z * 0.5f + bcTransform.localPosition.z);
        leftRay += transform.up * (bcTransform.localPosition.y + bcCenter.y);

        return leftRay;
    }

    private Vector3 GetRightRay(Vector3 trPos, Vector3 trForward, Vector3 trRight, Vector3 trUp, Transform bcTransform, Vector3 bcCenter)
    {
        var rightRay = trPos;
        rightRay += trRight * (bcSize.x * 0.5f + bcTransform.localPosition.x);
        rightRay += trForward * (bcSize.z * 0.5f + bcTransform.localPosition.z);
        rightRay += trUp * (bcTransform.localPosition.y + bcCenter.y);

        return rightRay;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        var trPos = transform.position;
        var trForward = transform.forward;
        var trRight = transform.right;
        var trUp = transform.up;
        var bcTransform = bc.transform;
        var bcCenter = bc.center;

        if (bc != null)
        {
            var forwardRay = GetMiddleRay(trPos, trForward, trRight, trUp, bcTransform, bcCenter);

            Gizmos.DrawRay(forwardRay,
                trForward * 20);

            var rightRay = GetRightRay(trPos, trForward, trRight, trUp, bcTransform, bcCenter);

            Gizmos.DrawRay(rightRay,
                trForward * 20);

            var leftRay = GetLeftRay(trPos, trForward, trRight, trUp, bcTransform, bcCenter);

            Gizmos.DrawRay(leftRay,
                trForward * 20);
        }
    }
}