//using cky.Car;
using cky.FCG.Pedestrian;
using cky.FCG.Pedestrian.StateMachine;
using cky.Ragdoll;
using cky.StateMachine.Base;
using cky.UTS.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public enum PasserbyStates
    {
        Empty, Idle, Walk, Run, WaitTaxi, MovetoTaxi, LeanToTaxi, EnterCar,
        InCar, ExitCar, GetInTaxi, GetOutTaxi, Death, TaxiDialogue, Dialogue, WalkWithoutCheckingCar
    }
    public class PasserbyStateMachine : BaseStateMachine, IStateMachine
    {
        [field: SerializeField] public PasserbySettings Settings { get; set; }

        [field: SerializeField] public PasserbyStates State { get; set; }
        [field: SerializeField] public PasserbyStates PreSet_State { get; set; }
        public PasserbyBaseState CurrentPasserby_State { get; set; }

        [field: SerializeField] public MovePath MovePath { get; set; }
        [field: SerializeField] public PasserbyAnimatorController AnimatorController { get; set; }
        [field: SerializeField] public RagdollController RagdollController { get; set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; set; }

        public PasserbyStateMachine NearPasserby { get; set; }
        public SemaphoreMovementSide NearSemaphore { get; set; }
        public Transform NearCar { get; set; }
        public Transform NearPlayer { get; set; }

        public float CurrentMoveSpeed { get; set; }
        public float TargetMoveSpeed { get; private set; }

        public bool IsInsideSemaphore { get; set; }
        public bool IsRedSemaphore { get; set; }

        [SerializeField] Collider[] targetsInViewRadius;
        [SerializeField] List<PasserbyStateMachine> _passersby = new List<PasserbyStateMachine>();
        [SerializeField] List<SemaphoreMovementSide> _semaphores = new List<SemaphoreMovementSide>();
        [SerializeField] List<Transform> _cars = new List<Transform>();
        [SerializeField] List<Transform> _players = new List<Transform>();

        public string WayName;

        [HideInInspector] public Transform LeanTaxiTransform;

        [HideInInspector] public float MoveSpeedBlendValue;
        [HideInInspector] public float MoveSpeedBlendSpeed;
        [HideInInspector] public bool IsAlive { get; set; } = true;
        [HideInInspector] public bool IsCurrentlyChoosen { get; set; } = false;
        [HideInInspector] public bool WasChoosen { get; set; } = false;

        [HideInInspector] public float IsIdle = 0.0f; // Hareket etmek için sýrada bekleyen yayalar sýrayla harekete baþlasýn diye.

        public void ChangeWithInputHandler() => SwitchState(new Idle_State(this));
        public void SetTargetMoveSpeed(float value) => TargetMoveSpeed = value;



        public void Init(PasserbySettings settings, MovePath movePath, string wayName)
        {
            Settings = settings;
            State = settings.state;
            PreSet_State = State;

            MovePath = movePath;
            MovePath.SetWayName(wayName);
            WayName = wayName;

            Rigidbody = GetComponent<Rigidbody>();
            RagdollController = GetComponent<RagdollController>();
            AnimatorController = GetComponent<PasserbyAnimatorController>();

            MoveSpeedBlendSpeed = 2.0f;
        }



        public void OpenCarDoor_AnimEvent()
        {
            //DriverSingleton.Instance.CharacterData.CarData.doors[1].OpenCarDoorViaNPC();
        }
        public void CloseCarDoor_AnimEvent()
        {
            //DriverSingleton.Instance.CharacterData.CarData.doors[1].CloseCarDoorViaNPC();
        }


        public void EnableCollisions(bool b)
        {
            GetComponent<Collider>().enabled = b;
            Rigidbody.detectCollisions = b;
        }


        private void Start()
        {
            CurrentPasserby_State = CreateNewStateByEnum(State);
            SwitchState(CurrentPasserby_State);
        }


        protected override void Tick()
        {
            base.Tick();
        }


        protected override void FixedTick()
        {
            base.FixedTick();
        }


        public void ChangeState(PasserbyStates state)
        {
            State = state;
            CurrentPasserby_State = CreateNewStateByEnum(State);
            SwitchState(CurrentPasserby_State);
        }


        #region Create New State By Enum

        private PasserbyBaseState CreateNewStateByEnum(PasserbyStates state)
        {
            switch (state)
            {
                case PasserbyStates.Empty: return new Empty_State(this);
                case PasserbyStates.Idle: return new Idle_State(this);
                case PasserbyStates.Walk: return new Walk_State(this);
                case PasserbyStates.Run: return new Run_State(this);

                case PasserbyStates.WaitTaxi: return new WaitTaxi_State(this);
                case PasserbyStates.MovetoTaxi: return new MoveToTaxi_State(this);
                case PasserbyStates.LeanToTaxi: return new LeanToTaxi_State(this);
                case PasserbyStates.EnterCar: return new EnterCar_State(this);
                case PasserbyStates.InCar: return new InCar_State(this);
                case PasserbyStates.ExitCar: return new ExitCar_State(this);

                case PasserbyStates.GetInTaxi: return new GetInTaxi_State(this);
                case PasserbyStates.GetOutTaxi: return new GetOutTaxi_State(this);

                case PasserbyStates.Death: return new Death_State(this);

                case PasserbyStates.TaxiDialogue: return new TaxiDialogue_State(this);
                case PasserbyStates.Dialogue: return new Dialogue_State(this);
                case PasserbyStates.WalkWithoutCheckingCar: return new WalkWithoutCheckingCar_State(this);

                default:
                    {
                        Debug.Log("You need to check this!");
                        return new Walk_State(this);
                    }
            }
        }

        #endregion


        #region Get Path Function
        public void GetPath(float fixedDeltaTime)
        {
            if (Rigidbody == null) { Rigidbody = GetComponent<Rigidbody>(); }
            var rbPos = Rigidbody.transform.position;
            var rbPosY = rbPos.y;
            var thisTr = transform;

            Vector3 randFinishPos = new Vector3(MovePath.finishPos.x + MovePath.randXFinish, MovePath.finishPos.y, MovePath.finishPos.z + MovePath.randZFinish);

            Vector3 targetPos = new Vector3(randFinishPos.x, rbPosY, randFinishPos.z);

            var richPointDistance = Vector3.Distance(Vector3.ProjectOnPlane(rbPos, Vector3.up), Vector3.ProjectOnPlane(randFinishPos, Vector3.up));

            if (richPointDistance < 0.2f && State == PasserbyStates.Walk && ((MovePath.loop) || (!MovePath.loop && MovePath.targetPoint > 0 && MovePath.targetPoint < MovePath.targetPointsTotal)))
            {
                if (MovePath.forward)
                {
                    if (MovePath.targetPoint < MovePath.targetPointsTotal)
                        targetPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPoint + 1);
                    else
                        targetPos = MovePath.walkPath.getNextPoint(MovePath.w, 0);
                    targetPos.y = rbPosY;
                }
                else
                {
                    if (MovePath.targetPoint > 0)
                        targetPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPoint - 1);
                    else
                        targetPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPointsTotal);
                    targetPos.y = rbPosY;
                }
            }

            if (richPointDistance < 0.5f && State == PasserbyStates.Run && ((MovePath.loop) || (!MovePath.loop && MovePath.targetPoint > 0 && MovePath.targetPoint < MovePath.targetPointsTotal)))
            {
                if (MovePath.forward)
                {
                    if (MovePath.targetPoint < MovePath.targetPointsTotal)
                        targetPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPoint + 1);
                    else
                        targetPos = MovePath.walkPath.getNextPoint(MovePath.w, 0);
                    targetPos.y = rbPosY;
                }
                else
                {
                    if (MovePath.targetPoint > 0)
                        targetPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPoint - 1);
                    else
                        targetPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPointsTotal);
                    targetPos.y = rbPosY;
                }
            }

            Vector3 direction = targetPos - thisTr.position;

            if (direction != Vector3.zero)
            {
                Vector3 newDir = Vector3.zero;

                Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                newDir = Vector3.RotateTowards(thisTr.forward, direction, Settings.moveRotatitonSpeed * fixedDeltaTime, 0.0f);

                //transform.rotation = Quaternion.LookRotation(newDir);
                thisTr.rotation = Quaternion.Slerp(thisTr.rotation, Quaternion.LookRotation(newDir), Settings.moveRotatitonSpeed * fixedDeltaTime);
            }

            if (richPointDistance > MovePath.walkPointThreshold)
            {
                if (Time.deltaTime > 0)
                {
                    thisTr.position += thisTr.forward * CurrentMoveSpeed * fixedDeltaTime;
                    //Rigidbody.MovePosition(thisTr.position + thisTr.forward * CurrentMoveSpeed * fixedDeltaTime);
                }
            }
            else if (richPointDistance <= MovePath.walkPointThreshold && MovePath.forward)
            {

                if (MovePath.targetPoint != MovePath.targetPointsTotal)
                {
                    MovePath.targetPoint++;

                    MovePath.finishPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPoint);
                }
                else if (MovePath.targetPoint == MovePath.targetPointsTotal)
                {
                    if (MovePath.loop)
                    {
                        MovePath.finishPos = MovePath.walkPath.getStartPoint(MovePath.w);

                        MovePath.targetPoint = 0;
                    }
                    else
                    {
                        if (MovePath != null)
                        {
                            MovePath.walkPath.SpawnPoints[MovePath.w].AddToSpawnQuery(new MovePathParams { });
                        }

                        Destroy(gameObject);
                    }
                }

            }
            else if (richPointDistance <= MovePath.walkPointThreshold && !MovePath.forward)
            {
                if (MovePath.targetPoint > 0)
                {
                    MovePath.targetPoint--;

                    MovePath.finishPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPoint);
                }
                else if (MovePath.targetPoint == 0)
                {
                    if (MovePath.loop)
                    {
                        MovePath.finishPos = MovePath.walkPath.getNextPoint(MovePath.w, MovePath.targetPointsTotal);

                        MovePath.targetPoint = MovePath.targetPointsTotal;
                    }
                    else
                    {
                        MovePath.walkPath.SpawnPoints[MovePath.w].AddToSpawnQuery(new MovePathParams { });
                        Destroy(gameObject);
                    }
                }
            }
        }
        #endregion

        #region Vision

        #region AI Sight
        public void AISight()
        {
            ClearLists();

            targetsInViewRadius = Physics.OverlapSphere(transform.position, Settings.viewRadius + IsIdle, Settings.targetMask);
            _passersby = new List<PasserbyStateMachine>();
            _semaphores = new List<SemaphoreMovementSide>();
            _cars = new List<Transform>();
            _players = new List<Transform>();

            SortTargetsInView_Passerby_Semaphore_Car_Player(targetsInViewRadius);
        }

        private void ClearLists()
        {
            //NearPasserby = null; NearSemaphore = null; NearCar = null; NearPlayer = null;
            _passersby.Clear(); _semaphores.Clear(); _cars.Clear(); _players.Clear();
        }

        public void RunIfNeed()
        {
            if (IsInsideSemaphore)
            {
                if (IsRedSemaphore)
                {
                    State = PasserbyStates.Run;
                    ChangeState(State);
                }
            }
        }

        public void IfCanReturnToNormalMove()
        {
            if (NearPasserby == null && NearSemaphore == null && NearCar == null && NearPlayer == null)
            {
                if (!IsInsideSemaphore && !IsRedSemaphore)
                {
                    State = PreSet_State;
                    ChangeState(State);
                }
            }
        }

        #endregion


        #region Sort
        private void SortTargetsInView_Passerby_Semaphore_Car_Player(Collider[] targetsInViewRadius)
        {
            Transform thisTransform = transform;
            Vector3 thisPosition = thisTransform.position;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            var halfViewAngle = Settings.viewAngle * 0.5f;

            foreach (Collider c in targetsInViewRadius)
            {
                Transform cTransform = c.transform;
                Vector3 target = cTransform.position - thisPosition;

                if (cTransform.CompareTag(TagHelper.TAG_PEOPLE))
                {
                    if (cTransform.position != thisPosition)
                    {
                        MovePath cMovePath = c.GetComponent<MovePath>();

                        if (cMovePath.GetWayName() != WayName) { /*Debug.Log($"{cMovePath.GetWayName()} {WayName}");*/ continue; }
                        if (cMovePath.w != MovePath.w) { /*Debug.Log($"{cMovePath.w} {MovePath.w}");*/ continue; }
                        if (cMovePath.forward != MovePath.forward) { /*Debug.Log($"{cMovePath.forward} {MovePath.forward}");*/ continue; }

                        if (Vector3.Dot(forward, target) > 0)
                            _passersby.Add(c.GetComponent<PasserbyStateMachine>());

                        NearTarget_Passerby(thisPosition, forward, halfViewAngle);
                    }
                }
                else if (cTransform.CompareTag(TagHelper.TAG_PEOPLESEMAPHORE))
                {
                    if (Vector3.Dot(forward, target) > 0)
                        _semaphores.Add(c.GetComponent<SemaphoreMovementSide>());

                    NearTarget_SemaphoreMovementSide(thisPosition, forward, halfViewAngle);
                }
                else if (cTransform.CompareTag(TagHelper.TAG_CAR))
                {
                    if (Vector3.Dot(forward, target) > 0)
                        _cars.Add(c.GetComponent<Transform>());

                    NearTarget_Car(thisPosition, forward, halfViewAngle);
                }
                else if (cTransform.CompareTag(TagHelper.TAG_PLAYER))
                {
                    if (Vector3.Dot(forward, target) > 0)
                        _players.Add(c.GetComponent<Transform>());

                    NearTarget_Player(thisPosition, forward, halfViewAngle);
                }
            }

            if (_passersby.Count == 0) NearPasserby = null;
            if (_semaphores.Count == 0) NearSemaphore = null;
            if (_cars.Count == 0) NearCar = null;
            if (_players.Count == 0) NearPlayer = null;
        }
        #endregion


        #region NearTarget
        Vector3 _rayOffsetH = new Vector3(0, 1, 0);

        private void NearTarget_Passerby(Vector3 thisPosition, Vector3 transformForward, float halfViewAngle)
        {
            foreach (var p in _passersby)
            {
                Transform passerbyTr = p.transform;
                Vector3 passerbyPos = passerbyTr.position;
                Vector3 dir = (passerbyPos - thisPosition); dir.y = 0.0f; dir.Normalize();

                if (Vector3.Angle(transformForward, dir) < halfViewAngle)
                {
                    float distToPasserby = Vector3.Distance(thisPosition, passerbyPos); float distToNearPasserby = Mathf.Infinity;

                    if (distToPasserby > Settings.distToPasserby) continue;

                    if (!Physics.Raycast(thisPosition + _rayOffsetH, dir, distToPasserby, Settings.obstacleMask))
                    {
                        if (NearPasserby == null)
                        {
                            distToNearPasserby = distToPasserby;
                            NearPasserby = p;
                            continue;
                        }
                        else
                        {
                            if (distToNearPasserby < distToPasserby)
                            {
                                NearPasserby = p;
                            }
                        }
                    }
                }
            }
        }
        private void NearTarget_SemaphoreMovementSide(Vector3 thisPosition, Vector3 transformForward, float halfViewAngle)
        {
            foreach (var s in _semaphores)
            {
                Transform semaphoreTr = s.transform;
                Vector3 semaphorePos = semaphoreTr.position;

                Vector3 dir = (semaphorePos - thisPosition); dir.y = 0.0f; dir.Normalize();

                if (Vector3.Angle(transformForward, dir) < halfViewAngle)
                {
                    float distToSemaphore = Vector3.Distance(thisPosition, semaphorePos); float distToNearSemaphore = Mathf.Infinity;

                    if (!Physics.Raycast(thisPosition + _rayOffsetH, dir, distToSemaphore, Settings.obstacleMask))
                    {
                        if (NearSemaphore == null)
                        {
                            distToNearSemaphore = distToSemaphore;
                            NearSemaphore = s;
                            continue;
                        }
                        else
                        {
                            if (distToSemaphore < distToNearSemaphore)
                            {
                                NearSemaphore = s;
                            }
                        }
                    }
                }
            }
        }
        private void NearTarget_Car(Vector3 thisPosition, Vector3 transformForward, float halfViewAngle)
        {
            foreach (var c in _cars)
            {
                Transform carTr = c.transform;
                Vector3 carPos = carTr.position;

                Vector3 dir = (carPos - thisPosition); dir.y = 0.0f; dir.Normalize();

                if (Vector3.Angle(transformForward, dir) < halfViewAngle)
                {
                    float distToCar = Vector3.Distance(thisPosition, carPos); float distToNearCar = Mathf.Infinity;

                    if (distToCar > Settings.distToCar) continue;

                    if (!Physics.Raycast(thisPosition, dir, distToCar, Settings.obstacleMask))
                    {
                        if (NearCar == null)
                        {
                            distToNearCar = distToCar;
                            NearCar = c;
                            continue;
                        }
                        else
                        {
                            if (distToCar < distToNearCar)
                            {
                                NearCar = c;
                            }
                        }
                    }
                }
            }
        }
        private void NearTarget_Player(Vector3 thisPosition, Vector3 transformForward, float halfViewAngle)
        {
            foreach (var p in _players)
            {
                Transform playerTr = p.transform;
                Vector3 playerPos = playerTr.position;
                Vector3 dir = (playerPos - thisPosition); dir.y = 0.0f; dir.Normalize();

                if (Vector3.Angle(transformForward, dir) < halfViewAngle)
                {
                    float distToPlayer = Vector3.Distance(thisPosition, playerPos); float distToNearPlayer = Mathf.Infinity;

                    if (distToPlayer > Settings.distToPlayer) continue;

                    if (!Physics.Raycast(thisPosition + _rayOffsetH, dir, distToPlayer, Settings.obstacleMask))
                    {
                        if (NearPlayer == null)
                        {
                            distToNearPlayer = distToPlayer;
                            NearPlayer = p;
                            continue;
                        }
                        else
                        {
                            if (distToNearPlayer < distToPlayer)
                            {
                                NearPlayer = p;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #endregion


        public void CurrentMoveSpeedUpdate(float deltaTime)
        {
            CurrentMoveSpeed = Mathf.MoveTowards(CurrentMoveSpeed, TargetMoveSpeed, deltaTime * 4.5f);
        }


        public void ChangeStateWithDelay(PasserbyStates s)
        {
            StartCoroutine(DelayedChangeState(s));
        }
        IEnumerator DelayedChangeState(PasserbyStates s)
        {
            yield return new WaitForSeconds(1.0f);
            State = PreSet_State;
            ChangeState(s);
        }


        public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
        {
            if (!angleIsGlobal)
            {
                angleInDegrees += transform.eulerAngles.y;
            }
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        public void BeTaxiCustomer()
        {
            //IsCurrentlyChoosen = true;
            //WasChoosen = true;

            //LeanTaxiTransform = DriverSingleton.Instance.CharacterData.CarData.leanTaxiTransform;
            //State = PasserbyStates.WaitTaxi;
            //ChangeState(State);
        }


        public Vector3 HitVelocity { get; set; }
        PedestrianStates IStateMachine.State { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        PedestrianStates IStateMachine.PreSet_State { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void TransitionToDeathState(Vector3 hitVelocity)
        {
            HitVelocity = hitVelocity;
            RagdollController.GetHitVelocity(hitVelocity);

            State = PasserbyStates.Death;
            ChangeState(State);
        }

        public void MakeKinematic(bool b) => Rigidbody.isKinematic = b;

        public void ArrivedToTaxiDestination()
        {
            State = PasserbyStates.ExitCar;
            ChangeState(State);
        }

        public void ChangeState(PedestrianStates state)
        {
            throw new System.NotImplementedException();
        }
    }
}