using cky.FCG.Pedestrian.StateMachine;
using UnityEngine;

namespace cky.FCG.Pedestrian.States
{
    public class WalkWithoutCheckingCar_State : PedestrianBaseState
    {
        PedestrianAnimatorController _animatorController;
        bool _exited;
        float timeCounter;
        float maxStayTime = 2.0f;

        public WalkWithoutCheckingCar_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.IsCurrentlyChoosen = false;

            Debug.Log($"{stateMachine.name} Walking Witout Checking Car");

            _animatorController = stateMachine.AnimatorController;
        }

        public override void Exit()
        {

        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {
            SetMoveAnimationBlend(deltaTime);

            if (_exited) return;

            stateMachine.RunIfNeed();

            stateMachine.AISight();

            ActionNearPedestrian();
            ActionNearSemaphore();
            //ActionNearCar();
            ActionNearPlayer();

            timeCounter += deltaTime;
            if (timeCounter > maxStayTime)
            {
                TransitionToPresetState();
            }
        }

        private void ActionNearPedestrian()
        {
            var nearPedestrian = stateMachine.NearPedestrian;
            if (stateMachine.NearPedestrian == null) return;

            _exited = true;
            stateMachine.ChangeStateWithDelay(PedestrianStates.Idle);
        }


        private void ActionNearSemaphore()
        {
            var nearSemaphore = stateMachine.NearSemaphore;
            if (nearSemaphore == null) return;

            if (!nearSemaphore.PeopleMoveState)
            {
                if (!stateMachine.IsInsideSemaphore)
                {
                    stateMachine.State = PedestrianStates.Idle;
                    stateMachine.ChangeState(stateMachine.State);
                }
            }
        }


        private void ActionNearCar()
        {
            var nearCar = stateMachine.NearCar;
            if (nearCar == null) return;

            stateMachine.State = PedestrianStates.Idle;
            stateMachine.ChangeState(stateMachine.State);
        }


        private void ActionNearPlayer()
        {
            var nearPlayer = stateMachine.NearPlayer;

            if (nearPlayer == null) return;

            stateMachine.State = PedestrianStates.Idle;
            stateMachine.ChangeState(stateMachine.State);
        }

        private void SetMoveAnimationBlend(float deltaTime)
        {
            var blend = stateMachine.MoveSpeedBlendValue;
            if (blend != stateMachine.Settings.walkSpeedBlend)
            {
                if (blend < stateMachine.Settings.walkSpeedBlend - 0.02f)
                {
                    stateMachine.MoveSpeedBlendValue += deltaTime * stateMachine.MoveSpeedBlendIncDecSpeed;
                }
                else if (blend > stateMachine.Settings.walkSpeedBlend + 0.02f)
                {
                    stateMachine.MoveSpeedBlendValue -= deltaTime * stateMachine.MoveSpeedBlendIncDecSpeed;
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