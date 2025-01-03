//using DG.Tweening;
//using UnityEngine;

//public class CKY_MaterialFaderHDRP : MonoBehaviour
//{
//    [SerializeField] private Renderer[] renderers; // Birden fazla Renderer alabiliriz

//    private Material[][] materials; // Her renderer için ayrı bir materyal dizisi tutarız

//    private void Start()
//    {
//        if (renderers != null && renderers.Length > 0)
//        {
//            materials = new Material[renderers.Length][];

//            for (int i = 0; i < renderers.Length; i++)
//            {
//                if (renderers[i] != null)
//                {
//                    // Her renderer'ın materyallerini kopyala
//                    materials[i] = renderers[i].materials;
//                }
//            }
//        }
//    }

//    private void Update()
//    {
//        if (materials == null || materials.Length == 0)
//            return;

//        if (Input.GetKeyDown(KeyCode.Alpha7)) // 7 tuşu
//        {
//            SetSurfaceType("Transparent");
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha8)) // 8 tuşu
//        {
//            SetSurfaceType("Opaque");
//        }
//    }

//    private void SetSurfaceType(string surfaceType)
//    {
//        for (int i = 0; i < materials.Length; i++)
//        {
//            if (materials[i] == null)
//                continue;

//            foreach (var material in materials[i])
//            {
//                if (material.shader.name != "HDRP/Lit")
//                {
//                    Debug.LogWarning($"Renderer {renderers[i].name}: Materyal HDRP/Lit shader kullanmıyor.");
//                    continue;
//                }

//                if (surfaceType == "Transparent")
//                {
//                    material.SetFloat("_SurfaceType", 1);
//                    material.SetFloat("_BlendMode", 0);
//                    material.SetFloat("_AlphaCutoffEnable", 0);
//                    material.SetFloat("_ZWrite", 0);
//                    material.SetFloat("_CullMode", 0); // Çift yönlü render
//                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

//                    material.DOFade(0.1f, 1);
//                }
//                else if (surfaceType == "Opaque")
//                {
//                    material.SetFloat("_SurfaceType", 0);
//                    material.SetFloat("_BlendMode", 0);
//                    material.SetFloat("_AlphaCutoffEnable", 0);
//                    material.SetFloat("_ZWrite", 1);
//                    material.SetFloat("_CullMode", 2); // Sadece ön yüzeyi render
//                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
//                }

//                // Shader yenile
//                material.shader = material.shader;
//            }

//            // Güncellenen materyalleri renderere geri ata
//            renderers[i].materials = materials[i];
//        }

//        Debug.Log($"Materials set to {surfaceType}");
//    }
//}