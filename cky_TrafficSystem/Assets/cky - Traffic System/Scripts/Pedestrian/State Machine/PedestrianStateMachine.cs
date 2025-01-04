using System.Collections.Generic;
using cky.StateMachine.Base;
using System.Collections;
using UnityEngine.AI;
using CKY_Pooling;
using UnityEngine;

namespace cky.TrafficSystem
{
    public enum PedestrianStates
    {
        Empty, Idle, Walk, Run, Death, Chaos
    }

    public enum HumanTypes { None, Normal }

    public struct Tags
    {
        public const string People = "People";
        public const string Car = "Car";
    }

    public class PedestrianStateMachine : BaseStateMachine, ITrafficSystemUnit/*, IHitable, IChaosable*/
    {
        [field: SerializeField] public bool IsAlive { get; set; }

        [field: SerializeField] public HumanTypes HumanType { get; private set; }

        [field: SerializeField] public PedestrianSettings Settings { get; set; }
        [field: SerializeField] public PedestrianStates State { get; set; }
        [field: SerializeField] public PedestrianStates PreSet_State { get; set; }

        [field: SerializeField] public PedestrianAnimatorController AnimatorController { get; set; }
        [field: SerializeField] public RagdollToggle RagdollToggle { get; set; }
        [field: SerializeField] public NavMeshAgent Agent { get; set; }
        [field: SerializeField] public PedestrianIKLook PedestrianIKLook { get; set; }

        [field: SerializeField] public PedestrianStateMachine NearPedestrian { get; set; }
        [field: SerializeField] public Transform NearCar { get; set; }
        //[field: SerializeField] public Transform NearPlayer { get; set; }

        public float TargetMoveSpeed { get; private set; }

        Collider[] _crosswalkColliders;
        public Crosswalk PreviousCrosswalk;
        public Crosswalk OnCrosswalk;

        public TrafficSystem_Abstract TrafficSystem { get; set; }

        [SerializeField] Collider[] targetsInViewRadius;
        [SerializeField] List<PedestrianStateMachine> _pedestrians = new List<PedestrianStateMachine>();
        [SerializeField] List<Transform> _cars = new List<Transform>();
        //[SerializeField] List<Transform> _players = new List<Transform>();
        //[SerializeField] List<Transform> _stopLights = new List<Transform>();

        [HideInInspector] public bool QuestCompleted { get; set; } = false;
        [HideInInspector] public GameObject QuestDestination;

        bool _isFirstCreation = true;

        //SoundManager SoundManager;

        public Vector3 Position => transform.position;
        public int SideAtual => sideAtual;
        public Transform AtualWay => atualWay;



        public void AgentStop(bool b) { if (Agent.isActiveAndEnabled) Agent.isStopped = b; }
        public void AgentMoveSpeed(float speed) => Agent.speed = speed;
        public void AnimatorMoveSpeedUpdate()
        {
            Vector3 velocity = Agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            AnimatorController.SetAnimatorMoveSpeedValue(speed);
        }
        public void AgentSetDestination(Vector3 targetPoint)
        {
            if (Agent.isActiveAndEnabled)
            {
                targetPoint.y = _thisTr.position.y;
                Agent.destination = targetPoint;
            }

            AnimatorMoveSpeedUpdate();
        }

        private void OnEnable()
        {
            if (_isFirstCreation)
            {
                _isFirstCreation = false;

                GetSettings(Settings);

                _thisTr = transform;
                cameraTr = Camera.main.transform;

                _crosswalkColliders = new Collider[1];

                //SoundManager = GameObject.FindWithTag(TagHelper.SOUNDMANAGER).GetComponent<SoundManager>();
            }

            IsAlive = true;

            if (atualWay)
                Init();

            State = PreSet_State;
            SwitchState(CreateNewStateByEnum(State));
        }

        private void Start()
        {
            SelfDestructWhenAwayFromThePlayerInit();
        }

