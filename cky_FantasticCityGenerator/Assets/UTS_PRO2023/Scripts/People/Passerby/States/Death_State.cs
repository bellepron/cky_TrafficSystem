//using cky.Car;
using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class Death_State : PasserbyBaseState
    {
        float timeCounter = 0.0f;

        public Death_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            stateMachine.IsAlive = false;
            stateMachine.RagdollController.Die();

            if (stateMachine.IsCurrentlyChoosen)
            {
                //DriverSingleton.Instance.AskQuest_EventTrigger();
            }
        }

        public override void Exit()
        {

        }

        public override void Tick(float deltaTime)
        {
            timeCounter += deltaTime;
            if (timeCounter > stateMachine.Settings.destroyTimeWhenDead)
            {
                stateMachine.MovePath.walkPath.SpawnPoints[stateMachine.MovePath.w].AddToSpawnQuery(new MovePathParams());

                MonoBehaviour.Destroy(stateMachine.gameObject);
            }
        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }
    }
}