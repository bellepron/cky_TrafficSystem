using cky.UTS.People.Passersby.StateMachine;
using UnityEngine;

namespace cky.UTS.People.Passersby
{
    [CreateAssetMenu(menuName = "UTS/New Passersby Settings", fileName = "New Passersby Settings")]
    public class PasserbySettings : ScriptableObject
    {
        public PasserbyStates state = PasserbyStates.Walk;

        public float walkSpeed = 1.2f;
        public float runSpeed = 3.0f;
        public float walkSpeedBlend = 0.5f;
        public float runSpeedBlend = 1.0f;
        public float moveRotatitonSpeed = 15.0f;
        public float idleRotationSpeed = 5.0f;
        public float viewAngle = 55.0f;
        public float viewRadius = 3.0f;
        public float distToPasserby = 4.0f;
        public float distToSemaphore = 25.0f;
        public float distToCar = 10.0f;
        public float distToPlayer = 10.0f;

        public LayerMask targetMask = 3840;
        public LayerMask obstacleMask;

        public float destroyTimeWhenDead = 5.0f;
    }
}