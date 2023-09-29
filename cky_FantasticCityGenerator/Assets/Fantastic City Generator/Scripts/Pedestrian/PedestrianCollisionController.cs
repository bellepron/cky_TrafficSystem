//using cky.Car;
using cky.UTS.Helpers;
using UnityEngine;

namespace cky.FCG.Pedestrian.StateMachine
{
    [RequireComponent(typeof(PedestrianStateMachine))]
    public class PedestrianCollisionController : MonoBehaviour
    {
        private PedestrianStateMachine PedestrianStateMachine;

        bool _hit;

        private void Awake()
        {
            PedestrianStateMachine = GetComponent<PedestrianStateMachine>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_hit == false && other.CompareTag(TagHelper.TAG_CAR)) /*&& DriverSingleton.Instance.CharacterData.VehicleController.isActiveAndEnabled*/ /*&& DriverSingleton.Instance.CharacterData.VehicleController.Speed > 1*/
            {
                var otherRbVelocity = other.GetComponentInParent<Rigidbody>().velocity;
                //Debug.Log($"{otherRbVelocity.magnitude}");

                if (otherRbVelocity.magnitude > 3)
                {
                    _hit = true;

                    PedestrianStateMachine.TransitionToDeathState(otherRbVelocity);
                }
            }
        }
    }
}