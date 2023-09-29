using System;
using Unity.VisualScripting;
using UnityEngine;

namespace cky.Ragdoll
{
    public class RagdollSetter : MonoBehaviour
    {
        [field: SerializeField] public RagdollSettings Settings { get; private set; }

        private RagdollLimb[] _limbs;
        private int _limbCount;


        #region Set

        public void Set()
        {
            _limbs = GetComponentsInChildren<RagdollLimb>();
            _limbCount = _limbs.Length;

            for (int i = 0; i < _limbCount; i++)
            {
                var type = _limbs[i].limbType;
                var info = Settings.infos[i];
                var limb = _limbs[i];

                switch (type)
                {
                    case LimbTypes.PELVIS:
                        ForBoxCollider(limb, info, true);
                        break;
                    case LimbTypes.MIDDLESPINE:
                        ForBoxCollider(limb, info);
                        break;
                    case LimbTypes.HEAD:
                        ForSphereCollider(limb, info);
                        break;

                    default:
                        ForCapsuleCollider(limb, info);
                        break;
                }
            }
        }

        private void ForBoxCollider(RagdollLimb limb, RagdollInfo info, bool isPelvis = false)
        {
            var col = limb.AddComponent<BoxCollider>();
            col.center = info.colliderCenter;
            col.size = info.colliderSize;

            SetRigidbody(limb, info);

            if (isPelvis == false)
            {
                CharacterJointSave(limb, info);
            }
        }

        private void ForCapsuleCollider(RagdollLimb limb, RagdollInfo info)
        {
            var col = limb.AddComponent<CapsuleCollider>();
            col.center = info.colliderCenter;
            col.radius = info.colliderRadius;
            col.height = info.colliderHeight;
            col.direction = info.capsuleColliderDirection;

            SetRigidbody(limb, info);
            CharacterJointSave(limb, info);
        }

        private void ForSphereCollider(RagdollLimb limb, RagdollInfo info)
        {
            var col = limb.AddComponent<SphereCollider>();
            col.center = info.colliderCenter;
            col.radius = info.colliderRadius;

            SetRigidbody(limb, info);
            CharacterJointSave(limb, info);
        }

        private void SetRigidbody(RagdollLimb limb, RagdollInfo info)
        {
            var rb = limb.AddComponent<Rigidbody>();
            rb.mass = info.rigidbodyMass;
        }

        private void CharacterJointSave(RagdollLimb limb, RagdollInfo info)
        {
            var characterJoint = limb.AddComponent<CharacterJoint>();
            characterJoint.connectedBody = ConvertToRigidbody(info.connectedLimbType);
            characterJoint.anchor = info.anchor;
            characterJoint.axis = info.axis;
            characterJoint.connectedAnchor = info.connectedAnchor;
            characterJoint.swingAxis = info.swingAxis;
        }

        private Rigidbody ConvertToRigidbody(LimbTypes type)
        {
            for (int i = 0; i < _limbCount; i++)
            {
                if (type == _limbs[i].limbType)
                    return _limbs[i].GetComponent<Rigidbody>();
            }

            Debug.Log($"{type} Uzuv eksik");
            return null;
        }

        public void GetHitVelocity(Vector3 hitVelocity)
        {
            for (int i = 0; i < _limbCount; i++)
            {
                if (LimbTypes.PELVIS == _limbs[i].limbType)
                {
                    _limbs[i].GetComponent<Rigidbody>().AddForce((hitVelocity + new Vector3(0, 5, 0)) * 100, ForceMode.VelocityChange);
                }
                //else // .ok gerekli deðil gibi
                //{
                //    _limbs[i].GetComponent<Rigidbody>().AddForce((hitVelocity + new Vector3(0, 3, 0)) * 50, ForceMode.VelocityChange);
                //}
            }
        }

        #endregion
    }
}