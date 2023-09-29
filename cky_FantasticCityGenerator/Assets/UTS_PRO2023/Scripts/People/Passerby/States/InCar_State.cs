//using cky.Car;
//using cky.Managers;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class InCar_State : PasserbyBaseState
    {
        //Transform _carTr;
        //bool _isOnTheTargetAngle = true;

        public InCar_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            //DriverSingleton.Instance.IsOnQuest(true);
            //_carTr = DriverSingleton.Instance.CharacterData.Car.transform;

            //UIManager.Instance.QuestProvider.ChooseCustomerDestination();
        }

        public override void Exit()
        {

        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {
            //if (_isOnTheTargetAngle == true) return;

            //RotateToCarDirection(deltaTime);
        }

        //private void RotateToCarDirection(float deltaTime)
        //{
        //    var tr = stateMachine.transform;

        //    var dir = _carTr.forward;
        //    var angle = Vector3.Angle(tr.forward, dir);

        //    if (angle > 1.0f)
        //    {
        //        tr.rotation = Quaternion.Slerp(
        //            tr.rotation,
        //            Quaternion.LookRotation(dir),
        //            10 * deltaTime);

        //        Debug.Log(angle);
        //    }
        //    else
        //    {
        //        tr.rotation = Quaternion.LookRotation(dir);

        //        _isOnTheTargetAngle = true;
        //    }
        //}
    }
}