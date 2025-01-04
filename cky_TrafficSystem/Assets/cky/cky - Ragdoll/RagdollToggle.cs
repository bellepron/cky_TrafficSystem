using cky.Ragdoll;
using UnityEngine;
using UnityEngine.AI;

public class RagdollToggle : MonoBehaviour
{
    //[SerializeField] Animator animator;
    //[SerializeField] NavMeshAgent agent;
    //[SerializeField] CapsuleCollider capsuleCollider;
    //Collider[] childrenCollider;
    //Rigidbody[] childrenRigidbody;
    //[SerializeField] GameObject pelvis;
    //Rigidbody pelvisRb;

    //RagdollLimb[] ragdollLimbs;

    //bool _isFirstTime = true;
    //bool _isActive;

    //int _ragdollLimbsLength;

    //ConfigurableJoint[] configurableJoints;
    //int confJointCount;
    //float[] jointDrivesPosSpringMaximum;

    //private void OnEnable()
    //{
    //    if (_isFirstTime)
    //    {
    //        _isFirstTime = false;
    //        //childrenCollider = pelvis.GetComponentsInChildren<Collider>();
    //        //childrenRigidbody = pelvis.GetComponentsInChildren<Rigidbody>();
    //        pelvisRb = pelvis.GetComponent<Rigidbody>();

    //        ragdollLimbs = GetComponentsInChildren<RagdollLimb>();
    //        _ragdollLimbsLength = ragdollLimbs.Length;
    //        //childrenCollider = new Collider[_ragdollLimbsLength];
    //        childrenRigidbody = new Rigidbody[_ragdollLimbsLength];
    //        for (int i = 0; i < _ragdollLimbsLength; i++)
    //        {
    //            //childrenCollider[i] = ragdollLimbs[i].GetComponent<Collider>();
    //            childrenRigidbody[i] = ragdollLimbs[i].GetComponent<Rigidbody>();
    //        }

    //        configurableJoints = GetComponentsInChildren<ConfigurableJoint>();
    //        confJointCount = configurableJoints.Length;
    //        jointDrivesPosSpringMaximum = new float[confJointCount];
    //        for (int i = 0; i < confJointCount; i++)
    //        {
    //            jointDrivesPosSpringMaximum[i] = configurableJoints[i].angularXDrive.positionSpring;
    //        }
    //    }

    //    RagdollActivate(false);
    //}

    //public void RagdollActivate(bool active)
    //{
    //    _isActive = active;

    //    animator.enabled = !active;
    //    //capsuleCollider.enabled = !active;
    //    agent.enabled = !active;

    //    for (int i = 0; i < _ragdollLimbsLength; i++)
    //    {
    //        //childrenCollider[i].enabled = active;
    //        //childrenRigidbody[i].detectCollisions = active;

    //        //childrenRigidbody[i].isKinematic = !active;
    //        childrenRigidbody[i].constraints = active ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
    //    }
    //}



    //public void Hited(HitType hitType, Vector3 hitPoint, Vector3 hitVector, ForceMode forceMode, int dmg)
    //{
    //    // UYGULA
    //    RagdollActivate(true);

    //    pelvisRb.AddForce(hitVector.normalized * 500, forceMode);
    //}



    ////public void Hited(Vector3 hitVelocity, ForceMode forceMode = ForceMode.Impulse)
    ////{
    ////    var hitVeloMag = hitVelocity.magnitude;
    ////    if (!_isActive)
    ////    {
    ////        //Debug.Log(hitVeloMag);
    ////        if (hitVeloMag < 30)
    ////        {
    ////            RagdollActivate(true);
    ////        }
    ////        else
    ////        {
    ////            RagdollActivate(true);
    ////        }
    ////    }

    ////    var hv = hitVelocity + Vector3.up * (hitVeloMag * UnityEngine.Random.Range(0.15f, 0.25f));

    ////    var randomV = UnityEngine.Random.Range(7f, 8f);
    ////    var randomPercent = UnityEngine.Random.Range(5f, 9f) * 0.1f;

    ////    pelvisRb.AddForce(hv * randomV, ForceMode.Impulse);

    ////    for (int i = 0; i < _ragdollLimbsLength; i++)
    ////    {
    ////        childrenRigidbody[i].AddForce(hv * randomV * randomPercent, ForceMode.Impulse);
    ////    }


    ////    var multiplier = Mathf.Clamp(hitVeloMag, 0f, 50f) / 250;
    ////    var length = configurableJoints.Length;
    ////    for (int i = 0; i < length; i++)
    ////    {
    ////        JointDrive jDrive = new JointDrive() { maximumForce = Mathf.Infinity, positionDamper = 0, positionSpring = jointDrivesPosSpringMaximum[i] * multiplier };
    ////        //Debug.Log(jointDrivesPosSpringMaximum[i] * multiplier);

    ////        configurableJoints[i].angularXDrive = jDrive;
    ////        configurableJoints[i].angularYZDrive = jDrive;
    ////    }
    ////}
}