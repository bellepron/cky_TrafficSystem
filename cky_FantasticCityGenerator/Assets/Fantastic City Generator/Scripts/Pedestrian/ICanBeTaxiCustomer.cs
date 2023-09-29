using cky.FCG.Pedestrian.StateMachine;
using UnityEngine;

namespace cky.FCG.Pedestrian
{
    public class ICanBeTaxiCustomer : MonoBehaviour
    {
        PedestrianStateMachine sm;

        private void Awake()
        {
            sm = GetComponent<PedestrianStateMachine>();
        }

        public bool IsAlive() => sm.IsAlive;
        public bool IsInsideSemaphore() => sm.IsInsideSemaphore;
        public void BeTaxiCustomer() => sm.BeTaxiCustomer();
    }
}