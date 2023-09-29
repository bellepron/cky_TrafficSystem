//using cky.Car;
using cky.FCG.Pedestrian.StateMachine;
using UnityEngine;

namespace cky.FCG.Pedestrian.States
{
    public class Death_State : PedestrianBaseState
    {
        float timeCounter = 0.0f;

        public Death_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            //stateMachine.IsAlive = false;
            //stateMachine.AgentIsStopped(true);
            //stateMachine.RagdollController.Die();

            //if (stateMachine.IsCurrentlyChoosen)
            //{
            //    DriverSingleton.Instance.AskQuest_EventTrigger();
            //}
        }

        public override void Exit()
        {

        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        public override void Tick(float deltaTime)
        {
            timeCounter += deltaTime;
            if (timeCounter > stateMachine.Settings.destroyTimeWhenDead)
            {
                Debug.Log("Look At This");
                //stateMachine.MovePath.walkPath.SpawnPoints[stateMachine.MovePath.w].AddToSpawnQuery(new MovePathParams());

                stateMachine.DestroyObject();
            }
        }
    }
}