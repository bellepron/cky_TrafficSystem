//using cky.Car;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class EnterCar_State : PasserbyBaseState
    {
        PasserbyAnimatorController _animatorController;
        bool _isStartedEnteringCar; // Cause of animation transitions.

        public EnterCar_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            stateMachine.MakeKinematic(true);
            PosRotStrike();
            //stateMachine.transform.parent = DriverSingleton.Instance.CharacterData.Car.transform;

            _animatorController = stateMachine.AnimatorController;
            _animatorController.ActivateRootMotion(false);

            _animatorController.EnterCar_Trigger();
        }

        public override void Exit()
        {
            //PosRotStrike2();
        }

        public override void Tick(float deltaTime)
        {
            //_animatorController.ForDebugging();
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

        private void ChangeToInCar()
        {
            //DriverSingleton.Instance.AddTaxiCustomer(stateMachine);

            stateMachine.State = PasserbyStates.InCar;
            stateMachine.ChangeState(stateMachine.State);
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