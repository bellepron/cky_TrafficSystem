using cky.FCG.Pedestrian.StateMachine;
using UnityEngine;

namespace cky.FCG.Pedestrian
{
    [CreateAssetMenu(menuName = "cky/FCG/New Pedestrian Settings", fileName = "New Pedestrian Settings")]
    public class PedestrianSettings : ScriptableObject
    {
        public PedestrianStates state = PedestrianStates.Walk;

        public float distanceToSelfDestroyDefault = 200;
        public float checkingAwayFromPlayerRepeatRate = 5.0f;
        public float timeToStayStill = 20.0f;
        public float timeToStayStill2 = 40.0f;

        public float reachDistanceToNode = 1.0f;

        public float delayToWalkAgain = 0.5f;


        public float walkSpeed = 2.0f;
        public float maxRandomWalkSpeed = 7.0f;
        public float runSpeed = 7.0f;
        public float idleRotationSpeed = 5.0f;
        public float viewAngle = 55.0f;
        public float viewRadius = 3.0f;
        public float distToCar = 10.0f;
        public float distToPlayer = 10.0f;
        public float distToPedestrian = 5.0f;
        public float distToStopLight = 3.0f;

        public LayerMask targetMask = 3840;
        public LayerMask obstacleMask;

        public float ikLookDistance = 8.0f;

        public LayerMask crosswalkLayerMask;

        public LayerMask chaosEffectLayer;
        public float chaosRadius = 10.0f;

        [Space(15)]
        [Header("Gizmos")]
        public Color idleColor = Color.black;
        public Color walkColor = Color.blue;
        public Color runColor = Color.magenta;
        public Color chaosColor = Color.red;
    }
}