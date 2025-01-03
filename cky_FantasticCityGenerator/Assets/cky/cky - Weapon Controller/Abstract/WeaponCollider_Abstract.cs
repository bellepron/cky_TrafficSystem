//using static MLSpace.BallProjectile;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using MLSpace;

//public class WeaponCollider_Abstract : MonoBehaviour
//{
//    WeaponController_Abstract _weaponController;
//    WeaponData _weaponData;
//    Animator _animator;

//    [SerializeField] Transform[] onTrs;

//    List<GameObject> affectedObjects = new List<GameObject>();

//    Vector3[] _prevPositions;
//    Vector3[] _motionVectors;
//    int length_Trs;



//    public void Initialize(WeaponController_Abstract weaponController, WeaponData weaponData, Animator animator)
//    {
//        _weaponController = weaponController;
//        _weaponData = weaponData;
//        _animator = animator;
//    }

//    private void Start()
//    {
//        length_Trs = onTrs.Length;
//        _prevPositions = new Vector3[length_Trs];
//        _motionVectors = new Vector3[length_Trs];
//    }

//    public void Start_AttackDuration()
//    {
//        affectedObjects.Clear();
//    }

//    private void FixedUpdate()
//    {
//        if (_weaponController.IsAttacking)
//        {
//            Update___PreviousPositions_and_MotionVectors();
//        }
//        else
//        {
//            MakeThemZero();
//        }
//    }
//    void MakeThemZero()
//    {
//        for (int i = 0; i < length_Trs; i++)
//        {
//            _prevPositions[i] = Vector3.zero;
//            _motionVectors[i] = Vector3.zero;
//        }
//    }

//    private HitInformation m_HitInfo = new HitInformation();
//    private void OnTriggerEnter(Collider other)
//    {
//        if (!_weaponController.IsDamaging) return;

//        if (other.TryGetComponent<BodyColliderScript>(out var bodyColliderScript))
//        {
//            if (bodyColliderScript.ParentObject != null)
//            {
//                if (!affectedObjects.Contains(bodyColliderScript.ParentObject))
//                {
//                    affectedObjects.Add(bodyColliderScript.ParentObject);

//                    var closestPointIndex = Get_ClosestPointIndex(other.transform.position);
//                    var closestPointOnWeapon = onTrs[closestPointIndex].position;
//                    var direction = _motionVectors[closestPointIndex].normalized;
//                    var length = _motionVectors[closestPointIndex].magnitude;

//                    bodyColliderScript.Customer_StateMachine.TakeDamage(_weaponData.hitType, bodyColliderScript.bodyPart, closestPointOnWeapon, direction, _weaponData.damage);

//                    Ray ray = new Ray(closestPointOnWeapon, direction);
//                    RaycastHit[] hits = Physics.SphereCastAll(ray, _weaponData.hitRadius, length, _weaponData.layerMask);
//                    List<int> chosenHits = new List<int>();
//                    RagdollManager ragMan = null;
//                    RaycastHit? rayhit = null;

//                    var hitStrength = _weaponData.hitStrength;
//                    for (int i = 0; i < hits.Length; i++)
//                    {
//                        RaycastHit rhit = hits[i];
//                        BodyColliderScript bcs = rhit.collider.GetComponent<BodyColliderScript>();
//                        if (!bcs) continue;
//                        if (!bcs.ParentObject) continue;

//                        if (!ragMan)
//                        {
//                            ragMan = bcs.ParentRagdollManager;
//                            m_HitInfo.hitObject = bcs.ParentObject;
//                            m_HitInfo.collider = rhit.collider;
//                            m_HitInfo.hitDirection = direction;
//                            m_HitInfo.hitStrength = hitStrength;
//                            rayhit = rhit;
//                        }

//                        chosenHits.Add(bcs.index);
//                    }

//                    if (hits.Length > 0)
//                    {
//                        if (ragMan)
//                        {
//                            if (!rayhit.HasValue) return;

//                            m_HitInfo.bodyPartIndices = chosenHits.ToArray();

//                            ragMan.StartHitReaction(m_HitInfo.bodyPartIndices, direction * _weaponData.hitStrength);
//                        }
//                    }

//                    _animator.CrossFade(AnimatorHelper.state_HardHitReaction, 0.1f);
//                    _weaponController.Interrupt_Damaging();

//                    EventBus.OnHit_Baseballbat_EventTrigger(other.transform.position, Vector3.zero);
//                }
//            }
//        }
//        else
//        {
//            Debug.Log($"Trigger Enter: {other.name}");

//            //if (other.TryGetComponent<Rigidbody>(out var rb))
//            //{
//            //    var closestPointIndex = 4;
//            //    var closestPointOnWeapon = onTrs[closestPointIndex].position;
//            //    var motionvector = _motionVectors[closestPointIndex];

//            //    rb.AddForce(motionvector, ForceMode.Impulse);
//            //}

//            _animator.CrossFade(AnimatorHelper.state_HardHitReaction, 0.05f);
//            _weaponController.Interrupt_Damaging();

//            EventBus.OnHit_Baseballbat_EventTrigger(other.transform.position, Vector3.zero);
//        }
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (!_weaponController.IsDamaging) return;

//        //Debug.Log($"Collision Enter: {collision.transform.name}");

//        if (collision.transform.TryGetComponent<Rigidbody>(out var rb))
//        {
//            var closestPointIndex = Get_ClosestPointIndex(collision.contacts[0].point);
//            var closestPointOnWeapon = onTrs[closestPointIndex].position;
//            var motionvector = _motionVectors[closestPointIndex];

//            rb.AddForce(motionvector * 25, ForceMode.Impulse);
//        }

//        _animator.CrossFade(AnimatorHelper.state_HardHitReaction, 0.05f);
//        _weaponController.Interrupt_Damaging();

//        EventBus.OnHit_Baseballbat_EventTrigger(collision.transform.position, Vector3.zero);
//    }

//    void Update___PreviousPositions_and_MotionVectors()
//    {
//        for (int i = 0; i < length_Trs; i++)
//        {
//            _motionVectors[i] = onTrs[i].position - _prevPositions[i];

//            _prevPositions[i] = onTrs[i].position;
//        }
//    }

//    int Get_ClosestPointIndex(Vector3 hitPos)
//    {
//        int closestIndex = onTrs
//            .Select((tr, index) => new { Transform = tr, Index = index })   // Transform ve Index'leri bir arada tut
//            .OrderBy(item => Vector3.Distance(item.Transform.position, hitPos)) // Mesafeye göre sýrala
//            .First().Index; // En yakýn olanýn Index'ini al

//        return closestIndex;
//    }
//}