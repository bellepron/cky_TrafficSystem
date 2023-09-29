using UnityEngine;

namespace cky.Ragdoll
{
    public class RagdollSaver : MonoBehaviour
    {
        [field: SerializeField] public RagdollSettings Settings { get; set; }

        private void Start()
        {
            Save();
        }

        #region Save

        private void Save()
        {
            var limbs = GetComponentsInChildren<RagdollLimb>();
            var limbCount = limbs.Length;

            Settings.infos = new RagdollInfo[limbCount];

            for (int i = 0; i < limbCount; i++)
            {
                var currentLimb = limbs[i];
                var type = currentLimb.limbType;
                Settings.infos[i] = new RagdollInfo();
                var info = Settings.infos[i];

                switch (type)
                {
                    case LimbTypes.PELVIS:
                        ForBoxCollider(type, currentLimb, info, true);
                        break;
                    case LimbTypes.MIDDLESPINE:
                        ForBoxCollider(type, currentLimb, info);
                        break;
                    case LimbTypes.HEAD:
                        ForSphereCollider(type, currentLimb, info);
                        break;

                    default:
                        ForCapsuleCollider(type, currentLimb, info);
                        break;
                }
            }

            Debug.Log($"{transform.name} ragdoll settings saved successfully!");
        }

        private void ForBoxCollider(LimbTypes type, RagdollLimb limb, RagdollInfo info, bool isPelvis = false)
        {
            info.LimbType = type;

            var col = limb.GetComponent<BoxCollider>();
            info.colliderCenter = col.center;
            info.colliderSize = col.size;

            SaveRigidbody(limb, info);

            if (isPelvis == false)
            {
                SaveCharacterJoint(limb, info);
            }
        }

        private void ForCapsuleCollider(LimbTypes type, RagdollLimb limb, RagdollInfo info)
        {
            info.LimbType = type;

            var col = limb.GetComponent<CapsuleCollider>();
            info.colliderCenter = col.center;
            info.colliderRadius = col.radius;
            info.colliderHeight = col.height;
            info.capsuleColliderDirection = col.direction;

            SaveRigidbody(limb, info);
            SaveCharacterJoint(limb, info);
        }

        private void ForSphereCollider(LimbTypes type, RagdollLimb limb, RagdollInfo info)
        {
            info.LimbType = type;

            var col = limb.GetComponent<SphereCollider>();
            info.colliderCenter = col.center;
            info.colliderRadius = col.radius;

            SaveRigidbody(limb, info);
            SaveCharacterJoint(limb, info);
        }

        private void SaveRigidbody(RagdollLimb limb, RagdollInfo info)
        {
            var rb = limb.GetComponent<Rigidbody>();
            info.rigidbodyMass = rb.mass;
        }

        private void SaveCharacterJoint(RagdollLimb limb, RagdollInfo info)
        {
            var characterJoint = limb.GetComponent<CharacterJoint>();
            info.connectedLimbType = characterJoint.connectedBody.GetComponent<RagdollLimb>().limbType;
            info.anchor = characterJoint.anchor;
            info.axis = characterJoint.axis;
            info.connectedAnchor = characterJoint.connectedAnchor;
            info.swingAxis = characterJoint.swingAxis;
        }

        #endregion
    }
}