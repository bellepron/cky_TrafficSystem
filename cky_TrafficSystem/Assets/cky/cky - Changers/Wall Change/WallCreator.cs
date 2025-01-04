using UnityEditor;
using UnityEngine;

namespace cky.Changer.WallChange
{
    public class WallCreator : MonoBehaviour
    {
        //[SerializeField] WallData wallData;
        //[SerializeField] WallPart[] parts;
        //[SerializeField] GameObject wallPrefab;
        //[SerializeField] WallTypes createWallType = WallTypes.Closed;

        //[SerializeField]
        //private int xDivide = 1;
        //[SerializeField]
        //private int yDivide = 1;
        //[SerializeField]
        //private int zDivide = 1;

        //public void CreateWall()
        //{
        //    wallData.wallTypeDatas = new WallTypeData[xDivide * yDivide * zDivide];
        //    parts = new WallPart[xDivide * yDivide * zDivide];
        //    wallData.Save();

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
        //                var wallPart = Instantiate(wallPrefab, position, transform.rotation).GetComponent<WallPart>();
        //                wallPart.transform.localScale = prefabScale;
        //                wallPart.transform.parent = transform;

        //                wallData.wallTypeDatas[q] = new WallTypeData(WallTypes.Closed);
        //                parts[q] = wallPart.GetComponent<WallPart>();
        //                wallPart.Initialize(wallData, q, createWallType);

        //                q++;
        //            }
        //        }
        //    }

        //    Save();

        //    EditorUtility.SetDirty(this);
        //}

        //private void Awake()
        //{
        //    wallData.Load();

        //    var length = parts.Length;
        //    for (int i = 0; i < length; i++)
        //    {
        //        parts[i].Initialize(wallData, i, wallData.wallTypeDatas[i].wallType);
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
        //    wallData.Save();
        //}
    }
}