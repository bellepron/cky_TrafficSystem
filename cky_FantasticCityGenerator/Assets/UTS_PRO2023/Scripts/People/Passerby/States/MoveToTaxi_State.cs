//using cky.Car;
using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class MoveToTaxi_State : PasserbyBaseState
    {
        //DriverSingleton _driverSingleton;
        Transform _targetTransform;

        public MoveToTaxi_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            Debug.Log($"{stateMachine.name} Move to Taxi!");
            //_driverSingleton = DriverSingleton.Instance;
            _targetTransform = stateMachine.LeanTaxiTransform;

            stateMachine.AnimatorController.SetMoveSpeed(1.0f);

            //_driverSingleton.ActivateCarControl(false);
        }

        public override void Exit()
        {
            stateMachine.MoveSpeedBlendValue = 0.0f;
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            RotateToTaxi(deltaTime);
            DistanceCheck();
        }

        public override void FixedTick(float fixedDeltaTime)
        {


        }

        private void Move(float deltaTime)
        {
            stateMachine.transform.position += stateMachine.transform.forward * stateMachine.Settings.runSpeed * deltaTime;
        }

        private void RotateToTaxi(float deltaTime)
        {
            var dir = (_targetTransform.position - stateMachine.transform.position); dir.y = 0.0f; dir.Normalize();

            stateMachine.transform.rotation = Quaternion.Slerp(
                    stateMachine.transform.rotation,
                    Quaternion.LookRotation(dir),
                    stateMachine.Settings.moveRotatitonSpeed * deltaTime);
        }

        private void DistanceCheck()
        {
            var distance = Vector3.Distance(stateMachine.transform.position, _targetTransform.position);
            if (distance < 0.5f)
            {
                LeanToTaxi_Command();
            }
        }

        private void LeanToTaxi_Command()
        {
            stateMachine.State = PasserbyStates.LeanToTaxi;
            stateMachine.ChangeState(stateMachine.State);
        }
    }
}