using UnityEngine;

namespace cky.TrafficSystem
{
    public class TrafficLightController : TrafficLightControllerAbstract
    {
        [SerializeField] TrafficLight trafficLight_N;
        [SerializeField] TrafficLight trafficLight_S;
        [SerializeField] TrafficLight trafficLight_E;
        [SerializeField] TrafficLight trafficLight_W;

        [Space(15)]
        [Header("Extra Light")]
        [SerializeField] TrafficLightExtra extraLight;



        void Start()
        {
            _step = (Random.Range(1, 8) < 4) ? 0 : 2; // Means Red or Green

            EnableObjects();
        }

        public override void EnableObjects()
        {
            var color = FindColor(_step);
            trafficLight_N.SetStatus(color);
            trafficLight_S.SetStatus(color);

            color = FindColor((_step + 2) % 4);
            trafficLight_E.SetStatus(color);
            trafficLight_W?.SetStatus(color);
        }

        public override void ReOpen()
        {
            base.ReOpen();

            trafficLight_N.ReOpen();
            trafficLight_S.ReOpen();
            trafficLight_E.ReOpen();
            if (trafficLight_W) trafficLight_W.ReOpen();

            if (extraLight) extraLight.ReOpen();

            //if (IsFirstCreation)
            //{
            //    IsFirstCreation = false;

            //    trafficLight_N.interactableSettings = interactableSettings;
            //    trafficLight_S.interactableSettings = interactableSettings;
            //    trafficLight_E.interactableSettings = interactableSettings;
            //    if (trafficLight_W) trafficLight_W.interactableSettings = interactableSettings;

            //    if (extraLight) extraLight.interactableSettings = interactableSettings;
            //}
        }
    }
}