//using cky.Car;
using cky.FCG.Pedestrian.StateMachine;
//using cky.Managers;
using cky.Reuseables.Extension;
using UnityEngine;

namespace cky.FCG.Pedestrian.States
{
    public class WaitTaxi_State : PedestrianBaseState
    {
        //DriverSingleton _driver;
        Transform _playerCarTr;
        bool _isInRange; // Cause of animation transitions.
        bool _isStartedHandWaving; // Cause of animation transitions.

        public WaitTaxi_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //_driver = DriverSingleton.Instance;
            //_playerCarTr = _driver.CharacterData.Car.transform;

            stateMachine.AnimatorController.SetMoveSpeed(0.0f);

            stateMachine.AgentIsStopped(true);

            //UIManager.Instance.MapCanvasController.SetNewDestination(stateMachine.transform);
        }

        public override void Exit()
        {
            stateMachine.AgentIsStopped(false);
            //UIManager.Instance.MapCanvasController.DisableNavigation();
        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {
            RotateToPlayer(deltaTime);
            if (!_isInRange) CheckDistance();
            else MoveToTaxiTrigger(deltaTime);
        }

        private void RotateToPlayer(float deltaTime)
        {
            var dir = (_playerCarTr.position - stateMachine.transform.position); dir.y = 0.0f; dir.Normalize();
            var angle = Vector3.Angle(stateMachine.transform.forward, dir);

            if (angle > 30)
            {
                stateMachine.transform.rotation = Quaternion.Slerp(
                    stateMachine.transform.rotation,
                    Quaternion.LookRotation(dir),
                    stateMachine.Settings.idleRotationSpeed * deltaTime);
            }
        }

        private void CheckDistance()
        {
            var distance = Vector3.Distance(stateMachine.transform.position, _playerCarTr.position);

            if (distance < 15)
            {
                if (_playerCarTr.InSight_Direction(_playerCarTr.right, stateMachine.transform, 160))
                {
                    PlayerCarInRange();
                }
            }
        }

        private void PlayerCarInRange()
        {
            _isInRange = true;

            stateMachine.AnimatorController.WaveHands_Trigger();
        }

        float timeCounter;
        private void MoveToTaxiTrigger(float deltaTime)
        {
            timeCounter += deltaTime;
            if (timeCounter < 0.5f) return;

            if (stateMachine.AnimatorController.IsWavingHands())
            {
                _isStartedHandWaving = true;
            }
            else
            {
                if (_isStartedHandWaving)
                {
                    stateMachine.State = PedestrianStates.MovetoTaxi;
                    stateMachine.ChangeState(stateMachine.State);
                }
            }
        }
    }
}