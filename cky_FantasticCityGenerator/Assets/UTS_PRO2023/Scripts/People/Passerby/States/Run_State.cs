
namespace cky.UTS.People.Passersby.StateMachine
{
    public class Run_State : PasserbyBaseState
    {
        PasserbyAnimatorController _animatorController;
        bool _exited;

        public Run_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            _animatorController = stateMachine.AnimatorController;
            _animatorController.ActivateRootMotion(true);

            stateMachine.SetTargetMoveSpeed(stateMachine.Settings.runSpeed);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {
            SetMoveAnimationBlend(deltaTime);

            if (_exited) return;

            stateMachine.CurrentMoveSpeedUpdate(deltaTime);

            stateMachine.AISight();

            ActionNearPasserby();
            ActionNearSemaphore();
            ActionNearCar();
            ActionNearPlayer();
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
            if (blend != stateMachine.Settings.runSpeedBlend)
            {
                if (blend < stateMachine.Settings.runSpeedBlend - 0.02f)
                {
                    stateMachine.MoveSpeedBlendValue += deltaTime * stateMachine.MoveSpeedBlendSpeed;
                }
                else
                {
                    stateMachine.MoveSpeedBlendValue = stateMachine.Settings.runSpeedBlend;
                }

                stateMachine.AnimatorController.SetMoveSpeed(stateMachine.MoveSpeedBlendValue);
            }
        }
    }
}