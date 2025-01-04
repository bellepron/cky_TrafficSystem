using UnityEngine;

namespace cky.Ragdoll
{
    [System.Serializable]
    public class RagdollInfo
    {
        public LimbTypes LimbType;
        public Vector3 colliderCenter;
        public float colliderRadius;
        public float colliderHeight;
        public Vector3 colliderSize;
        public int capsuleColliderDirection;

        public float rigidbodyMass;

        public LimbTypes connectedLimbType;
        public Vector3 anchor;
        public Vector3 axis;
        public Vector3 connectedAnchor;
        public Vector3 swingAxis;
    }
}