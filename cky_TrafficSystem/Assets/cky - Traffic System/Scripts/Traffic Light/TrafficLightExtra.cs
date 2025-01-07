using UnityEngine.AI;
using UnityEngine;

namespace cky.TrafficSystem
{
    public class TrafficLightExtra : MonoBehaviour/*, IHitable*/
    {
        //SoundManager SoundManager;

        //[HideInInspector] public InteractableSettings interactableSettings;

        [Space(15)]
        [Header("Extra Light")]
        [SerializeField] Rigidbody rb;
        [SerializeField] NavMeshObstacle navMeshObstacle;

        //bool _isFirstCreation = true;
        //Vector3 _cachedPos;
        //Quaternion _cachedRot;

        public void ReOpen()
        {
            rb.isKinematic = true;

            //if (_isFirstCreation)
            //{
            //    _isFirstCreation = false;

            //    SoundManager = GameObject.FindWithTag(TagHelper.SOUNDMANAGER).GetComponent<SoundManager>();

            //    _cachedPos = transform.localPosition;
            //    _cachedRot = transform.localRotation;
            //}
            //else
            //{
            //    transform.localPosition = _cachedPos;
            //    transform.localRotation = _cachedRot;
            //}
        }

        //public void Hited(HitType hitType, Vector3 hitRbCenterOfMass, Vector3 hitPoint, Vector3 hitPower, ForceMode forceMode, bool isCar = false)
        //{
        //    if (!isCar) return;

        //    Hit(hitRbCenterOfMass, hitPoint, hitPower, forceMode);
        //}

        //public virtual void Hit(Vector3 hitRbCenterOfMass, Vector3 hitPoint, Vector3 hitPower, ForceMode forceMode)
        //{
        //    if (hitPower.magnitude > interactableSettings.powerLimit)
        //    {
        //        var hitPowerModified = hitPower; hitPowerModified.Normalize(); hitPowerModified += new Vector3(0, 0.35f, 0); hitPowerModified.Normalize();
        //        hitPowerModified *= 10;
        //        rb.AddForce(hitPowerModified, forceMode);
        //    }

        //    navMeshObstacle.enabled = false;
        //    rb.isKinematic = false;

        //    //SoundManager.LampBreakingSound(hitPoint);
        //    //SoundManager.LampBrokenElectricityParticle(transform.position);
        //}
    }
}