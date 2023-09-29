//using cky.Car;
using cky.FCG.Pedestrian.StateMachine;
//using cky.Managers;
using UnityEngine;

namespace cky.FCG.Pedestrian.States
{
    public class ExitCar_State : PedestrianBaseState
    {
        float _timeCounter;

        public ExitCar_State(PedestrianStateMachine stateMachine) : base(stateMachine)
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
        private void TransitionToPresetStateAfterAnimationEnds(float deltaTime)
        {
            //_timeCounter += deltaTime;
            //if (_timeCounter < 0.5f) return;

            //if (!stateMachine.AnimatorController.IsExitingCar())
            //{
            //    UIManager.Instance.TaximeterController.StopTaximeter();

            //    stateMachine.MakeKinematic(false);
            //    stateMachine.AgentActivate(true);

            //    stateMachine.State = stateMachine.PreSet_State;
            //    stateMachine.ChangeState(stateMachine.State);
            //}
        }

        private void LastStrike()
        {
            Debug.Log("Last Strike");

            //var seatInfo = DriverSingleton.Instance.CharacterData.CarData.seats[1].seatInfo;

            //stateMachine.transform.SetPositionAndRotation(seatInfo.customerExitTransform.position, seatInfo.customerExitTransform.rotation);
        }
    }
}