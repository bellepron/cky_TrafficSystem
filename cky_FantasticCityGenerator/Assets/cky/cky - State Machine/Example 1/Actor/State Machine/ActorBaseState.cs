using cky.StateMachine.Base;

namespace cky.StateMachine.Example1.Actor.StateMachine
{
    public abstract class ActorBaseState : BaseState
    {
        protected ActorStateMachine stateMachine;

        public ActorBaseState(ActorStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}