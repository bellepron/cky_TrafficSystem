//using cky.Car;
//using cky.Managers;
using cky.Reuseables.Extension;
//using DG.Tweening;
using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class LeanToTaxi_State : PasserbyBaseState
    {
        //DriverSingleton _driverSingleton;
        PasserbyAnimatorController _animatorController;

        Transform _targetTransform;

        public LeanToTaxi_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            //_driverSingleton = DriverSingleton.Instance;
            _animatorController = stateMachine.AnimatorController;
            _targetTransform = stateMachine.LeanTaxiTransform;

            stateMachine.SetTargetMoveSpeed(0.0f);

            //UIManager.Instance.QuestProvider.CloseCurrentTaxiCustomerCanvas();

            // Lean Animation
            _animatorController.LeanCarDoor_Trigger();

            //DOVirtual.DelayedCall(1.1f, () => StartDialogue());
        }

        public override void Exit()
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

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        private void StartDialogue()
        {
            stateMachine.State = PasserbyStates.TaxiDialogue;
            stateMachine.ChangeState(stateMachine.State);
        }
    }
}