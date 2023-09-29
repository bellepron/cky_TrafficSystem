using UnityEngine;

namespace cky.Ragdoll
{
    [CreateAssetMenu(fileName = "Ragdoll", menuName = "Ragdoll/New Ragdoll Settings")]
    public class RagdollSettings : ScriptableObject
    {
        public RagdollInfo[] infos;
    }
}