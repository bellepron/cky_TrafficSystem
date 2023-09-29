using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class Idle_State : PasserbyBaseState
    {
        bool _exited;

        public Idle_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            stateMachine.IsIdle = 0.15f;

            stateMachine.SetTargetMoveSpeed(0.0f);
        }

        public override void Exit()
        {
            stateMachine.IsIdle = 0.0f;
        }

        public override void Tick(float deltaTime)
        {
            SetMoveAnimationBlend(deltaTime);

            stateMachine.CurrentMoveSpeedUpdate(deltaTime);

            stateMachine.AISight();

            ActionNearSemaphore();

            stateMachine.IfCanReturnToNormalMove();
        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }


        private void ActionNearSemaphore()
        {
            var nearSemaphore = stateMachine.NearSemaphore;
            if (nearSemaphore == null) return;

            if (nearSemaphore.PeopleMoveState)
            {
                stateMachine.State = stateMachine.PreSet_State;
                stateMachine.ChangeState(stateMachine.State);
            }
        }

        private void SetMoveAnimationBlend(float deltaTime)
        {
            var blend = stateMachine.MoveSpeedBlendValue;
            if (blend != 0.0f)
            {
                if (blend > 0.005f)
                {
                    //stateMachine.MoveSpeedBlendValue -= deltaTime * stateMachine.MoveSpeedBlendSpeed;
                    stateMachine.MoveSpeedBlendValue = Mathf.MoveTowards(stateMachine.MoveSpeedBlendValue, 0.0f, deltaTime * stateMachine.MoveSpeedBlendSpeed);
                }
                else
                {
                    stateMachine.MoveSpeedBlendValue = 0.0f;
                }

                stateMachine.AnimatorController.SetMoveSpeed(stateMachine.MoveSpeedBlendValue);
            }
        }
    }
}