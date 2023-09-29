using UnityEngine;

namespace cky.FCG.Pedestrian.StateMachine
{
    public class PedestrianAnimationEvents : MonoBehaviour
    {
        PedestrianStateMachine _sm;

        private void Awake() => _sm = GetComponent<PedestrianStateMachine>();

        public void OpenCarDoor() => _sm.OpenCarDoor_AnimEvent();
        public void CloseCarDoor() => _sm.CloseCarDoor_AnimEvent();
        public void EnableCollisions() => _sm.EnableCollisions(true);
        public void DisableCollisions() => _sm.EnableCollisions(false);
    }
}