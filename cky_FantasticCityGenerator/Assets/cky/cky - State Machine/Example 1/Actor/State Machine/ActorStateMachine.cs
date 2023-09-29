using cky.StateMachine.Example1.Actor.States;
using cky.StateMachine.Base;
using UnityEngine;

namespace cky.StateMachine.Example1.Actor.StateMachine
{
    public class ActorStateMachine : BaseStateMachine
    {
        [field: SerializeField] public InputReader InputReader { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; } = 3.0f;
        [field: SerializeField] public Transform BaseTr { get; private set; }
        [field: SerializeField] public Transform TargetTr { get; private set; }
        //[field: SerializeField] public Animator Animator { get; private set; }
        //[field: SerializeField] public Targeter Targeter { get; private set; }

        private void Start()
        {
            SwitchState(new IdleState(this));
        }
    }
}