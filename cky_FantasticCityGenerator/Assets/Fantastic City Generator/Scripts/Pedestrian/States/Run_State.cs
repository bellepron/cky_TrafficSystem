using cky.FCG.Pedestrian.StateMachine;

namespace cky.FCG.Pedestrian.States
{
    public class Run_State : PedestrianBaseState
    {
        PedestrianAnimatorController _animatorController;
        bool _exited;

        public Run_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            _animatorController = stateMachine.AnimatorController;
            stateMachine.AgentMoveSpeed(stateMachine.Settings.runSpeed);
        }

        public override void Exit()
        {

        }

        public override void FixedTick(float fixedDeltaTime)
        {
            SetMoveAnimationBlend(fixedDeltaTime);

            if (_exited) return;

            stateMachine.AISight();

            ActionNearPedestrian();
            ActionNearSemaphore();
            ActionNearCar();
            ActionNearPlayer();

            stateMachine.ReturnWalkingFromRunning();

            stateMachine.ckyMove();
        }

        public override void Tick(float deltaTime)
        {

        }

        private void ActionNearPedestrian()
        {
            var nearPedestrian = stateMachine.NearPedestrian;
            if (stateMachine.NearPedestrian == null) return;

            stateMachine.whoMakeMeStopTr = nearPedestrian.transform;

            stateMachine.State = PedestrianStates.Idle;
            stateMachine.ChangeState(stateMachine.State);
        }


        private void ActionNearSemaphore()
        {
            var nearSemaphore = stateMachine.NearSemaphore;
            if (nearSemaphore == null) return;

            if (!nearSemaphore.PeopleMoveState)
            {
                if (!stateMachine.IsInsideSemaphore)
                {
                    stateMachine.whoMakeMeStopTr = nearSemaphore.transform;
                    stateMachine.State = PedestrianStates.Idle;
                    stateMachine.ChangeState(stateMachine.State);
                }
            }
        }


        private void ActionNearCar()
        {
            var nearCar = stateMachine.NearCar;
            if (nearCar == null) return;

            stateMachine.whoMakeMeStopTr = nearCar;
            stateMachine.State = PedestrianStates.Idle;
            stateMachine.ChangeState(stateMachine.State);
        }


        private void ActionNearPlayer()
        {
            var nearPlayer = stateMachine.NearPlayer;

            if (nearPlayer == null) return;

            stateMachine.whoMakeMeStopTr = nearPlayer;

            stateMachine.State = PedestrianStates.Idle;
            stateMachine.ChangeState(stateMachine.State);
        }

        private void SetMoveAnimationBlend(float deltaTime)
        {
            var blend = stateMachine.MoveSpeedBlendValue;
            if (blend != stateMachine.Settings.runSpeedBlend)
            {
                if (blend < stateMachine.Settings.runSpeedBlend - 0.02f)
                {
                    stateMachine.MoveSpeedBlendValue += deltaTime * stateMachine.MoveSpeedBlendIncDecSpeed;
                }
                else
                {
                    stateMachine.MoveSpeedBlendValue = stateMachine.Settings.runSpeedBlend;
                }

                _animatorController.SetMoveSpeed(stateMachine.MoveSpeedBlendValue);
            }

            //stateMachine.AnimatorController_MoveSpeed();
        }
    }
}