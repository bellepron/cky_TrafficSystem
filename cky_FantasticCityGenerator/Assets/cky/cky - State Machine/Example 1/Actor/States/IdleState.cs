using cky.StateMachine.Example1.Actor.StateMachine;

namespace cky.StateMachine.Example1.Actor.States
{
    public class IdleState : ActorBaseState
    {
        public IdleState(ActorStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.InputReader.GoToBaseEvent += GoToBase;
            stateMachine.InputReader.GoToTargetEvent += GoToTarget;
        }

        public override void Exit()
        {
            stateMachine.InputReader.GoToBaseEvent -= GoToBase;
            stateMachine.InputReader.GoToTargetEvent -= GoToTarget;
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            
        }

        public override void Tick(float deltaTime)
        {

        }

        private void GoToBase()
        {
            stateMachine.SwitchState(new GoToBaseState(stateMachine));
        }

        private void GoToTarget()
        {
            stateMachine.SwitchState(new GoToTargetState(stateMachine));
        }
    }
}