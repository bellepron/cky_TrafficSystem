
namespace cky.TrafficSystem
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
            if (_exited) return;

            stateMachine.AISight();

            //ActionNearPedestrian();
            //ActionNearSemaphore();
            ActionNearCar();
            //ActionNearPlayer();
            //ActionNearStopLight();

            //stateMachine.ReturnWalkingFromRunning();

            stateMachine.MoveOnTrafficSystem();
        }

        public override void Tick(float deltaTime)
        {

        }

        private void ActionNearPedestrian()
        {
            var nearPedestrian = stateMachine.NearPedestrian;
            if (stateMachine.NearPedestrian == null) return;

            stateMachine.whoMakeMeStopTr = nearPedestrian.transform;

            stateMachine.ChangeState(PedestrianStates.Idle);
        }


        //private void ActionNearSemaphore()
        //{
        //    var nearSemaphore = stateMachine.NearSemaphore;
        //    if (nearSemaphore == null) return;

        //    if (!nearSemaphore.PeopleMoveState)
        //    {
        //        if (!stateMachine.IsInsideSemaphore)
        //        {
        //            stateMachine.whoMakeMeStopTr = nearSemaphore.transform;
        //            stateMachine.State = PedestrianStates.Idle;
        //            stateMachine.ChangeState(stateMachine.State);
        //        }
        //    }
        //}


        private void ActionNearCar()
        {
            var nearCar = stateMachine.NearCar;
            if (nearCar == null) return;

            stateMachine.whoMakeMeStopTr = nearCar;

            stateMachine.ChangeState(PedestrianStates.Idle);
        }


        //private void ActionNearPlayer()
        //{
        //    var nearPlayer = stateMachine.NearPlayer;

        //    if (nearPlayer == null) return;

        //    stateMachine.whoMakeMeStopTr = nearPlayer;

        //    stateMachine.ChangeState(PedestrianStates.Idle);
        //}

        //private void ActionNearStopLight()
        //{
        //    var nearStopLight = stateMachine.NearStopLight;
        //    if (nearStopLight == null) return;

        //    stateMachine.whoMakeMeStopTr = nearStopLight.transform;

        //    stateMachine.ChangeState(PedestrianStates.Idle);
        //}
    }
}