        public void GetSettings(PedestrianSettings settings)
        {
            Settings = settings;
            State = settings.state;
            PreSet_State = State;
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
            if (State == PedestrianStates.Chaos)
            {
                if (state == PedestrianStates.Death)
                {

                }
                else
                {
                    return;
                }
            }

            State = state;
            SwitchState(CreateNewStateByEnum(State));
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

                case PedestrianStates.Death: return new Death_State(this);
                case PedestrianStates.Chaos: return new Chaos_State(this);

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
            ClearLists();

            targetsInViewRadius = Physics.OverlapSphere(_thisTr.position, Settings.viewRadius, Settings.targetMask);

            _pedestrians = new List<PedestrianStateMachine>();
            _cars = new List<Transform>();
            //_players = new List<Transform>();

            SortTargetsInView_Pedestrian_Car_Player(targetsInViewRadius);

            // newCKY
            _crosswalkColliders[0] = null;
            if (State != PedestrianStates.Idle)
            {
                Crosswalker();
            }
        }
        private void Crosswalker()
        {
            var scale = transform.GetChild(0).localScale;
            var crosswalkCount = Physics.OverlapBoxNonAlloc(transform.position + new Vector3(0, scale.y * 0.5f, 0), scale / 2, _crosswalkColliders, Quaternion.identity, Settings.crosswalkLayerMask);

            if (_crosswalkColliders[0] == null)
            {
                OnCrosswalk = null;
                if (State == PedestrianStates.Run)
                {
                    ChangeState(PreSet_State);
                }
            }
            else
            {
                OnCrosswalk = _crosswalkColliders[0].GetComponent<Crosswalk>();

                if (!PreviousCrosswalk)
                {
                    if (!OnCrosswalk.PedestrianGreen)
                    {
                        ChangeState(PedestrianStates.Idle);
                        OnCrosswalk = null; // For making PreviousCrosswalk = null. To control !OnCrosswalk.PedestrianGreen
                    }
                }
                else
                {
                    OnCrosswalk.PedestrianHere(); // If this on crosswalk...

                    if (OnCrosswalk.PedestrianGreen)
                    {
                        if (State != PedestrianStates.Walk)
                        {
                            ChangeState(PedestrianStates.Walk);
                        }
                    }
                    if (!OnCrosswalk.PedestrianGreen)
                    {
                        if (State != PedestrianStates.Run)
                        {
                            ChangeState(PedestrianStates.Run);
                        }
                    }
                }
            }

            PreviousCrosswalk = OnCrosswalk;
        }

        private void ClearLists()
        {
            NearPedestrian = null; NearCar = null; /*NearPlayer = null;*/
            _pedestrians.Clear(); _cars.Clear(); /*_players.Clear();*/ /*_stopLights.Clear();*/
        }

        public void IfCanReturnToNormalMove(ref bool exitState)
        {
            if (NearPedestrian == null && NearCar == null /*&& NearPlayer == null*/)
            {
                exitState = true;

                ChangeStateWithDelay(PreSet_State);
            }
        }

        public void BackToPresetState()
        {
            ChangeState(PedestrianStates.Idle);
        }

        #endregion



