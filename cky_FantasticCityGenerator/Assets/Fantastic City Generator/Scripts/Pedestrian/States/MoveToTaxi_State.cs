//using cky.Car;
using cky.FCG.Pedestrian.StateMachine;
using UnityEngine;

namespace cky.FCG.Pedestrian.States
{
    public class MoveToTaxi_State : PedestrianBaseState
    {
        //DriverSingleton _driverSingleton;
        Transform _targetTransform;

        public MoveToTaxi_State(PedestrianStateMachine stateMachine) : base(stateMachine)
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

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            DistanceCheck();
        }

        private void Move(float deltaTime)
        {
            stateMachine.AgentSetDestination(_targetTransform.position);
        }

        private void DistanceCheck()
        {
            //var distance = Vector3.Distance(stateMachine.transform.position, _targetTransform.position);
            //if (distance < 0.5f)
            //{
            //    LeanToTaxi_Command();
            //}

            if (stateMachine.Agent.remainingDistance < 0.5f)
            {
                LeanToTaxi_Command();
            }
        }

        private void LeanToTaxi_Command()
        {
            stateMachine.State = PedestrianStates.LeanToTaxi;
            stateMachine.ChangeState(stateMachine.State);
        }
    }
}