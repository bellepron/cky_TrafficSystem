//using cky.Helpers;
using UnityEngine;

namespace cky.FCG.Pedestrian.StateMachine
{
    public class PedestrianAnimatorController : MonoBehaviour
    {
       Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetAnimatorMoveSpeedValue(float value) => _animator.SetFloat(/*AnimatorHelper.a_MoveSpeed*/"MoveSpeed", value);
    }
}