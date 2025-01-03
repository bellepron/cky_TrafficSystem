using UnityEngine;

namespace cky.TrafficSystem
{
    public class PedestrianAnimatorController : MonoBehaviour
    {
        Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetAnimatorMoveSpeedValue(float value) => _animator.SetFloat("MoveSpeed", value);
    }
}