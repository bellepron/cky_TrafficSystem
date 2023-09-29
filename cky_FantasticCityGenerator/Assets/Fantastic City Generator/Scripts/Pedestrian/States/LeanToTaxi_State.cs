//using cky.Car;
using cky.FCG.Pedestrian.StateMachine;
//using cky.Managers;
using cky.Reuseables.Extension;
//using DG.Tweening;
using UnityEngine;

namespace cky.FCG.Pedestrian.States
{
    public class LeanToTaxi_State : PedestrianBaseState
    {
        //DriverSingleton _driverSingleton;
        PedestrianAnimatorController _animatorController;

        Transform _targetTransform;

        public LeanToTaxi_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //_driverSingleton = DriverSingleton.Instance;
            //_animatorController = stateMachine.AnimatorController;
            //_targetTransform = stateMachine.LeanTaxiTransform;

            //UIManager.Instance.QuestProvider.CloseCurrentTaxiCustomerCanvas();

            //// Lean Animation
            //_animatorController.LeanCarDoor_Trigger();

            //DOVirtual.DelayedCall(1.1f, () => StartDialogue());
        }

        public override void Exit()
        {

        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {
            // SetRotation& position clearly.
            //stateMachine.transform.SetPositionAndRotation(_targetTransform.position,
            //                                              _targetTransform.rotation);

            stateMachine.transform.position = Vector3.Lerp(stateMachine.transform.position, _targetTransform.position, 5 * deltaTime);
            stateMachine.transform.TurnToDirection(_targetTransform.forward, 5);
        }

        private void StartDialogue()
        {
            stateMachine.State = PedestrianStates.TaxiDialogue;
            stateMachine.ChangeState(stateMachine.State);
        }
    }
}