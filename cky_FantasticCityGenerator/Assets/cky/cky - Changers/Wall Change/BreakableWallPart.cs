//using UnityEngine;

//public class BreakableWallPart : MonoBehaviour
//{
//    private BreakableWall breakableWall;
//    public BreakableWall BreakableWall
//    {
//        get
//        {
//            if (breakableWall == null)
//            {
//                breakableWall = GetComponentInParent<BreakableWall>();
//            }
//            return breakableWall;
//        }
//        private set
//        {
//            breakableWall = value;
//        }
//    }

//    public Rigidbody rb;

//    Renderer _renderer;
//    Material _material;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//        _renderer = rb.GetComponent<Renderer>();
//        _material = _renderer.material;
//    }

//    public void WhenBreakableWallHited(Material brokenWallTransparentMaterial)
//    {
//        rb.isKinematic = false;

//        _renderer.material = brokenWallTransparentMaterial;
//        _material = _renderer.material;
//    }

//    public void ChangeMaterialColor(Color newColor)
//    {
//        _material.SetColor("_BaseColor", newColor);
//    }
//}