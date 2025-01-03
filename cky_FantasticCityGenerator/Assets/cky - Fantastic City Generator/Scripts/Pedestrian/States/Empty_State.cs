using cky.FCG.Pedestrian.StateMachine;

namespace cky.FCG.Pedestrian.States
{
    public class Empty_State : PedestrianBaseState
    {
        public Empty_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.AnimatorController.SetAnimatorMoveSpeedValue(0.0f);
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {

        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }
    }
}