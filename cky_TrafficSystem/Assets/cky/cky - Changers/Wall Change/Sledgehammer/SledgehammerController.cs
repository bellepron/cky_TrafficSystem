//using System.Collections;
//using UnityEngine;

//public class SledgehammerController : MonoBehaviour
//{
//    [SerializeField] Animator animator;
//    [SerializeField] SledgehammerCollision sledgehammerCollision;

//    private Coroutine attackCoroutine;

//    public bool IsAttacking;

//    private void OnEnable()
//    {
//        if (animator.enabled) animator.Rebind();
//    }

//    private void OnDisable()
//    {
//        IsAttacking = false;
//        attackCoroutine = null;
//        if (animator.enabled) animator.Rebind();
//    }

//    public void OnMouseClick()
//    {
//        Debug.Log("SlegdeHammer click");

//        if (!IsAttacking)
//        {
//            animator.SetTrigger(AnimatorHelper.trigger_Attack);

//            attackCoroutine = StartCoroutine(Attack_Cooldown());
//        }
//    }

//    IEnumerator Attack_Cooldown()
//    {
//        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorHelper.tag_Attack));
//        IsAttacking = true;
//        sledgehammerCollision.Start_AttackDuration();
//        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsTag(AnimatorHelper.tag_Attack));

//        IsAttacking = false;
//        sledgehammerCollision.End_AttackDuration();
//        attackCoroutine = null;
//    }

//    public void StopAttack()
//    {
//        StopCoroutine(attackCoroutine);

//        IsAttacking = false;
//        attackCoroutine = null;
//    }
//}