using UnityEngine;

namespace FCG
{
    [CreateAssetMenu(menuName = "FGC/Traffic Car Settings", fileName = "TrafficCar Settings")]
    public class TrafficCarSettings : ScriptableObject
    {
        public float rayLength = 10.0f;
        public float distanceToSelfDestroyDefault = 200;
        public float checkingAwayFromPlayerRepeatRate = 5.0f;
        public float timeToStayStill = 20.0f;
        public float timeToStayStill2 = 40.0f;
    }
}