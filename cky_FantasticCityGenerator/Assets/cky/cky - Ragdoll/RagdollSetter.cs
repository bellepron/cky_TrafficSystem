using UnityEngine;

namespace cky.Ragdoll
{
    public class RagdollSetter : MonoBehaviour
    {
        //[field: SerializeField] public RagdollSettings Settings { get; private set; }

        //private RagdollLimb[] _limbs;
        //private int _limbCount;



        //#region Set

        //public void Set()
        //{
        //    Delete();

        //    _limbs = GetComponentsInChildren<RagdollLimb>();
        //    _limbCount = _limbs.Length;

        //    for (int i = 0; i < _limbCount; i++)
        //    {
        //        var type = _limbs[i].bodyPart;
        //        var info = Settings.infos[i];
        //        var limb = _limbs[i];

        //        switch (type)
        //        {
        //            case LimbTypes.PELVIS:
        //                ForBoxCollider(limb, info, true);
        //                break;
        //            case LimbTypes.MIDDLESPINE:
        //                ForBoxCollider(limb, info);
        //                break;
        //            case LimbTypes.HEAD:
        //                ForSphereCollider(limb, info);
        //                break;

        //            default:
        //                ForCapsuleCollider(limb, info);
        //                break;
        //        }

        //        var rb = _limbs[i].GetComponent<Rigidbody>();
        //        rb.interpolation = Settings.interpolation;
        //        rb.collisionDetectionMode = Settings.collisionDetectionMode;
        //    }
        //}

        //private void ForBoxCollider(RagdollLimb limb, RagdollInfo info, bool isPelvis = false)
        //{
        //    var trash = limb.GetComponents<BoxCollider>();
        //    foreach (var coll in trash)
        //    {
        //        DestroyImmediate(coll);
        //    }
        //    var col = limb.gameObject.AddComponent<BoxCollider>();
        //    col.center = info.colliderCenter;
        //    col.size = info.colliderSize;
        //    col.material = Settings.physicMat;

        //    SetRigidbody(limb, info);

        //    if (isPelvis == false)
        //    {
        //        ConfigurableJointSave(limb, info);
        //    }
        //}

        //private void ForCapsuleCollider(RagdollLimb limb, RagdollInfo info)
        //{
        //    var trash = limb.GetComponents<CapsuleCollider>();
        //    foreach (var coll in trash)
        //    {
        //        DestroyImmediate(coll);
        //    }
        //    var col = limb.gameObject.AddComponent<CapsuleCollider>();
        //    col.center = info.colliderCenter;
        //    col.radius = info.colliderRadius;
        //    col.height = info.colliderHeight;
        //    col.direction = info.capsuleColliderDirection;
        //    col.material = Settings.physicMat;

        //    SetRigidbody(limb, info);
        //    ConfigurableJointSave(limb, info);
        //}

        //private void ForSphereCollider(RagdollLimb limb, RagdollInfo info)
        //{
        //    var trash = limb.GetComponents<SphereCollider>();
        //    foreach (var coll in trash)
        //    {
        //        DestroyImmediate(coll);
        //    }
        //    var col = limb.gameObject.AddComponent<SphereCollider>();
        //    col.center = info.colliderCenter;
        //    col.radius = info.colliderRadius;
        //    col.material = Settings.physicMat;

        //    SetRigidbody(limb, info);
        //    ConfigurableJointSave(limb, info);
        //}

        //private void SetRigidbody(RagdollLimb limb, RagdollInfo info)
        //{
        //    if (limb.TryGetComponent<Rigidbody>(out var rb) == false)
        //    {
        //        rb = limb.gameObject.AddComponent<Rigidbody>();

        //    }
        //    rb.mass = info.rigidbodyMass;
        //}

        ////private void CharacterJointSave(RagdollLimb limb, RagdollInfo info)
        ////{
        ////    if (limb.TryGetComponent<CharacterJoint>(out var characterJoint) == false)
        ////    {
        ////        characterJoint = limb.AddComponent<CharacterJoint>();
        ////    }

        ////    characterJoint.connectedBody = ConvertToRigidbody(info.connectedLimbType);
        ////    characterJoint.anchor = info.anchor;
        ////    characterJoint.axis = info.axis;
        ////    characterJoint.connectedAnchor = info.connectedAnchor;
        ////    characterJoint.swingAxis = info.swingAxis;
        ////}

