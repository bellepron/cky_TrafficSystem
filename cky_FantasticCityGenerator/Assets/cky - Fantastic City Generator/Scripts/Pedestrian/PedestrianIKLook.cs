//using cky.Car;
//using cky.Helpers;
using UnityEngine;

namespace cky.FCG.Pedestrian.StateMachine
{
    public class PedestrianIKLook : MonoBehaviour
    {
        Transform _playerTr;
        PedestrianSettings settings;
        Animator animator;
        float weight = 1;

        [SerializeField] bool isOpen;
        bool _openedBefore;

        public Transform _playerHeadTr;

        public void Open()
        {
            isOpen = true;

            if (_openedBefore) return;

            _playerTr = GameObject.FindWithTag("Player").transform;
            _playerHeadTr = _playerTr.transform;
            settings = GetComponent<PedestrianStateMachine>().Settings;
            animator = GetComponent<Animator>();
            _openedBefore = true;
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (isOpen == false) return;

            if (_playerHeadTr == null)
            {
                _playerTr = GameObject.FindWithTag("Player").transform;
                _playerHeadTr = _playerTr.transform;
            }

            animator.SetLookAtWeight(1f * weight, .3f * weight, 1.0f * weight, .1f * weight, .8f * weight);
            animator.SetLookAtPosition(_playerHeadTr.position);
        }

        //private void FixedUpdate()
        //{
        //    Transform tr = transform;

        //    var direction = _playerHeadTransform.position - tr.position;
        //    direction.y = 0.0f;

        //    if (direction.magnitude < settings.ikLookDistance)
        //    {
        //        Debug.Log("OPENNN");
        //        OpenIKSlightly();
        //    }
        //    else
        //    {
        //        Debug.Log("CLOSEEE");
        //        CloseIKSlightly();
        //    }
        //}

        public void OpenIKSlightly()
        {
            weight = Mathf.Lerp(weight, 1f, Time.fixedDeltaTime);
        }
        public void CloseIKSlightly()
        {
            weight = Mathf.Lerp(weight, 0f, Time.fixedDeltaTime);
        }
    }
}