        #region Sort
        public Vector3 TargetPointFromAtualWayScript() => atualWayScript.Node(sideAtual, currentNode);
        private void SortTargetsInView_Pedestrian_Car_Player(Collider[] targetsInViewRadius)
        {
            Transform thisTransform = transform;
            Vector3 thisPosition = thisTransform.position;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            var halfViewAngle = Settings.viewAngle * 0.5f;

            foreach (Collider c in targetsInViewRadius)
            {
                Transform cTransform = c.transform;
                Vector3 target = cTransform.position - thisPosition;

                if (cTransform.CompareTag(Tags.People))
                {
                    if (cTransform != thisTransform)
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
                else if (cTransform.CompareTag(Tags.Car))
                {
                    if (Vector3.Dot(forward, target) > 0)
                        _cars.Add(cTransform);

                    NearTarget_Car(thisPosition, forward, halfViewAngle);
                }
                //else if (cTransform.CompareTag(TagHelper.TAG_PLAYER))
                //{
                //    if (Vector3.Dot(forward, target) > 0)
                //        _players.Add(cTransform);

                //    NearTarget_Player(thisPosition, forward, halfViewAngle);
                //}
            }

            if (_pedestrians.Count == 0) NearPedestrian = null;
            if (_cars.Count == 0) NearCar = null;
            //if (_players.Count == 0) NearPlayer = null;
        }
        #endregion


        #region NearTarget
        Vector3 _rayOffsetH = new Vector3(0, 1, 0);

        private void NearTarget_Pedestrian(Vector3 thisPosition, Vector3 transformForward, float halfViewAngle)
        {
            foreach (var p in _pedestrians)
            {
                if (p == null) continue;

                Transform pedestrianTr = p.transform;
                Vector3 pedestrianPos = pedestrianTr.position;
                Vector3 dir = pedestrianPos - thisPosition; dir.y = 0.0f; dir.Normalize();

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
        //private void NearTarget_Player(Vector3 thisPosition, Vector3 transformForward, float halfViewAngle)
        //{
        //    foreach (var p in _players)
        //    {
        //        Transform playerTr = p.transform;
        //        Vector3 playerPos = playerTr.position;
        //        Vector3 dir = (playerPos - thisPosition); dir.y = 0.0f; dir.Normalize();

        //        if (Vector3.Angle(transformForward, dir) < halfViewAngle)
        //        {
        //            float distToPlayer = Vector3.Distance(thisPosition, playerPos); float distToNearPlayer = Mathf.Infinity;

        //            if (distToPlayer > Settings.distToPlayer) continue;

        //            if (!Physics.Raycast(thisPosition + _rayOffsetH, dir, distToPlayer, Settings.obstacleMask))
        //            {
        //                if (NearPlayer == null)
        //                {
        //                    distToNearPlayer = distToPlayer;
        //                    NearPlayer = p;
        //                    continue;
        //                }
        //                else
        //                {
        //                    if (distToNearPlayer < distToPlayer)
        //                    {
        //                        NearPlayer = p;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion

        #endregion


        public void ChangeStateWithDelay(PedestrianStates s)
        {
            StartCoroutine(DelayedChangeState(s));
        }
        IEnumerator DelayedChangeState(PedestrianStates s)
        {
            float delay = Settings.delayToWalkAgain;
            float timer = 0.0f;

            while (timer < delay)
            {

                timer += Time.deltaTime;

                yield return null;
            }

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



        int countWays;
        Transform[] nodes;
        [HideInInspector] public int currentNode = 0;
        float distanceToNode;
        Transform _thisTr;

        [HideInInspector] public Transform atualWay;
        [HideInInspector] public int sideAtual = 0;
        [HideInInspector] public WaypointsContainer_Abstract atualWayScript;
        Transform myOldWay;

        [HideInInspector]
        public int myOldSideAtual = 0;

        [HideInInspector]
        public WaypointsContainer_Abstract myOldWayScript = null;

        private Vector3 _avanceNode = Vector3.zero; //private Position where an additional and momentary node can be added

        private Transform behind = null;

        [HideInInspector] public Transform player;
        Transform cameraTr;
        Vector3 agentHeight = new Vector3(0, 2.0f, 0);

        float distanceToSelfDestroy = 0; //0 = Do not autodestroy with player distance


        public void Init()
        {
            atualWayScript = atualWay.GetComponent<WaypointsContainer_Pedestrian>();

            DefineNewPath();

            if (currentNode == 0) currentNode = 1;

            distanceToNode = Vector3.Distance(atualWayScript.Node(sideAtual, currentNode), transform.position);
        }

        public void TrafficSystemInit(int sideAtual, Transform atualWay, WaypointsContainer_Abstract atualWayScript, int currentNode, float distanceToSelfDestroy, Transform player, TrafficSystem_Abstract trafficSystem)
        {
            this.sideAtual = sideAtual;
            this.atualWay = atualWay;
            this.atualWayScript = atualWayScript;
            this.currentNode = currentNode;
            this.distanceToSelfDestroy = distanceToSelfDestroy;
            this.player = player;
            this.TrafficSystem = trafficSystem;

            Init();
        }

        private Vector3 GetNodePosition() => atualWayScript.Node(sideAtual, currentNode);

        public void ckyMove()
        {
            VerificaPoints();

            var targetPoint = GetNodePosition();
            targetPoint.y = _thisTr.position.y;

            distanceToNode = Vector3.Distance(targetPoint, _thisTr.position);

            AgentSetDestination(targetPoint);
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

                        myOldWayScript.UnSetNodeZero((myOldSideAtual == 1) ? 0 : 1, transform);
                    }
                }
                else
                {
                    //int t = TestWay();
                    int t = 0;

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

                    DefineNewPath();

                    currentNode = 0;

                    _avanceNode = myOldWayScript.AvanceNode(myOldSideAtual, myOldWayScript.waypoints.Count - 1, 7);
                }
            }
        }

        void DefineNewPath()
        {
            nodes = new Transform[atualWay.childCount];
            int n = 0;
            foreach (Transform child in atualWay)
                nodes[n++] = child;

            countWays = nodes.Length;
        }



        #region Self Destruct

        void SelfDestructWhenAwayFromThePlayerInit()
        {
            if (distanceToSelfDestroy == 0)
                distanceToSelfDestroy = Settings.distanceToSelfDestroyDefault;

            SelfDestructWhenAwayFromThePlayer();
            InvokeRepeating(nameof(SelfDestructWhenAwayFromThePlayer), 0.1f, Settings.checkingAwayFromPlayerRepeatRate);
        }

        void SelfDestructWhenAwayFromThePlayer()
        {
            var a = _thisTr.position; a.y = 0;
            var b = player.position; b.y = 0;
            var dist = Vector3.Distance(a, b);

            if (dist > distanceToSelfDestroy)
            {
                DestroyObject();
            }
        }

        void DestroyObject()
        {
            CKY_PoolManager.Despawn(transform);
        }

        void OnDisable()
        {
            TrafficSystem?.RemoveFromCurrentUnits(this);
        }

        #endregion


        //Collider[] _targetsInChaosRadius;

        //public void TriggerChaos()
        //{
        //    _targetsInChaosRadius = Physics.OverlapSphere(transform.position, Settings.chaosRadius, Settings.chaosEffectLayer);

        //    foreach (Collider c in _targetsInChaosRadius)
        //    {
        //        if (c.TryGetComponent<IChaosable>(out var iChaosable))
        //        {
        //            iChaosable.Perform_Chaos();
        //        }
        //    }
        //}

        //public void Perform_Chaos()
        //{
        //    TransitionToChaosState();
        //}

        //public void TransitionToChaosState()
        //{
        //    ChangeState(PedestrianStates.Chaos);
        //}



        //public void Hited(HitType hitType, Vector3 hitRbCenterOfMass, Vector3 hitPoint, Vector3 hitPower, ForceMode forceMode, bool isCar = false)
        //{
        //    if (isCar && hitPower.magnitude < 1) return;

        //    RagdollToggle.Hited(hitPower, forceMode);
        //    TransitionToDeathState();

        //    EventBus.OnSomeoneKilled_EventTrigger(hitType, HumanType);
        //}

        public void TransitionToDeathState()
        {
            if (!IsAlive) return;
            ChangeState(PedestrianStates.Death);
        }

        public void Death()
        {
            IsAlive = false;

            AgentStop(true);

            //TriggerChaos();
            //SoundManager.FuckYou(transform.position, HumanType);
        }



        #region Gizmos

        [Space(15)]
        [Header("Gizmo Cube")]
        [SerializeField] Vector3 gizmoCubeScale = new Vector3(0.7f, 3, 1);
        void OnDrawGizmos()
        {
            if (!IsAlive) return;

            if (State == PedestrianStates.Idle) Gizmos.color = Settings.idleColor;
            else if (State == PedestrianStates.Walk) Gizmos.color = Settings.walkColor;
            else if (State == PedestrianStates.Run) Gizmos.color = Settings.runColor;
            else if (State == PedestrianStates.Chaos) Gizmos.color = Settings.chaosColor;

            Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(new Vector3(0, gizmoCubeScale.y * 0.5f, 0), gizmoCubeScale);
            //Gizmos.matrix = Matrix4x4.identity;
        }

        #endregion
    }
}