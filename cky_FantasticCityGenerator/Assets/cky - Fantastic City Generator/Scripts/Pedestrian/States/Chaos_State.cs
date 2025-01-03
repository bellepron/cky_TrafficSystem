using UnityEngine;
using UnityEngine.AI;

namespace cky.TrafficSystem
{
    public class Chaos_State : PedestrianBaseState
    {
        Transform _runAwayFromTr;
        float _timer;
        float _interval = 1.5f;
        float _checkRadius = 15.0f;
        float _controlAwayFrom = 15.0f; // Kontrol edilen çemderin kaçýlan þeyden uzaklýðý.
        Vector3 _targetPos;

        public Chaos_State(PedestrianStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.AgentMoveSpeed(stateMachine.Settings.runSpeed);

            _runAwayFromTr = stateMachine.player;

            _timer = _interval;
        }

        public override void Exit()
        {

        }

        public override void FixedTick(float fixedDeltaTime)
        {
            _timer += fixedDeltaTime;

            if (_timer > _interval)
            {
                _timer = 0.0f;

                _targetPos = RandomNavmeshLocation();
                if (_targetPos != Vector3.zero)
                {
                    stateMachine.AgentSetDestination(_targetPos);
                }
            }
        }

        public override void Tick(float deltaTime)
        {
            stateMachine.AnimatorMoveSpeedUpdate();
        }

        Vector3 RandomNavmeshLocation()
        {
            Vector3 randomDirection = Random.insideUnitSphere * _checkRadius;

            var controlPos = stateMachine.transform.position + (stateMachine.transform.position - _runAwayFromTr.position).normalized * _controlAwayFrom;
            randomDirection += controlPos;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, _checkRadius, 1))
            {
                return hit.position;
            }

            return Vector3.zero;
        }
    }
}