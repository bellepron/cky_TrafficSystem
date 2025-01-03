using UnityEngine;

namespace FCG
{
    public class SingleTrafficLightController : TrafficLightControllerAbstract
    {
        [SerializeField] TrafficLight trafficLight;

        [SerializeField] bool _startWithRed = true;

        [Space(15)]
        [Header("Extra Light")]
        [SerializeField] TrafficLightExtra extraLight;



        void Start()
        {
            _step = _startWithRed ? 0 : 2;

            EnableObjects();
        }

        public override void EnableObjects()
        {
            trafficLight.SetStatus(FindColor(_step));
        }



        public override void ReOpen()
        {
            //base.ReOpen();

            //trafficLight.ReOpen();

            //if (extraLight) extraLight.ReOpen();

            //if (IsFirstCreation)
            //{
            //    IsFirstCreation = false;

            //    trafficLight.interactableSettings = interactableSettings;
            //    extraLight.interactableSettings = interactableSettings;
            //}
        }
    }
}