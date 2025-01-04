
namespace cky.TrafficSystem
{
    public class Idle_State : PedestrianBaseState
    {
        bool _exited;

        public Idle_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.AgentStop(true);
        }

        public override void Exit()
        {
            stateMachine.whoMakeMeStopTr = null;

            stateMachine.AgentStop(false);
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            stateMachine.AISight();

            if (_exited) return;

            stateMachine.IfCanReturnToNormalMove(ref _exited);

            stateMachine.AnimatorMoveSpeedUpdate();
        }

        public override void Tick(float deltaTime)
        {

        }
    }
}