using UnityEngine;

namespace cky.UTS.Car
{
    [CreateAssetMenu(menuName = "UTS/New Car Settings", fileName = "New Car Settings")]
    public class CarSettings : ScriptableObject
    {
        public float moveSpeedBase = 12.0f;
        public float maxSpeed = 15.0f;
        public float speedRandomness = 0.15f;
        public float speedIncrease = 2.0f;
        public float speedDecrease = 2.0f;

        public float distanceToCar = 10.0f;
        public float distanceToSemaphore = 10.0f;
        public float maxAngleToMoveBreak = 8.0f;
        public float nextPointThreshold = 3;

        public float getPathReachDistance_CKY = 5.0f;
        public float getPathReachDistance_Way = 5.0f;
        public float getPathReachDistance_WayForBrake = 10.0f;
    }
}