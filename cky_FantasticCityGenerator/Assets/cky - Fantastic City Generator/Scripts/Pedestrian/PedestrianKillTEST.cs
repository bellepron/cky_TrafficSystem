using UnityEngine;

public class PedestrianKillTEST : MonoBehaviour
{
    [SerializeField] float power = 100;
    [SerializeField] ForceMode forceMode = ForceMode.VelocityChange;
    [SerializeField] float radius = 0.5f;

    float k;

    private void Update()
    {
        if (transform.parent != null)
        {
            k = 2 / transform.parent.localScale.x;
        }
        else
        {
            k = 2;
        }

        transform.localScale = Vector3.one * radius * k;
    }

    //private void OnCollisionEnter(Collision other)
    //{
    //    if (other.transform.TryGetComponent<IHitable>(out var iHitable))
    //    {
    //        var pos = transform.position; pos.y = 0;
    //        var otherPos = other.transform.position; otherPos.y = 0;
    //        var dir = otherPos - pos; dir.Normalize();
    //        iHitable.Hited(HitType.NONE, transform.position, Vector3.zero, dir * power, forceMode);
    //    }
    //}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}