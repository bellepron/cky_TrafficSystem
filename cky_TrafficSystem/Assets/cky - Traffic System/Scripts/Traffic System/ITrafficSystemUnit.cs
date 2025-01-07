using UnityEngine;

namespace cky.TrafficSystem
{
    public interface ITrafficSystemUnit
    {
        public Vector3 Position { get; }
        public int SideAtual { get; }
        public Transform AtualWay { get; }
        public WaypointsContainer_Abstract AtualWayScript { get; }
        public bool NodeSteerCarefully { get; }
        public bool NodeSteerCarefully2 { get; }
        public Transform MyOldWay { get; }
        public int MyOldSideAtual { get; }
        public WaypointsContainer_Abstract MyOldWayScript { get; }
        public Vector3 AvanceNode { get; }

        public void TrafficSystemInit(int sideAtual, Transform atualWay, WaypointsContainer_Abstract atualWayScript, int currentNode, float distanceToSelfDestroy, Transform player, TrafficSystem_Abstract trafficSystem);
    }
}