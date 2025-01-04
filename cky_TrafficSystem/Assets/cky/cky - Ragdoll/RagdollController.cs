//using CKY.ActiveRagdoll;
using UnityEngine;

namespace cky.Ragdoll
{
    //[RequireComponent(typeof(RagdollSetter))]
    //public class RagdollController : MonoBehaviour
    //{
    //    private RagdollSetter RagdollSetter;

    //    private Animator Animator;
    //    private Rigidbody Rigidbody;
    //    private CapsuleCollider CapsuleCollider;
    //    [SerializeField] private GameObject CheckingBox;

    //    private void Awake()
    //    {
    //        RagdollSetter = GetComponent<RagdollSetter>();
    //        Animator = GetComponent<Animator>();
    //        Rigidbody = GetComponent<Rigidbody>();
    //        CapsuleCollider = GetComponent<CapsuleCollider>();
    //    }

    //    public void EditorSet()
    //    {
    //        GetComponent<RagdollSetter>().Set();
    //    }
    //    public void EditorDelete()
    //    {
    //        GetComponent<RagdollSetter>().Delete();
    //    }

    //    //private void Update()
    //    //{
    //    //    if (Input.GetKeyUp(KeyCode.K))
    //    //    {
    //    //        Die();
    //    //    }
    //    //}

    //    public void Die()
    //    {
    //        Animator.enabled = false;
    //        Destroy(Rigidbody);
    //        Destroy(CapsuleCollider);
    //        Destroy(CheckingBox);

    //        RagdollSetter.Set();
    //    }


    //    public void GetHitVelocity(Vector3 hitVelocity)
    //    {
    //        RagdollSetter.GetHitVelocity(hitVelocity);
    //    }

    //}
}