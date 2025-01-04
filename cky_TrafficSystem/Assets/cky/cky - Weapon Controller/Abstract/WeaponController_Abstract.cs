//using System.Linq;
//using UnityEngine;

//public class WeaponController_Abstract : MonoBehaviour
//{
//    [SerializeField] HitType hitType = HitType.BASEBALLBAT;
//    [SerializeField] WeaponData_SO weaponData_SO;
//    [SerializeField] Animator animator;
//    [SerializeField] WeaponCollider_Abstract weaponCollider;

//    WeaponData _currentWeaponData;

//    public bool IsAttacking;
//    public bool IsDamaging;

//    private void Awake()
//    {
//        if (weaponData_SO == null)
//        {
//            Debug.LogError("WeaponData_SO is null.");

//            return;
//        }
//        else
//        {
//            _currentWeaponData = weaponData_SO.WeaponDatas.SingleOrDefault(v => v.hitType == hitType);

//            if (_currentWeaponData == null)
//            {
//                Debug.LogError($"WeaponDatas does not contain a weapon with hitType '{hitType}', or multiple entries were found.");

//                return;
//            }
//        }

//        if (animator == null)
//        {
//            Debug.LogError("animator is null.");
//        }

//        if (weaponCollider == null)
//        {
//            Debug.LogError("weaponCollider is null.");
//        }

//        weaponCollider.Initialize(this, _currentWeaponData, animator);
//    }

//    private void OnEnable()
//    {
//        if (animator.enabled) animator.Rebind();
//    }

//    private void OnDisable()
//    {
//        IsAttacking = false;
//        IsDamaging = false;
//        if (animator.enabled) animator.Rebind();
//    }

//    public void OnMouseClick()
//    {
//        if (!IsAttacking)
//        {
//            animator.ResetTrigger(AnimatorHelper.trigger_Attack);
//            animator.SetTrigger(AnimatorHelper.trigger_Attack);

//            IsAttacking = true;
//        }
//    }

//    //private void Update()
//    //{
//    //    if (Input.GetMouseButtonDown(0) && !IsAttacking)
//    //    {
//    //        animator.ResetTrigger(AnimatorHelper.trigger_Attack);
//    //        animator.SetTrigger(AnimatorHelper.trigger_Attack);

//    //        IsAttacking = true;
//    //    }
//    //}

//    public void Start_Damaging()
//    {
//        IsDamaging = true;
//        weaponCollider.Start_AttackDuration();
//    }

//    public void Stop_Damaging()
//    {
//        IsDamaging = false;
//    }

//    public void Interrupt_Damaging()
//    {
//        IsAttacking = false;
//        IsDamaging = false;
//        animator.SetTrigger(AnimatorHelper.trigger_EndAttack);
//    }

//    public void End_Attacking()
//    {
//        IsAttacking = false;
//        animator.SetTrigger(AnimatorHelper.trigger_EndAttack);
//    }
//}