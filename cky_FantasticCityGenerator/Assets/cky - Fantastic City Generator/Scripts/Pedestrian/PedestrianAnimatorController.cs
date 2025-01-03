//using cky.Helpers;
using UnityEngine;

namespace cky.FCG.Pedestrian.StateMachine
{
    public class PedestrianAnimatorController : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        public void SetAnimatorMoveSpeedValue(float value) => _animator.SetFloat(/*AnimatorHelper.a_MoveSpeed*/"MoveSpeed", value);
    }
}