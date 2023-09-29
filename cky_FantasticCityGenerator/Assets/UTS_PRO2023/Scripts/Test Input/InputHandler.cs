using cky.UTS.People.Passersby.StateMachine;
using UnityEngine;

namespace cky.UTS.TestInput
{
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] PasserbyStateMachine[] passersby;

        private void Start()
        {
            passersby = FindObjectsOfType<PasserbyStateMachine>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                passersby[0].ChangeWithInputHandler();
            }
        }
    }
}