        //private void ConfigurableJointSave(RagdollLimb limb, RagdollInfo info)
        //{
        //    JointDrive jDrive;
        //    if (limb.bodyPart == LimbTypes.LEFTFOREARM || limb.bodyPart == LimbTypes.RIGHTFOREARM || limb.bodyPart == LimbTypes.LEFTCALF || limb.bodyPart == LimbTypes.RIGHTCALF) jDrive = new JointDrive() { maximumForce = Mathf.Infinity, positionDamper = 0, positionSpring = Settings.drivePS_CalfsAndArms };
        //    else if (limb.bodyPart == LimbTypes.MIDDLESPINE || limb.bodyPart == LimbTypes.MIDDLESPINE) jDrive = new JointDrive() { maximumForce = Mathf.Infinity, positionDamper = 0, positionSpring = Settings.drivePS_MiddleSpineAndHead };
        //    else jDrive = new JointDrive() { maximumForce = Mathf.Infinity, positionDamper = 0, positionSpring = Settings.drivePS };

        //    if (limb.TryGetComponent<ConfigurableJoint>(out var configurableJoint) == false)
        //    {
        //        configurableJoint = limb.gameObject.AddComponent<ConfigurableJoint>();
        //    }

        //    configurableJoint.connectedBody = ConvertToRigidbody(info.connectedLimbType);
        //    configurableJoint.anchor = info.anchor;
        //    configurableJoint.axis = info.axis;
        //    configurableJoint.connectedAnchor = info.connectedAnchor;
        //    configurableJoint.xMotion = ConfigurableJointMotion.Locked;
        //    configurableJoint.yMotion = ConfigurableJointMotion.Locked;
        //    configurableJoint.zMotion = ConfigurableJointMotion.Locked;
        //    configurableJoint.angularXDrive = jDrive;
        //    configurableJoint.angularYZDrive = jDrive;
        //    //configurableJoint.enableProjection = true;
        //}

        //private Rigidbody ConvertToRigidbody(LimbTypes type)
        //{
        //    for (int i = 0; i < _limbCount; i++)
        //    {
        //        if (type == _limbs[i].bodyPart)
        //            return _limbs[i].GetComponent<Rigidbody>();
        //    }

        //    Debug.Log($"{type} Uzuv eksik");
        //    return null;
        //}

        //public void GetHitVelocity(Vector3 hitVelocity)
        //{
        //    for (int i = 0; i < _limbCount; i++)
        //    {
        //        if (LimbTypes.PELVIS == _limbs[i].bodyPart)
        //        {
        //            _limbs[i].GetComponent<Rigidbody>().AddForce((hitVelocity + new Vector3(0, 5, 0)), ForceMode.VelocityChange);
        //        }
        //        //else // çok gerekli deðil gibi
        //        //{
        //        //    _limbs[i].GetComponent<Rigidbody>().AddForce((hitVelocity + new Vector3(0, 3, 0)) * 50, ForceMode.VelocityChange);
        //        //}
        //    }
        //}

        //#endregion



        //#region Editor Delete Collider & Rigidbody & Joints

        //public void Delete()
        //{
        //    _limbs = GetComponentsInChildren<RagdollLimb>();
        //    _limbCount = _limbs.Length;

        //    for (int i = 0; i < _limbCount; i++)
        //    {
        //        var limb = _limbs[i];
        //        Debug.Log(limb.bodyPart);

        //        if (limb.TryGetComponent<Collider>(out var collider))
        //        {
        //            DestroyImmediate(collider);
        //        }
        //        if (limb.TryGetComponent<CharacterJoint>(out var charJoint))
        //        {
        //            DestroyImmediate(charJoint);
        //        }
        //        if (limb.TryGetComponent<ConfigurableJoint>(out var confJoint))
        //        {
        //            DestroyImmediate(confJoint);
        //        }
        //        if (limb.TryGetComponent<Rigidbody>(out var rigidbody))
        //        {
        //            DestroyImmediate(rigidbody);
        //        }
        //    }
        //}

        //#endregion

    }
}