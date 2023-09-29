//using cky.Car;
using cky.UTS.Helpers;
using cky.UTS.People.Passersby.StateMachine;
using UnityEngine;

namespace cky.UTS.People.Passersby
{
    [RequireComponent(typeof(PasserbyStateMachine))]
    public class PasserbyCollisionController : MonoBehaviour
    {
        private PasserbyStateMachine PasserbyStateMachine;

        bool _hit;

        private void Awake()
        {
            PasserbyStateMachine = GetComponent<PasserbyStateMachine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            //if (_hit == false && other.CompareTag(TagHelper.TAG_CAR) && DriverSingleton.Instance.CharacterData.VehicleController.isActiveAndEnabled /*&& DriverSingleton.Instance.CharacterData.VehicleController.Speed > 1*/)
            //{
            //    _hit = true;

            //    var hitVelocity = other.GetComponentInParent<Rigidbody>().velocity;

            //    PasserbyStateMachine.TransitionToDeathState(hitVelocity);
            //}
        }
    }
}