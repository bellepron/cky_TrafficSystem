using UnityEngine;

namespace cky.TrafficSystem
{
    public enum TrafficLightColor { Red, Yellow, Green }

    public abstract class TrafficLightControllerAbstract : MonoBehaviour
    {
        [SerializeField] protected TrafficLightSettings settings;
        //[SerializeField] protected InteractableSettings interactableSettings;

        protected float _countTime;
        [SerializeField] protected int _step;
        private string _funktionName = "TrafficLightTurn";

        public bool IsActive { get; set; } = false;

        protected bool IsFirstCreation { get; set; } = true;

        private void OnEnable()
        {
            ReOpen();
            InvokeRepeating(nameof(TrafficLightTurn), 0, 1);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(TrafficLightTurn));
        }

        public virtual void TrafficLightTurn()
        {
            _countTime += 1;

            switch (_step)
            {
                case 0:
                    {
                        if (_countTime >= settings.redGreenTime)
                        {
                            _step = 1; _countTime = 0; EnableObjects();
                        }

                        break;
                    }
                case 1:
                    {
                        if (_countTime >= settings.yellowTime)
                        {
                            _step = 2; _countTime = 0; EnableObjects();
                        }

                        break;
                    }
                case 2:
                    {
                        if (_countTime >= settings.redGreenTime)
                        {
                            _step = 3; _countTime = 0; EnableObjects();
                        }

                        break;
                    }
                case 3:
                    {
                        if (_countTime >= settings.yellowTime)
                        {
                            _step = 0; _countTime = 0; EnableObjects();
                        }

                        break;
                    }
            }
        }

        public abstract void EnableObjects();

        protected TrafficLightColor FindColor(int status)
        {
            switch (status)
            {
                case 0:
                    return TrafficLightColor.Red;
                case 1:
                    return TrafficLightColor.Yellow;
                case 2:
                    return TrafficLightColor.Green;
                case 3:
                    return TrafficLightColor.Yellow;

                default:
                    return TrafficLightColor.Red;
            }
        }



        public virtual void ReOpen()
        {
            IsActive = true;
        }
    }
}