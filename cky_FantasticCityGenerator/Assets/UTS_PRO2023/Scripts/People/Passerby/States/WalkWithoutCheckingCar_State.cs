using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class WalkWithoutCheckingCar_State : PasserbyBaseState
    {
        PasserbyAnimatorController _animatorController;
        bool _exited;
        float timeCounter;
        float maxStayTime = 2.0f;

        public WalkWithoutCheckingCar_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.IsCurrentlyChoosen = false;

            Debug.Log($"{stateMachine.name} Walking Witout Checking Car");

            _animatorController = stateMachine.AnimatorController;

            stateMachine.SetTargetMoveSpeed(stateMachine.Settings.walkSpeed);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {
            SetMoveAnimationBlend(deltaTime);

            if (_exited) return;

            stateMachine.RunIfNeed();
            stateMachine.CurrentMoveSpeedUpdate(deltaTime);

            stateMachine.AISight();

            ActionNearPasserby();
            ActionNearSemaphore();
            //ActionNearCar();
            ActionNearPlayer();

            timeCounter += deltaTime;
            if (timeCounter > maxStayTime)
            {
                TransitionToPresetState();
            }
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            stateMachine.GetPath(fixedDeltaTime);
        }

        private void ActionNearPasserby()
        {
            var nearPasserby = stateMachine.NearPasserby;
            if (stateMachine.NearPasserby == null) return;

            _exited = true;
            stateMachine.ChangeStateWithDelay(PasserbyStates.Idle);
        }


        private void ActionNearSemaphore()
        {
            var nearSemaphore = stateMachine.NearSemaphore;
            if (nearSemaphore == null) return;

            if (!nearSemaphore.PeopleMoveState)
            {
                if (!stateMachine.IsInsideSemaphore)
                {
                    stateMachine.State = PasserbyStates.Idle;
                    stateMachine.ChangeState(stateMachine.State);
                }
            }
        }


        private void ActionNearCar()
        {
            var nearCar = stateMachine.NearCar;
            if (nearCar == null) return;

            stateMachine.State = PasserbyStates.Idle;
            stateMachine.ChangeState(stateMachine.State);
        }


        private void ActionNearPlayer()
        {
            var nearPlayer = stateMachine.NearPlayer;

            if (nearPlayer == null) return;

            stateMachine.State = PasserbyStates.Idle;
            stateMachine.ChangeState(stateMachine.State);
        }

        private void SetMoveAnimationBlend(float deltaTime)
        {
            var blend = stateMachine.MoveSpeedBlendValue;
            if (blend != stateMachine.Settings.walkSpeedBlend)
            {
                if (blend < stateMachine.Settings.walkSpeedBlend - 0.02f)
                {
                    stateMachine.MoveSpeedBlendValue += deltaTime * stateMachine.MoveSpeedBlendSpeed;
                }
                else if (blend > stateMachine.Settings.walkSpeedBlend + 0.02f)
                {
                    stateMachine.MoveSpeedBlendValue -= deltaTime * stateMachine.MoveSpeedBlendSpeed;
                }
                else
                {
                    stateMachine.MoveSpeedBlendValue = stateMachine.Settings.walkSpeedBlend;
                }

                _animatorController.SetMoveSpeed(stateMachine.MoveSpeedBlendValue);
            }
        }

        public void TransitionToPresetState()
        {
            stateMachine.State = stateMachine.PreSet_State;
            stateMachine.ChangeState(stateMachine.State);
        }
    }
}