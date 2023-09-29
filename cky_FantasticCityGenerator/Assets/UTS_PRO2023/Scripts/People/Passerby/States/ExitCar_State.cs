//using cky.Car;
using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class ExitCar_State : PasserbyBaseState
    {
        public ExitCar_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.AnimatorController.ExitCar_Trigger();
        }

        public override void Exit()
        {
            LastStrike();
        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {
            //RotateToPlayer(deltaTime);
            TransitionToPresetStateAfterAnimationEnds(deltaTime);
        }

        float timeCounter;
        private void TransitionToPresetStateAfterAnimationEnds(float deltaTime)
        {
            timeCounter += deltaTime;
            if (timeCounter < 0.5f) return;

            if (!stateMachine.AnimatorController.IsExitingCar())
            {
                stateMachine.MakeKinematic(false);

                stateMachine.State = stateMachine.PreSet_State;
                stateMachine.ChangeState(stateMachine.State);
            }
        }

        private void LastStrike()
        {
            Debug.Log("Last Strike");

            //var seatInfo = DriverSingleton.Instance.CharacterData.CarData.seats[1].seatInfo;

            //stateMachine.transform.SetPositionAndRotation(seatInfo.customerExitTransform.position, seatInfo.customerExitTransform.rotation);
        }
    }
}