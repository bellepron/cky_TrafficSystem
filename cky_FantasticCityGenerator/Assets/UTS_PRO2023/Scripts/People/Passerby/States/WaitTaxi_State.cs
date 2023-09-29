//using cky.Car;
//using cky.Managers;
using cky.Reuseables.Extension;
using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class WaitTaxi_State : PasserbyBaseState
    {
        //DriverSingleton _driver;
        Transform _playerCarTr;
        bool _isInRange; // Cause of animation transitions.
        bool _isStartedHandWaving; // Cause of animation transitions.

        public WaitTaxi_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            //_driver = DriverSingleton.Instance;
            //_playerCarTr = _driver.CharacterData.Car.transform;

            stateMachine.AnimatorController.SetMoveSpeed(0.0f);

            stateMachine.SetTargetMoveSpeed(0.0f);

            //UIManager.Instance.MapCanvasController.SetNewDestination(stateMachine.transform);
        }

        public override void Exit()
        {
            //UIManager.Instance.MapCanvasController.DisableNavigation();
        }

        public override void Tick(float deltaTime)
        {
            RotateToPlayer(deltaTime);
            if (!_isInRange) CheckDistance();
            else MoveToTaxiTrigger(deltaTime);
        }

        public override void FixedTick(float fixedDeltaTime)
        {

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
                    stateMachine.State = PasserbyStates.MovetoTaxi;
                    stateMachine.ChangeState(stateMachine.State);
                }
            }
        }
    }
}