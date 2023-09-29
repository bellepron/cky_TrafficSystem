using cky.StateMachine.Base;

namespace cky.FCG.Pedestrian.StateMachine
{
    public abstract class PedestrianBaseState : BaseState
    {
        protected PedestrianStateMachine stateMachine;

        public PedestrianBaseState(PedestrianStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }
    }
}