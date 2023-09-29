//using cky.Car;
using cky.FCG.Pedestrian.States;
using cky.Ragdoll;
using cky.StateMachine.Base;
using cky.UTS.Helpers;
using FCG.Pedestrians;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace cky.FCG.Pedestrian.StateMachine
{
    public enum PedestrianStates
    {
        Empty, Idle, Walk, Run, WaitTaxi, MovetoTaxi, LeanToTaxi, EnterCar,
        InCar, ExitCar, GetInTaxi, GetOutTaxi, Death, TaxiDialogue, Dialogue, WalkWithoutCheckingCar
    }

    public class PedestrianStateMachine : BaseStateMachine, IStateMachine
    {
        [field: SerializeField] public PedestrianSettings Settings { get; set; }
        [field: SerializeField] public PedestrianStates State { get; set; }
        [field: SerializeField] public PedestrianStates PreSet_State { get; set; }
        public PedestrianBaseState CurrentPedestrian_State { get; set; }

        [field: SerializeField] public PedestrianAnimatorController AnimatorController { get; set; }
        [field: SerializeField] public RagdollController RagdollController { get; set; }
        [field: SerializeField] public Rigidbody Rigidbody { get; set; }
        [field: SerializeField] public NavMeshAgent Agent { get; set; }

        [field: SerializeField] public PedestrianStateMachine NearPedestrian { get; set; }
        [field: SerializeField] public SemaphoreMovementSide NearSemaphore { get; set; }
        [field: SerializeField] public Transform NearCar { get; set; }
        [field: SerializeField] public Transform NearPlayer { get; set; }

        public float TargetMoveSpeed { get; private set; }

        [field: SerializeField] public bool IsInsideSemaphore { get; set; }
        [field: SerializeField] public bool IsRedSemaphore { get; set; }

        [SerializeField] Collider[] targetsInViewRadius;
        [SerializeField] List<PedestrianStateMachine> _pedestrians = new List<PedestrianStateMachine>();
        [SerializeField] List<SemaphoreMovementSide> _semaphores = new List<SemaphoreMovementSide>();
        [SerializeField] List<Transform> _cars = new List<Transform>();
        [SerializeField] List<Transform> _players = new List<Transform>();

        [HideInInspector] public Transform LeanTaxiTransform;

        [HideInInspector] public float MoveSpeedBlendValue;
        [HideInInspector] public float MoveSpeedBlendIncDecSpeed;
        [HideInInspector] public bool IsAlive { get; set; } = true;
        [HideInInspector] public bool IsCurrentlyChoosen { get; set; } = false;
        [HideInInspector] public bool WasChoosen { get; set; } = false;

        [HideInInspector] public float IsIdle = 0.0f; // Hareket etmek için sýrada bekleyen yayalar sýrayla harekete baþlasýn diye.

        public void ChangeWithInputHandler() => SwitchState(new Idle_State(this));
        public void AgentIsStopped(bool b) => Agent.isStopped = b;
        public void AgentMoveSpeed(float speed) => Agent.speed = speed;
        public void AnimatorController_MoveSpeed() => AnimatorController.SetMoveSpeed(Agent.velocity.magnitude);
        public void AgentActivate(bool b) => Agent.enabled = b;
        public void AgentSetDestination(Vector3 targetPoint)
        {
            targetPoint.y = thisTr.position.y;
            Agent.destination = targetPoint;
        }


        public void Init(PedestrianSettings settings)
        {
            Settings = settings;
            State = settings.state;
            PreSet_State = State;

            Rigidbody = GetComponent<Rigidbody>();
            RagdollController = GetComponent<RagdollController>();
            AnimatorController = GetComponent<PedestrianAnimatorController>();

            //MoveSpeedBlendSpeed = 2.0f;
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

        private void Awake()
        {
            Init(Settings);

            Agent = GetComponent<NavMeshAgent>();
            cameraTr = Camera.main.transform;

            timeStoped = Time.time;

            thisTr = transform;
            //thisTr.SetParent(transform);
            thisTr.localRotation = Quaternion.identity;


            if (player && !FindObjectOfType<PedestrianTrafficSystem>())
                Debug.LogError("The Pedestrian Traffic System.prefab not found in the Hierarchy");


            if (atualWay)
                Init();

            MoveSpeedBlendIncDecSpeed = 3;

            SelfDestructWhenAwayFromThePlayerInit();
        }

        private void Start()
        {
            CurrentPedestrian_State = CreateNewStateByEnum(State);
            SwitchState(CurrentPedestrian_State);
        }


        protected override void Tick()
        {
            base.Tick();
        }


        protected override void FixedTick()
        {
            base.FixedTick();
        }


        public void ChangeState(PedestrianStates state)
        {
            State = state;
            CurrentPedestrian_State = CreateNewStateByEnum(State);
            SwitchState(CurrentPedestrian_State);
        }


        #region Create New State By Enum

        private PedestrianBaseState CreateNewStateByEnum(PedestrianStates state)
        {
            switch (state)
            {
                case PedestrianStates.Empty: return new Empty_State(this);
                case PedestrianStates.Idle: return new Idle_State(this);
                case PedestrianStates.Walk: return new Walk_State(this);
                case PedestrianStates.Run: return new Run_State(this);

                case PedestrianStates.WaitTaxi: return new WaitTaxi_State(this);
                case PedestrianStates.MovetoTaxi: return new MoveToTaxi_State(this);
                case PedestrianStates.LeanToTaxi: return new LeanToTaxi_State(this);
                case PedestrianStates.EnterCar: return new EnterCar_State(this);
                case PedestrianStates.InCar: return new InCar_State(this);
                case PedestrianStates.ExitCar: return new ExitCar_State(this);

                case PedestrianStates.GetInTaxi: return new GetInTaxi_State(this);
                case PedestrianStates.GetOutTaxi: return new GetOutTaxi_State(this);

                case PedestrianStates.Death: return new Death_State(this);

                case PedestrianStates.TaxiDialogue: return new TaxiDialogue_State(this);
                case PedestrianStates.Dialogue: return new Dialogue_State(this);
                case PedestrianStates.WalkWithoutCheckingCar: return new WalkWithoutCheckingCar_State(this);

                default:
                    {
                        Debug.Log("You need to check this!");
                        return new Walk_State(this);
                    }
            }
        }

        #endregion

        public Transform whoMakeMeStopTr;
        #region Vision

        #region AI Sight
        public void AISight()
        {
            //if (whoMakeMeStopTr != null)
            //{
            //    var thisPos = thisTr.position;
            //    var whoMakeMeStopPos = whoMakeMeStopTr.position; whoMakeMeStopPos.y = thisPos.y;
            //    var dist = Vector3.Distance(whoMakeMeStopPos, thisPos);
            //    var forwardDirection = transform.TransformDirection(Vector3.forward);
            //    var targetDirection = whoMakeMeStopPos - thisPos;

            //    if (Vector3.Angle(forwardDirection, targetDirection) < Settings.viewAngle * 0.5f)
            //    {
            //        if (whoMakeMeStopTr.CompareTag(TagHelper.TAG_PEOPLE))
            //        {
            //            if (dist < Settings.distToPedestrian + IsIdle)
            //            {
            //                Debug.Log("PEOPLE");
            //                return;
            //            }
            //        }
            //        else if (whoMakeMeStopTr.CompareTag(TagHelper.TAG_CAR))
            //        {
            //            if (dist < Settings.distToCar /*+ IsIdle*/)
            //            {
            //                Debug.Log("CAR");
            //                return;
            //            }
            //        }
            //        else if (whoMakeMeStopTr.CompareTag(TagHelper.TAG_PLAYER))
            //        {
            //            if (dist < Settings.distToPlayer + IsIdle)
            //            {
            //                Debug.Log("PLAYER");
            //                return;
            //            }
            //        }
            //        else if (whoMakeMeStopTr.CompareTag(TagHelper.TAG_PEOPLESEMAPHORE))
            //        {
            //            if (dist < Settings.distToSemaphore + IsIdle + 1000)
            //            {
            //                Debug.Log("PEOPLE SEMAPHORE");
            //                return;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        whoMakeMeStopTr = null;
            //    }
            //}

            ClearLists();

            targetsInViewRadius = Physics.OverlapSphere(thisTr.position, Settings.viewRadius /*+ IsIdle*/, Settings.targetMask);

            _pedestrians = new List<PedestrianStateMachine>();
            _semaphores = new List<SemaphoreMovementSide>();
            _cars = new List<Transform>();
            _players = new List<Transform>();

            SortTargetsInView_Pedestrian_Semaphore_Car_Player(targetsInViewRadius);
        }

        private void ClearLists()
        {
            NearPedestrian = null; NearSemaphore = null; NearCar = null; NearPlayer = null;
            _pedestrians.Clear(); _semaphores.Clear(); _cars.Clear(); _players.Clear();
        }

        public void ReturnWalkingFromRunning()
        {
            if (!IsInsideSemaphore)
            {
                State = PreSet_State;
                ChangeState(State);
            }
        }

        public void RunIfNeed()
        {
            if (IsInsideSemaphore)
            {
                if (IsRedSemaphore)
                {
                    State = PedestrianStates.Run;
                    ChangeState(State);
                }
            }
        }

        public void IfCanReturnToNormalMove(ref bool exitState)
        {
            if (NearPedestrian == null && NearSemaphore == null && NearCar == null && NearPlayer == null)
            {
                if (!IsInsideSemaphore && !IsRedSemaphore)
                {
                    exitState = true;

                    ChangeStateWithDelay(PreSet_State);
                }
            }
        }

        #endregion



        #region Sort
        public Vector3 TargetPointFromAtualWayScript() => atualWayScript.Node(sideAtual, currentNode);
        private void SortTargetsInView_Pedestrian_Semaphore_Car_Player(Collider[] targetsInViewRadius)
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
                        if (Vector3.Dot(forward, target) > 0)
                        {
                            //var targetPoint = atualWayScript.Node(sideAtual, currentNode);
                            //var othersTargetPoint = cTransform.GetComponent<PedestrianStateMachine>().TargetPointFromAtualWayScript();
                            //var isSameWayPoint = Vector3.Distance(targetPoint, othersTargetPoint) < 0.1f;
                            //var d = Vector3.Dot(forward, cTransform.forward) > 0;
                            //if (d) // Her gördüðü yayaya takýlmasýn diye (Dönüþlerde karþý karþýya geldiklerinde mesela).
                            //{
                            _pedestrians.Add(c.GetComponent<PedestrianStateMachine>());
                            //}
                        }

                        NearTarget_Pedestrian(thisPosition, forward, halfViewAngle);
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

            if (_pedestrians.Count == 0) NearPedestrian = null;
            if (_semaphores.Count == 0) NearSemaphore = null;
            if (_cars.Count == 0) NearCar = null;
            if (_players.Count == 0) NearPlayer = null;
        }
        #endregion


        #region NearTarget
        Vector3 _rayOffsetH = new Vector3(0, 1, 0);

        private void NearTarget_Pedestrian(Vector3 thisPosition, Vector3 transformForward, float halfViewAngle)
        {
            foreach (var p in _pedestrians)
            {
                Transform pedestrianTr = p.transform;
                Vector3 pedestrianPos = pedestrianTr.position;
                Vector3 dir = (pedestrianPos - thisPosition); dir.y = 0.0f; dir.Normalize();

                if (Vector3.Angle(transformForward, dir) < halfViewAngle)
                {
                    if (Vector3.Angle(transformForward, p.transform.forward) < 70) // TODO: cky 70*2=140
                    {
                        float distToPedestrian = Vector3.Distance(thisPosition, pedestrianPos); float distToNearPedestrian = Mathf.Infinity;

                        if (distToPedestrian > Settings.distToPedestrian) continue;

                        if (!Physics.Raycast(thisPosition + _rayOffsetH, dir, distToPedestrian, Settings.obstacleMask))
                        {
                            if (NearPedestrian == null)
                            {
                                distToNearPedestrian = distToPedestrian;
                                NearPedestrian = p;
                                continue;
                            }
                            else
                            {
                                if (distToNearPedestrian < distToPedestrian)
                                {
                                    NearPedestrian = p;
                                }
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


        public void ChangeStateWithDelay(PedestrianStates s)
        {
            StartCoroutine(DelayedChangeState(s));
        }
        IEnumerator DelayedChangeState(PedestrianStates s)
        {
            yield return new WaitForSeconds(Settings.delayToWalkAgain);
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
            //State = PedestrianStates.WaitTaxi;
            //ChangeState(State);
        }


        public Vector3 HitVelocity { get; set; }
        public void TransitionToDeathState(Vector3 hitVelocity)
        {
            HitVelocity = hitVelocity;
            RagdollController.GetHitVelocity(hitVelocity);

            State = PedestrianStates.Death;
            ChangeState(State);
        }

        public void MakeKinematic(bool b) => Rigidbody.isKinematic = b;

        public void ArrivedToTaxiDestination()
        {
            State = PedestrianStates.ExitCar;
            ChangeState(State);
        }


















        //public StatusPedestrian status;

        private Vector3 mRayCenter;

        private int countWays;
        private Transform[] nodes;

        [HideInInspector]
        public int currentNode = 0;

        private float distanceToNode;

        private Vector3 relativeVector;

        private float timeStoped;

        private Transform thisTr;

        [HideInInspector]
        public Transform atualWay;

        [HideInInspector]
        public int sideAtual = 0;

        [HideInInspector]
        public FCGPedestrianWaypointsContainer atualWayScript;

        [HideInInspector]
        public bool nodeSteerCarefully = false;     //true if I'm turning to the side that has opposite traffic (Right hand and turning left) Or (Left hand and turning right)

        [HideInInspector]
        public bool nodeSteerCarefully2 = false;    // For double line oneway

        [HideInInspector]
        public Transform myOldWay;

        [HideInInspector]
        public int myOldSideAtual = 0;

        [HideInInspector]
        public FCGPedestrianWaypointsContainer myOldWayScript = null;

        private Vector3 _avanceNode = Vector3.zero; //private Position where an additional and momentary node can be added

        private Transform behind = null;

        //[HideInInspector]
        public Transform player;
        private Transform cameraTr;
        public Vector3 agentHeight = new Vector3(0, 2.0f, 0);

        //[HideInInspector]
        public PedestrianTrafficSystem tSystem;

        //[HideInInspector]
        public float distanceToSelfDestroy = 0; //0 = Do not autodestroy with player distance

        public float curveAdjustment = 0.0f;

        public void Configure()
        {
            //    
            float p = transform.localPosition.z + 0.6f;
            float l = transform.localPosition.x;

            //    
            Transform testC = new GameObject("RayTest").transform;
            testC.SetParent(transform);

            testC.localPosition = new Vector3(0, 0f, transform.localPosition.z + 4f);
            testC.LookAt(transform);
            testC.position += new Vector3(0, 0.8f, 0);

            if (Physics.Raycast(testC.position, testC.forward, out RaycastHit hit, 4))
            {
                Debug.DrawRay(testC.position, testC.forward * 4, Color.red);
            }
            else
            {
                Debug.LogError("Adicione um collider e então tente novamente");
            }

            DestroyImmediate(testC.gameObject);
        }


        public void Init()
        {
            atualWayScript = atualWay.GetComponent<FCGPedestrianWaypointsContainer>();

            DefineNewPath();

            if (currentNode == 0) currentNode = 1;

            distanceToNode = Vector3.Distance(atualWayScript.Node(sideAtual, currentNode), thisTr.position + thisTr.forward * (curveAdjustment * 0.5f));
        }

        //public void ActivateSelfDestructWhenAwayFromThePlayer()
        //{
        //    if (tSystem && player)
        //    {
        //        if (distanceToSelfDestroy == 0) distanceToSelfDestroy = Settings.distanceToSelfDestroyDefault;
        //        InvokeRepeating(nameof(SelfDestructWhenAwayFromThePlayer), 5f, Settings.checkingAwayFromPlayerRepeatRate);
        //    }
        //}

        public bool Get_avanceNode() => (currentNode == 0 && nodeSteerCarefully && _avanceNode != Vector3.zero);
        public Vector3 GetNodePosition() => atualWayScript.Node(sideAtual, currentNode);

        bool CheckBookAllPathOptions(FCGPedestrianWaypointsContainer wayScript, int side)
        {
            int total;
            int wSide;
            FCGPedestrianWaypointsContainer wScript;

            total = (side == 0) ? wayScript.nextWay0.Length : wayScript.nextWay1.Length;

            for (int i = 0; i < total; i++)
            {

                if (side == 0)
                {
                    wScript = wayScript.nextWay0[i];
                    wSide = wayScript.nextWaySide0[i];

                }
                else
                {
                    wScript = wayScript.nextWay1[i];
                    wSide = wayScript.nextWaySide1[i];
                }

                if (wScript)
                {
                    if ((wScript.GetNodeZeroCar(wSide) != null && wScript.GetNodeZeroCar(wSide) != transform) && wScript.GetNodeZeroOldWay(wSide) != myOldWay && (!Get_avanceNode() || !wScript.GetNodeZeroCar(wSide).GetComponent<TrafficPedestrian>().Get_avanceNode()))
                        return false;
                }
                else
                {
                    Debug.LogWarning("wScript Error");
                }
            }

            return true;
        }

        bool BookAllPathOptions(FCGPedestrianWaypointsContainer wayScript, int side, bool book = true)
        {
            int total;
            int wSide;
            FCGPedestrianWaypointsContainer wScript;

            total = (side == 0) ? wayScript.nextWay0.Length : wayScript.nextWay1.Length;

            for (int i = 0; i < total; i++)
            {
                if (side == 0)
                {
                    wScript = wayScript.nextWay0[i];
                    wSide = wayScript.nextWaySide0[i];
                }
                else
                {
                    wScript = wayScript.nextWay1[i];
                    wSide = wayScript.nextWaySide1[i];
                }

                if (book)
                {
                    bool force = wScript.GetNodeZeroCar(wSide) && wScript.GetNodeZeroCar(wSide).GetComponent<TrafficPedestrian>().Get_avanceNode();
                    if (!wScript.SetNodeZero(wSide, wayScript.transform, transform, force))
                        return false;
                }
                else
                {
                    wScript.UnSetNodeZero(wSide, transform);
                }
            }

            return true;
        }

        public void ckyMove()
        {
            VerificaPoints();

            var targetPoint = atualWayScript.Node(sideAtual, currentNode);
            targetPoint.y = thisTr.position.y;

            distanceToNode = Vector3.Distance(targetPoint, thisTr.position + thisTr.forward * (curveAdjustment * 0.5f));

            Agent.destination = targetPoint;
        }


        private void VerificaPoints()
        {
            if (distanceToNode < Settings.reachDistanceToNode)
            {
                if (currentNode < countWays - 1)
                {
                    currentNode++;

                    if (currentNode == 1)
                    {
                        atualWayScript.UnSetNodeZero(sideAtual, transform);  // Release the node so that the cars that were waiting for me to pass can proceed
                        //status = StatusPedestrian.transitingNormally;

                        if (nodeSteerCarefully || nodeSteerCarefully2)
                        {
                            myOldWayScript.UnSetNodeZero((myOldSideAtual == 1) ? 0 : 1, transform);
                            BookAllPathOptions(myOldWayScript, myOldSideAtual, false); //Release others nodes so that the cars that were waiting for me to pass can proceed
                        }

                        nodeSteerCarefully = false;
                        nodeSteerCarefully2 = false;
                    }
                }
                else
                {
                    int t = TestWay();

                    //True if the chosen path was the only option
                    bool verify = (sideAtual == 0) ? atualWayScript.nextWay0.Length == 1 : atualWayScript.nextWay1.Length == 1;

                    myOldWay = atualWay;
                    myOldSideAtual = sideAtual;
                    myOldWayScript = atualWayScript;

                    if (sideAtual == 0 && (!atualWayScript.oneway || atualWayScript.doubleLine))
                    {
                        sideAtual = atualWayScript.nextWaySide0[t];
                        atualWayScript = atualWayScript.nextWay0[t];
                    }
                    else
                    {
                        sideAtual = atualWayScript.nextWaySide1[t];
                        atualWayScript = atualWayScript.nextWay1[t];
                    }

                    atualWay = atualWayScript.transform;

                    //The road I'm on has no exit, so interdict the previous road that had only this road as an option
                    if (verify && atualWayScript.bloked)
                    {
                        myOldWayScript.bloked = true;

                        //status = StatusPedestrian.bloked;
                    }

                    DefineNewPath();

                    currentNode = 0;

                    float a = GetAngulo(transform, atualWayScript.Node(sideAtual, 0));

                    //if (myOldWayScript.oneway) 
                    if (myOldWayScript.oneway && !myOldWayScript.doubleLine)
                    {
                        if (myOldWayScript.doubleLine)
                            nodeSteerCarefully2 = (myOldSideAtual == 0 && a > 20 && a < 90) || (myOldSideAtual == 1 && a < 340 && a > 270);
                        else
                            nodeSteerCarefully = false;
                    }
                    else
                        nodeSteerCarefully = (atualWayScript.rightHand == 0 && a < 340 && a > 270) || (atualWayScript.rightHand != 0 && a > 20 && a < 90);

                    if (nodeSteerCarefully)
                    {
                        _avanceNode = myOldWayScript.AvanceNode(myOldSideAtual, myOldWayScript.waypoints.Count - 1, 7);
                    }
                }
            }
        }


        public Transform GetBehind() => behind;


        void DefineNewPath()
        {
            nodes = new Transform[atualWay.childCount];
            int n = 0;
            foreach (Transform child in atualWay)
                nodes[n++] = child;

            countWays = nodes.Length;
        }


        int TestWay()
        {
            //Check if the path drawn for me is a good option (based on traffic) "VerifyTraffic"
            //Also check if the selected path has been blocked "CheckStoped"

            int total = 0;

            if (sideAtual == 0)
                total = atualWayScript.nextWay0.Length;
            else
                total = atualWayScript.nextWay1.Length;

            int t = Random.Range(0, total); // Sort one of the available paths

            if (total > 1) // If there are more path options to choose from
            {
                if (CheckStoped(t) || VerifyTraffic(t, 30) < 30 || VerifyNodeSteerCarefully2(t))
                {
                    //Test the paths to see which option is best

                    float maior = 0;
                    for (int i = 0; i < total; i++)
                    {
                        if (!CheckStoped(i)) // && !VerifyNodeSteerCarefully2(i))
                        {
                            float vf = VerifyTraffic(i, 30);

                            if (vf == 30)
                                return i;
                            else if (vf < 30)
                            {
                                if (vf > maior)
                                {
                                    maior = vf;
                                    t = i;
                                }
                            }
                        }
                    }
                }
            }

            return t;
        }

        private Vector3 GetNodeNextWay(int way, int node = 0)
        {
            /*
            Returns the position of the specified node, from a chosen path that is among the available paths (vlinked to my current path)
            */

            if (sideAtual == 0)
                return atualWayScript.nextWay0[way].Node(atualWayScript.nextWaySide0[way], node);
            else
                return atualWayScript.nextWay1[way].Node(atualWayScript.nextWaySide1[way], node);
        }

        private bool CheckStoped(int way)
        {
            //Checks if a path is blocked (due to no exit or other reason)

            if (sideAtual == 0)
                return atualWayScript.nextWay0[way].bloked;
            else
                return atualWayScript.nextWay1[way].bloked;
        }


        /*
        bool VerifyDoubleOneWayOption(int t)
        {
            //Vector3 node0 = GetNodeNextWay(t, 0);
            //nodeSteerCarefully2 = (myOldSideAtual == 0 && a > 20 && a < 90) || (myOldSideAtual == 1 && a < 340 && a > 270);
            return true;    
        }
        */

        bool VerifyNodeSteerCarefully2(int t)
        {
            if (atualWayScript.oneway && atualWayScript.doubleLine)
            {
                float a = GetAngulo(transform, GetNodeNextWay(t, 0));
                return (sideAtual == 0 && a > 20 && a < 90) || (sideAtual == 1 && a < 340 && a > 270);

            }

            return false;
        }


        float VerifyTraffic(int t, float mts = 12)
        {
            //Checks if the specified path is a good choice, it may be congested

            RaycastHit hit2;

            Vector3 node0 = GetNodeNextWay(t, 0) + new Vector3(0, 0.5f, 0);
            Vector3 node1 = GetNodeNextWay(t, 1) + new Vector3(0, 0.5f, 0);


            if (Physics.Raycast(node0, node1 - node0, out hit2, mts))
                if (hit2.transform.GetComponent<TrafficPedestrian>())
                {
                    if (hit2.transform.GetComponent<TrafficPedestrian>().GetSpeed() < 8)
                        return hit2.distance;
                    else
                        return mts - 1;
                }

            return mts;
        }



        #region Self Destruct

        public void SelfDestructWhenAwayFromThePlayerInit()
        {
            if (tSystem && player)
            {
                var a = thisTr.position; a.y = 0;
                var b = player.position; b.y = 0;
                var dist = Vector3.Distance(a, b);

                if (distanceToSelfDestroy == 0)
                {
                    distanceToSelfDestroy = Settings.distanceToSelfDestroyDefault;
                }

                if (dist > distanceToSelfDestroy && !InTheFieldOfVision(transform.position, cameraTr))
                {
                    DestroyObject();
                }

                InvokeRepeating(nameof(SelfDestructWhenAwayFromThePlayer), 5f, Settings.checkingAwayFromPlayerRepeatRate);
            }
        }

        void SelfDestructWhenAwayFromThePlayer()
        {
            var a = thisTr.position; a.y = 0;
            var b = player.position; b.y = 0;
            var dist = Vector3.Distance(a, b);
            if (dist > distanceToSelfDestroy /*&& (Time.time > timeStoped + _settings.timeToStayStill)*/ /*& !InTheFieldOfVision(thisTr.position, cameraTr*/)
            {
                DestroyObject();
            }
        }

        public void DestroyObject()
        {
            tSystem.nPedestrians--;
            Destroy(this.gameObject);
        }

        #endregion



        bool InTheFieldOfVision(Vector3 source, Transform target)
        {
            // the IACar wants to disappear without it being seen by the camera/player

            RaycastHit obsRay2;
            Vector3 rayStartPos = source + agentHeight;
            Vector3 targetPos = target.position;

            if (Physics.Linecast(rayStartPos, targetPos, out obsRay2)) //, ~(1 << LayerMask.NameToLayer("Lattice"))))
            {
                var oRay2Tr = obsRay2.transform;
                if (oRay2Tr == target || oRay2Tr.root == target)
                {
                    //Debug.DrawLine(rayStartPos, targetPos, Color.red);
                    return true;
                }
                else
                {
                    //Debug.DrawLine(rayStartPos, targetPos, Color.green);
                    return false;
                }
            }
            else
            {
                //Debug.DrawLine(rayStartPos, targetPos, Color.blue);
                return true;
            }
        }


        private float GetAngulo(Transform origem, Vector3 target)
        {
            float r;

            GameObject compass = new GameObject("Compass");
            compass.transform.parent = origem;
            compass.transform.localPosition = new Vector3(0, 0, 0);

            compass.transform.LookAt(target);
            r = compass.transform.localEulerAngles.y;

            DestroyImmediate(compass);
            return r;
        }
    }
}