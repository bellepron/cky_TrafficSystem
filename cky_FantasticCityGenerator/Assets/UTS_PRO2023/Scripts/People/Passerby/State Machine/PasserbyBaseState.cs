using cky.StateMachine.Base;

namespace cky.UTS.People.Passersby.StateMachine
{
    public abstract class PasserbyBaseState : BaseState
    {
        protected PasserbyStateMachine stateMachine;

        public PasserbyBaseState(PasserbyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}