//using cky;
//using cky.Helpers;
//using cky.InteractableObjects;
//using cky.Managers;
using UnityEngine;
using UnityEngine.AI;

namespace FCG
{
    public class TrafficLight : MonoBehaviour/*, IHitable*/
    {
        //SoundManager SoundManager;

        //[HideInInspector] public InteractableSettings interactableSettings;

        [Space(5)]
        [SerializeField] TrafficLightColor lightColor;

        [Space(15)]
        [SerializeField] GameObject Green;
        [SerializeField] GameObject Yellow;
        [SerializeField] GameObject Red;
        [SerializeField] GameObject Pedestrians0;
        [SerializeField] GameObject Pedestrians1;
        [SerializeField] GameObject StopPedestrianCollider;
        [SerializeField] Crosswalk crosswalk;

        [Space(10)]
        [Header("ReCreate")]
        bool isWorking = false;
        [SerializeField] Rigidbody rb;
        [SerializeField] NavMeshObstacle navMeshObstacle;

        bool _isFirstCreation = true;
        Vector3 _cachedPosition;
        Quaternion _cachedRotation;



        private void Awake()
        {

            if (_isFirstCreation)
            {
                _isFirstCreation = false;

                //SoundManager = GameObject.FindWithTag(TagHelper.SOUNDMANAGER).GetComponent<SoundManager>();

                _cachedPosition = transform.position;
                _cachedRotation = transform.rotation;
            }
        }



        public void ReOpen()
        {
            isWorking = true;

            rb.isKinematic = true;
            navMeshObstacle.enabled = true;

            if (_isFirstCreation)
            {
                _isFirstCreation = false;

                //SoundManager = GameObject.FindWithTag(TagHelper.SOUNDMANAGER).GetComponent<SoundManager>();

                _cachedPosition = transform.position;
                _cachedRotation = transform.rotation;
            }
            else
            {
                transform.position = _cachedPosition;
                transform.rotation = _cachedRotation;
            }
        }



        public void SetStatus(TrafficLightColor color)
        {
            this.lightColor = color;

            switch (color)
            {
                case TrafficLightColor.Red: RedL(); break;
                case TrafficLightColor.Yellow: YellowL(); break;
                case TrafficLightColor.Green: GreenL(); break;
            }
        }

        private void RedL()
        {
            if (isWorking)
            {
                Red.SetActive(true);
                Yellow.SetActive(false);
                Green.SetActive(false);
            }
            PedestrianStatus(true);
        }
        private void YellowL()
        {
            if (isWorking)
            {
                Red.SetActive(false);
                Yellow.SetActive(true);
                Green.SetActive(false);
            }
            PedestrianStatus(false);
        }
        private void GreenL()
        {
            if (isWorking)
            {
                Red.SetActive(false);
                Yellow.SetActive(false);
                Green.SetActive(true);
            }
            PedestrianStatus(false);
        }
        private void PedestrianStatus(bool active)
        {
            if (isWorking)
            {
                Pedestrians0.SetActive(active);
                Pedestrians1.SetActive(active);
            }
            crosswalk.PedestrianGreenLight(active);
        }

        //public void Hited(HitType hitType, Vector3 hitRbCenterOfMass, Vector3 hitPoint, Vector3 hitPower, ForceMode forceMode, bool isCar = false)
        //{
        //    if (!isCar) return;

        //    Hit(hitType, hitRbCenterOfMass, hitPoint, hitPower, forceMode);
        //}

        //public virtual void Hit(HitType hitType, Vector3 hitRbCenterOfMass, Vector3 hitPoint, Vector3 hitPower, ForceMode forceMode)
        //{
        //    if (!isWorking) return;

        //    if (hitPower.magnitude > interactableSettings.powerLimit)
        //    {
        //        isWorking = false;
        //        var hitPowerModified = hitPower; hitPowerModified.Normalize(); hitPowerModified += new Vector3(0, 0.35f, 0); hitPowerModified.Normalize();
        //        hitPowerModified *= 10;
        //        rb.AddForce(hitPowerModified, forceMode);

        //        navMeshObstacle.enabled = false;
        //        rb.isKinematic = false;

        //        SoundManager.LampBreakingSound(hitPoint);
        //        SoundManager.LampBrokenElectricityParticle(transform.position);
        //    }
        //}
    }
}