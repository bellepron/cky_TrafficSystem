using UnityEngine;

namespace cky.TrafficSystem
{
    [CreateAssetMenu(fileName = "Traffic Light Settings", menuName = "cky/Traffic Light Settings")]
    public class TrafficLightSettings : ScriptableObject
    {
        public float redGreenTime = 8;
        public float yellowTime = 2;
    }
}