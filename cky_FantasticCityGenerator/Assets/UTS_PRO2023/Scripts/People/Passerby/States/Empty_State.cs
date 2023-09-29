using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class Empty_State : PasserbyBaseState
    {
        public Empty_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.AnimatorController.SetMoveSpeed(0.0f);

            stateMachine.SetTargetMoveSpeed(0.0f);
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