using UnityEngine;

namespace cky.StateMachine.Base
{
    public abstract class BaseStateMachine : MonoBehaviour
    {
        private BaseState currentState;

        public void SwitchState(BaseState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        private void Update() => Tick();
        private void FixedUpdate() => FixedTick();

        protected virtual void Tick() => currentState?.Tick(Time.deltaTime);
        protected virtual void FixedTick() => currentState?.FixedTick(Time.fixedDeltaTime);
    }
}