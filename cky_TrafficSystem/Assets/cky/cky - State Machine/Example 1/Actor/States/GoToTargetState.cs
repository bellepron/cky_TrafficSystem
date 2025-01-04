using cky.StateMachine.Example1.Actor.StateMachine;
using UnityEngine;

namespace cky.StateMachine.Example1.Actor.States
{
    public class GoToTargetState : ActorBaseState
    {
        private Vector3 _targetPos;
        private Vector3 _direction;
        public GoToTargetState(ActorStateMachine stateMachine) : base(stateMachine) { }

        public override void Enter()
        {
            _targetPos = stateMachine.TargetTr.position;
            _targetPos.y = stateMachine.transform.position.y;
            _direction = _targetPos - stateMachine.transform.position;
            _direction.Normalize();

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
            var distance = Vector3.Distance(_targetPos, stateMachine.transform.position);
            if (distance > 0.25f)
            {
                stateMachine.transform.position += _direction * deltaTime * stateMachine.MovementSpeed;
            }
            else
            {
                stateMachine.SwitchState(new GoToBaseState(stateMachine));
            }
        }

        private void Stop()
        {
            stateMachine.SwitchState(new IdleState(stateMachine));
        }
    }
}