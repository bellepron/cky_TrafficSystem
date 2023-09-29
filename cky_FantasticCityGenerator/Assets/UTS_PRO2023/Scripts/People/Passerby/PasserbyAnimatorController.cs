using cky.UTS.Helpers;
using UnityEngine;

namespace cky.UTS.People.Passersby
{
    public class PasserbyAnimatorController : MonoBehaviour
    {
        /* [SerializeField] */
        Animator _animator;
        [SerializeField] float transitionTime = 0.25f;

        private void Awake() => _animator = GetComponent<Animator>();

        public void Activate(bool b) => _animator.enabled = b;

        public void SetMoveSpeed(float value) => _animator.SetFloat(AnimatorHelper.a_MoveSpeed, value);

        public void EnterCar_Trigger() => _animator.SetTrigger(AnimatorHelper.a_EnterCar);
        public void ExitCar_Trigger() => _animator.SetTrigger(AnimatorHelper.a_ExitCar);
        public void WaveHands_Trigger() => _animator.SetTrigger(AnimatorHelper.a_WaveHands);

        public void LeanCarDoor_Trigger() => _animator.SetTrigger(AnimatorHelper.a_LeanCarDoor);
        public void NotLeanCarDoor_Trigger() => _animator.SetTrigger(AnimatorHelper.a_NotLeanCarDoor);


        public void ForDebugging()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            var currentStateName = stateInfo.fullPathHash;
            Debug.Log($"current state name: {currentStateName}");
        }
        public bool IsWavingHands()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorHelper.s_WaveHands))
                return true;
            return false;
        }
        public bool IsEnteringCar()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorHelper.s_EnterCar))
                return true;
            return false;
        }
        public bool IsExitingCar()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName(AnimatorHelper.s_ExitCar))
                return true;
            return false;
        }

        public void ActivateRootMotion(bool v) => _animator.applyRootMotion = v;
    }
}