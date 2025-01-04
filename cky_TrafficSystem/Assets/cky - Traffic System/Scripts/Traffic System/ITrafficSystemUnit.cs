using UnityEngine;

namespace cky.TrafficSystem
{
    public interface ITrafficSystemUnit
    {
        public Vector3 Position { get; }
        public int SideAtual { get; }
        public Transform AtualWay { get; }
        public void TrafficSystemInit(int sideAtual, Transform atualWay, WaypointsContainer_Abstract atualWayScript, int currentNode, float distanceToSelfDestroy, Transform player, TrafficSystem_Abstract trafficSystem);
    }
}