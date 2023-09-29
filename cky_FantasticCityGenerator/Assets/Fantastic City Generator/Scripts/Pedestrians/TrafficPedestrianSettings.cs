using UnityEngine;

namespace FCG.Pedestrians
{
    [CreateAssetMenu(menuName = "FGC/Traffic Pedestrian Settings", fileName = "TrafficPedestrian Settings")]
    public class TrafficPedestrianSettings : ScriptableObject
    {
        public float rayLength = 10.0f;
        public float distanceToSelfDestroyDefault = 200;
        public float checkingAwayFromPlayerRepeatRate = 5.0f;
        public float timeToStayStill = 20.0f;
        public float timeToStayStill2 = 40.0f;

        public float reachDistanceToNode = 5.0f;
    }
}