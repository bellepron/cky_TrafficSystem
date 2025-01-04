using UnityEditor;
using UnityEngine;

namespace cky.Changer.FloorChange
{
    public class FloorCreator : MonoBehaviour
    {
        //[SerializeField] FloorData floorData;
        //[SerializeField] FloorPart[] parts;
        //[SerializeField] GameObject floorPrefab;
        //[SerializeField] FloorTypes createFloorType = FloorTypes.Closed;

        //[SerializeField]
        //private int xDivide = 1;
        //[SerializeField]
        //private int yDivide = 1;
        //[SerializeField]
        //private int zDivide = 1;

        //public void CreateFloor()
        //{
        //    floorData.floorTypeDatas = new FloorTypeData[xDivide * yDivide * zDivide];
        //    parts = new FloorPart[xDivide * yDivide * zDivide];
        //    floorData.Save();

        //    ClearChildren();

        //    Vector3 originalScale = transform.lossyScale;
        //    Vector3 prefabScale = new Vector3(originalScale.x / xDivide, originalScale.y / yDivide, originalScale.z / zDivide);
        //    Vector3 startPosition = transform.position - originalScale / 2 + prefabScale / 2;

        //    var q = 0;
        //    for (int i = 0; i < xDivide; i++)
        //    {
        //        for (int j = 0; j < yDivide; j++)
        //        {
        //            for (int k = 0; k < zDivide; k++)
        //            {
        //                Vector3 position = startPosition + new Vector3(i * prefabScale.x, j * prefabScale.y, k * prefabScale.z);
        //                var floorPart = Instantiate(floorPrefab, position, Quaternion.identity).GetComponent<FloorPart>();
        //                floorPart.transform.localScale = prefabScale;
        //                floorPart.transform.parent = transform;

        //                floorData.floorTypeDatas[q] = new FloorTypeData(FloorTypes.Closed);
        //                parts[q] = floorPart.GetComponent<FloorPart>();
        //                floorPart.Initialize(floorData, q, createFloorType);

        //                q++;
        //            }
        //        }
        //    }

        //    Save();

        //    EditorUtility.SetDirty(this);
        //}

        //private void Awake()
        //{
        //    floorData.Load();

        //    var length = parts.Length;
        //    for (int i = 0; i < length; i++)
        //    {
        //        parts[i].Initialize(floorData, i, floorData.floorTypeDatas[i].floorType);
        //    }
        //}

        //private void ClearChildren()
        //{
        //    for (int i = transform.childCount - 1; i >= 0; i--)
        //    {
        //        DestroyImmediate(transform.GetChild(i).gameObject);
        //    }
        //}

        //public void Save()
        //{
        //    floorData.Save();
        //}
    }
}