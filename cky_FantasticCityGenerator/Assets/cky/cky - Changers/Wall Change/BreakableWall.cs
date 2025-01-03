//using System.Collections;
//using game.ProductBase;
//using System.Linq;
//using UnityEngine;

//public class BreakableWall : MonoBehaviour
//{
//    [SerializeField] BreakableWallPart[] parts;

//    int _index;
//    bool _isBroken;

//    [Space(15)]
//    [Header("Fade")]
//    public float fadeDuration = 2.5f;
//    Color _startColor;
//    Color _endColor;
//    private float fadeStartTime;

//    [SerializeField] float fadeStartDelay = 1.0f;
//    [SerializeField] float fadeDurationMin = 2.0f;
//    [SerializeField] float fadeDurationMax = 3.0f;


//    public void Initialize_FromStoreBuilding(int index, bool isBroken)
//    {
//        _index = index;
//        _isBroken = isBroken;

//        if (_isBroken)
//        {
//            transform.GetChild(0).gameObject.SetActive(false);

//            return;
//        }

//        _startColor = parts[0].GetComponent<Renderer>().material.GetColor("_BaseColor");
//        //_startColor = parts[0].GetComponent<Renderer>().material.color;
//        _endColor = new Color(_startColor.r, _startColor.g, _startColor.b, 0);
//    }

//    public void Hited_WithSledgehammer(Vector3 hitPos, float range, ForceMode forceMode, Material brokenWallTransparentMaterial)
//    {
//        if (_isBroken) return;

//        _isBroken = true;

//        foreach (var part in parts)
//        {
//            part.WhenBreakableWallHited(brokenWallTransparentMaterial);

//            float distance = Vector3.Distance(hitPos, part.transform.position);

//            if (distance <= range)
//            {
//                part.rb.AddExplosionForce(100f, hitPos, range, 1f, forceMode);
//            }
//        }

//        StartCoroutine(DisappearParts());

//        EventBus.OnWallBreake_EventTrigger(_index);
//    }

//    IEnumerator DisappearParts()
//    {
//        var productProvider = GameObject.FindWithTag(TagHelper.ProductProvider).GetComponent<ProductProvider>();
//        parts = parts.OrderBy(x => UnityEngine.Random.value).ToArray();

//        yield return new WaitForSeconds(fadeStartDelay);
//        fadeStartTime = Time.time;

//        int counter = 0;
//        foreach (var part in parts)
//        {
//            if (counter < 5)
//            {
//                StartCoroutine(FadeOutPart(part, productProvider));
//                counter++;
//            }
//            else
//            {
//                StartCoroutine(FadeOutPart(part));
//            }
//        }

//        yield return new WaitForSeconds(3);
//        gameObject.SetActive(false);
//    }

//    IEnumerator FadeOutPart(BreakableWallPart part, ProductProvider productProvider)
//    {
//        float randomFadeDuration = UnityEngine.Random.Range(fadeDurationMin, fadeDurationMax);

//        var elapsed = 0.0f;

//        while (elapsed < 1.0f)
//        {
//            elapsed = (Time.time - fadeStartTime) / randomFadeDuration;
//            Color currentColor = Color.Lerp(_startColor, _endColor, elapsed);
//            part.ChangeMaterialColor(currentColor);

//            yield return null;
//        }

//        productProvider.Generate_TrashBag(part.transform.position, part.transform.rotation, true);

//        part.gameObject.SetActive(false);
//    }

//    IEnumerator FadeOutPart(BreakableWallPart part)
//    {
//        float randomFadeDuration = UnityEngine.Random.Range(fadeDurationMin, fadeDurationMax);

//        var elapsed = 0.0f;

//        while (elapsed < 1.0f)
//        {
//            elapsed = (Time.time - fadeStartTime) / randomFadeDuration;
//            Color currentColor = Color.Lerp(_startColor, _endColor, elapsed);
//            part.ChangeMaterialColor(currentColor);

//            yield return null;
//        }

//        part.gameObject.SetActive(false);
//    }
//}