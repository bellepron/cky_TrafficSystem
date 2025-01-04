using System;
using UnityEngine;

namespace cky.StateMachine.Example1
{
    public class InputReader : MonoBehaviour
    {
        public event Action GoToBaseEvent;
        public event Action GoToTargetEvent;
        public event Action StopEvent;

        private void Start()
        {
            Debug.Log($"{this.name} Commands:\n" +
                      $"Space: Go To Base.\n" +
                      $"G: Go To Target.\n" +
                      $"S: Stop!");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GoToBaseEvent?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {
                GoToTargetEvent?.Invoke();
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StopEvent?.Invoke();
            }
        }
    }
}