//using cky.Car;
using cky.FCG.Pedestrian.StateMachine;
//using cky.Managers;

namespace cky.FCG.Pedestrian.States
{
    public class EnterCar_State : PedestrianBaseState
    {
        PedestrianAnimatorController _animatorController;
        bool _isStartedEnteringCar; // Cause of animation transitions.

        public EnterCar_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //UIManager.Instance.TaximeterController.StartTaximeter();

            //stateMachine.MakeKinematic(true);
            //stateMachine.AgentActivate(false);
            //PosRotStrike();
            //stateMachine.transform.parent = DriverSingleton.Instance.CharacterData.Car.transform;

            //_animatorController = stateMachine.AnimatorController;
            //_animatorController.ActivateRootMotion(false);

            //_animatorController.EnterCar_Trigger();
        }

        public override void Exit()
        {
            //PosRotStrike2();
        }

        public override void FixedTick(float fixedDeltaTime)
        {
            if (_animatorController.IsEnteringCar())
            {
                _isStartedEnteringCar = true;
            }
            else
            {
                if (_isStartedEnteringCar)
                {
                    ChangeToInCar();
                }
            }
        }

        public override void Tick(float deltaTime)
        {

        }

        private void ChangeToInCar()
        {
            //DriverSingleton.Instance.AddTaxiCustomer(stateMachine);

            //stateMachine.State = PedestrianStates.InCar;
            //stateMachine.ChangeState(stateMachine.State);
        }

        private void PosRotStrike()
        {
            //var customerEnterTransform = DriverSingleton.Instance.CharacterData.CarData.seats[1].seatInfo.customerEnterTransform;

            //stateMachine.transform.SetPositionAndRotation(customerEnterTransform.position, customerEnterTransform.rotation);
        }

        //private void PosRotStrike2()
        //{
        //    var customerTrWhenSit = DriverSingleton.Instance.CharacterData.CarData.seats[1].seatInfo.customerTrWhenSit;

        //    stateMachine.transform.SetPositionAndRotation(customerTrWhenSit.position, customerTrWhenSit.rotation);
        //}
    }
}