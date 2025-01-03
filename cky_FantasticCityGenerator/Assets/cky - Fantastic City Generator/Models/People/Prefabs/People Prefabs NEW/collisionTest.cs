using cky;
using cky.Managers;
using UnityEngine;

public class collisionTest : MonoBehaviour/*, IHitable*/
{
    [field: SerializeField] public bool IsAlive { get; set; }
    [field: SerializeField] public RagdollToggle RagdollToggle { get; set; }

    //public void Hited(HitType hitType, Vector3 hitRbCenterOfMass, Vector3 hitPoint, Vector3 hitPower, ForceMode forceMode, bool isCar = false)
    //{
    //    //if (isCar && hitPower.magnitude < 3) return;

    //    //gameObject.SetActive(false);
    //    RagdollToggle.Hited(hitPower * 100, forceMode);
    //}
}