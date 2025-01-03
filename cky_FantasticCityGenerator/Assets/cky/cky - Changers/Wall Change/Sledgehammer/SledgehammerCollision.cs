//using System.Collections.Generic;
//using UnityEngine;

//public class SledgehammerCollision : MonoBehaviour
//{
//    [SerializeField] SledgehammerController controller;
//    [SerializeField] Animator animator;

//    [SerializeField] Transform[] hitTrs;
//    [SerializeField] float radius = 0.2f;
//    [SerializeField] LayerMask layerMask;
//    [SerializeField] float power = 25.0f;
//    [SerializeField] Material brokenWallTransparentMaterial;
//    ForceMode forceMode = ForceMode.Impulse;

//    Collider[] _targetsInRadius;

//    List<Transform> affectedObjects = new List<Transform>();

//    public bool isActive;

//    public void Start_AttackDuration()
//    {
//        isActive = true;

//        affectedObjects.Clear();
//    }

//    public void End_AttackDuration()
//    {
//        isActive = false;
//    }

//    private void FixedUpdate()
//    {
//        if (isActive)
//        {
//            Hit();
//        }
//    }

//    private void Hit()
//    {
//        foreach (var hitTr in hitTrs)
//        {
//            var hitPos = hitTr.position;
//            _targetsInRadius = Physics.OverlapSphere(hitPos, radius, layerMask);
//            foreach (Collider c in _targetsInRadius)
//            {
//                if (c.TryGetComponent<BreakableWallPart>(out var bwp))
//                {
//                    var breakableWall = bwp.BreakableWall;
//                    var hitedWallTransform = breakableWall.transform;

//                    if (!affectedObjects.Contains(hitedWallTransform))
//                    {
//                        affectedObjects.Add(hitedWallTransform);

//                        breakableWall.Hited_WithSledgehammer(hitPos, 3, forceMode, brokenWallTransparentMaterial);

//                        animator.CrossFade(AnimatorHelper.state_Idle, 0.05f);
//                        controller.StopAttack();
//                        End_AttackDuration();

//                        EventBus.OnHit_Sledgehammer_EventTrigger(hitPos, (c.transform.position - hitPos).normalized);
//                    }

//                    return;
//                }
//            }
//        }
//    }

//    private Vector3 CalculateHitPower(Vector3 strikingObjectPos, Vector3 hitedObjectPos)
//    {
//        strikingObjectPos.y = 0f;
//        hitedObjectPos.y = 0f;
//        var dir = hitedObjectPos - strikingObjectPos;
//        dir.Normalize();

//        return dir * power;
//    }



//    void OnDrawGizmos()
//    {
//        Gizmos.color = Color.blue;

//        foreach (var hitTr in hitTrs)
//            Gizmos.DrawWireSphere(hitTr.position, radius);
//    }
//}