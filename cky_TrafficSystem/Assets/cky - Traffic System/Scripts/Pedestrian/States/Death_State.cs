
namespace cky.TrafficSystem
{
    public class Death_State : PedestrianBaseState
    {
        float timeCounter = 0.0f;

        public Death_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Death();
        }

        public override void Exit()
        {

        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {

        }
    }
}