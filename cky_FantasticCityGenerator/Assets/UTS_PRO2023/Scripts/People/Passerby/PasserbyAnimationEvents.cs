using cky.UTS.People.Passersby.StateMachine;
using UnityEngine;

namespace cky.UTS.People.Passersby
{
    public class PasserbyAnimationEvents : MonoBehaviour
    {
        PasserbyStateMachine _sm;

        private void Awake() => _sm = GetComponent<PasserbyStateMachine>();

        public void OpenCarDoor() => _sm.OpenCarDoor_AnimEvent();
        public void CloseCarDoor() => _sm.CloseCarDoor_AnimEvent();
        public void EnableCollisions() => _sm.EnableCollisions(true);
        public void DisableCollisions() => _sm.EnableCollisions(false);
    }
}