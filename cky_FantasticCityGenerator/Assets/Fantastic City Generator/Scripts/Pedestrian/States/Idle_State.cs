using cky.FCG.Pedestrian.StateMachine;
using UnityEngine;

namespace cky.FCG.Pedestrian.States
{
    public class Idle_State : PedestrianBaseState
    {
        bool _exited;

        public Idle_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.AgentIsStopped(true);
            stateMachine.IsIdle = 0.15f;
        }

        public override void Exit()
        {
            stateMachine.whoMakeMeStopTr = null;

            stateMachine.AgentIsStopped(false);
            stateMachine.IsIdle = 0.0f;
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            SetMoveAnimationBlend(fixedDeltaTime);

            //stateMachine.CurrentMoveSpeedUpdate(fixedDeltaTime);

            stateMachine.AISight();

            ActionNearSemaphore();

            if (_exited) return;

            stateMachine.IfCanReturnToNormalMove(ref _exited);

            stateMachine.RunIfNeed();
        }

        public override void Tick(float deltaTime)
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
                    stateMachine.MoveSpeedBlendValue = Mathf.MoveTowards(stateMachine.MoveSpeedBlendValue, 0.0f, deltaTime * stateMachine.MoveSpeedBlendIncDecSpeed);
                }
                else
                {
                    stateMachine.MoveSpeedBlendValue = 0.0f;
                }

                stateMachine.AnimatorController.SetMoveSpeed(stateMachine.MoveSpeedBlendValue);
            }

            //stateMachine.AnimatorController_MoveSpeed();
        }
    }
}