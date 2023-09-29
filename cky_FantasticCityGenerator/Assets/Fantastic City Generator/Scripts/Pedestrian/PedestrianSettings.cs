using cky.FCG.Pedestrian.StateMachine;
using UnityEngine;

namespace cky.FCG.Pedestrian
{
    [CreateAssetMenu(menuName = "cky/FCG/New Pedestrian Settings", fileName = "New Pedestrian Settings")]
    public class PedestrianSettings : ScriptableObject
    {
        public PedestrianStates state = PedestrianStates.Walk;


        public float rayLength = 10.0f;
        public float distanceToSelfDestroyDefault = 200;
        public float checkingAwayFromPlayerRepeatRate = 5.0f;
        public float timeToStayStill = 20.0f;
        public float timeToStayStill2 = 40.0f;

        public float reachDistanceToNode = 1.0f;

        public float delayToWalkAgain = 0.5f;


        public float walkSpeed = 1.2f;
        public float runSpeed = 3.0f;
        public float walkSpeedBlend = 0.5f;
        public float runSpeedBlend = 1.0f;
        public float moveRotatitonSpeed = 15.0f;
        public float idleRotationSpeed = 5.0f;
        public float viewAngle = 55.0f;
        public float viewRadius = 3.0f;
        public float distToPedestrian = 4.0f;
        public float distToSemaphore = 25.0f;
        public float distToCar = 10.0f;
        public float distToPlayer = 10.0f;

        public LayerMask targetMask = 3840;
        public LayerMask obstacleMask;

        public float destroyTimeWhenDead = 5.0f;
    }
}