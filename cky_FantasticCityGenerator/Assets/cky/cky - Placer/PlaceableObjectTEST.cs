using cky.Placer;
using UnityEngine;

public struct RendMats
{
    public Material[] mats;

    public RendMats(Material[] mats)
    {
        this.mats = mats;
    }
}

public class PlaceableObjectTEST : MonoBehaviour, IPlaceable
{
    Renderer[] renderers;
    RendMats[] rendMats;
    Collider[] childColliders;

    public Transform Transform => transform;
    public bool IsHighligted { get; private set; }
    [field: SerializeField] public bool IsHolding { get; private set; }

    [Space(15)]
    [Header("Collider")]
    [SerializeField] Color gizmoColor = Color.blue;
    [SerializeField] Vector3 colliderCenterOffset = Vector3.zero;
    [SerializeField] Vector3 colliderBoxSize = Vector3.one;
    Collider[] colliderHitResults = new Collider[1];



    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        var length = renderers.Length;
        rendMats = new RendMats[length];
        for (int i = 0; i < length; i++)
        {
            rendMats[i] = new RendMats(renderers[i].materials);
        }

        childColliders = GetComponentsInChildren<Collider>();
    }


    public void Highlight(bool b, PlacerData placerData = null)
    {
        if (b)
        {
            if (!IsHighligted)
            {
                IsHighligted = true;
                ChangeAllMaterials(placerData.highlightMat);
            }
        }
        else
        {
            if (IsHighligted)
            {
                IsHighligted = false;
                ChangeMaterialsDefault();
            }
        }
    }

    public void Holding(PlacerData placerData)
    {
        IsHolding = true;
        ChangeAllMaterials(placerData.holdingMatPlaceable);
        Open_ChildColliders(false);
    }

    public void Place(Vector3 pos, Quaternion rot)
    {
        IsHolding = false;
        ChangeMaterialsDefault();
        Open_ChildColliders(true);
    }

    void Open_ChildColliders(bool b)
    {
        foreach (var childCollider in childColliders)
        {
            childCollider.enabled = b;
        }
    }

    public bool IsPlaceable(LayerMask obstacleMask, PlacerData placerData)
    {
        int numColliders = Physics.OverlapBoxNonAlloc(transform.position + colliderCenterOffset, colliderBoxSize / 2, colliderHitResults, Quaternion.identity, obstacleMask);

        if (numColliders == 0)
        {
            ChangeAllMaterials(placerData.holdingMatPlaceable);
            return true;
        }
        else
        {
            ChangeAllMaterials(placerData.holdingMatCantPlaceable);
            return false;
        }
    }



    void ChangeAllMaterials(Material mat)
    {
        var length = renderers.Length;
        for (int i = 0; i < length; i++)
        {
            var matCount = rendMats[i].mats.Length;
            Material[] mats = new Material[matCount];
            for (int j = 0; j < matCount; j++) mats[j] = mat;
            renderers[i].materials = mats;
        }
    }

    void ChangeMaterialsDefault()
    {
        var length = renderers.Length;
        for (int i = 0; i < length; i++)
        {
            var matCount = rendMats[i].mats.Length;
            Material[] mats = new Material[matCount];
            for (int j = 0; j < matCount; j++) mats[j] = rendMats[i].mats[j];
            renderers[i].materials = mats;
        }
    }



    #region Gizmos

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position + colliderCenterOffset, colliderBoxSize);
    }

    #endregion
}