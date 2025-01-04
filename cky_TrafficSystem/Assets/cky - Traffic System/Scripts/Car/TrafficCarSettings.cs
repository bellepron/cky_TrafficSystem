using UnityEngine;

namespace cky.TrafficSystem
{
    [CreateAssetMenu(menuName = "cky/Data/Traffic Car Settings", fileName = "TrafficCar Settings")]
    public class TrafficCarSettings : ScriptableObject
    {
        public float rayLength = 10.0f;
        public float distanceToSelfDestroyDefault = 200;
        public float checkingAwayFromPlayerRepeatRate = 5.0f;
        public float timeToStayStill = 20.0f;
        public float timeToStayStill2 = 40.0f;


        [Space(25)]
        [Range(10000, 60000)]
        public float springs = 25000.0f;

        [Range(1000, 6000)]
        public float dampers = 1500.0f;

        [Range(60, 200)]
        public float carPower = 120f;

        [Range(5, 40)]
        public float brakePower = 8f;

        [Range(20, 210)]
        public float limitSpeed = 120.0f;

        [Range(30, 72)]
        public float maxSteerAngle = 40.0f; //Maximum wheel curvature angle

        [Range(-1, 1)]
        public float curveAdjustment = 0.0f; // make tighter or more open turns
    }
}