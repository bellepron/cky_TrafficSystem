using cky.StateMachine.Example1.Actor.StateMachine;
using UnityEngine;

namespace cky.StateMachine.Example1.Actor.States
{
    public class GoToBaseState : ActorBaseState
    {
        public GoToBaseState(ActorStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            stateMachine.InputReader.StopEvent += Stop;
        }

        public override void Exit()
        {
            stateMachine.InputReader.StopEvent -= Stop;
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            
        }

        public override void Tick(float deltaTime)
        {
            if (stateMachine.transform.position.x < stateMachine.BaseTr.position.x)
                stateMachine.transform.position += Vector3.right * deltaTime * stateMachine.MovementSpeed;
            else
                stateMachine.SwitchState(new GoToTargetState(stateMachine));
        }

        private void Stop()
        {
            stateMachine.SwitchState(new IdleState(stateMachine));
        }
    }
}