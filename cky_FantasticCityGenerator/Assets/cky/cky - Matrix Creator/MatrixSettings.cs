using System.Linq;
using UnityEditor;
using UnityEngine;

namespace cky.MatrixCreation
{
    [CreateAssetMenu(menuName = "cky/Matrix Creation/New Matrix Settings", fileName = "New Matrix Settings")]
    public class MatrixSettings : ScriptableObject
    {
        public int Dimension_I;
        public int Dimension_J;

        public MatrixData_Type[] matrixData_Type;

        public void Set(MatrixCreatorManager manager, int I, int J)
        {
            Dimension_I = I;
            Dimension_J = J;

            matrixData_Type = new MatrixData_Type[manager.matrixItemDatasLength];

            IMatrixItem[] allMatrixItems = (IMatrixItem[])FindObjectsByType(typeof(MatrixItem), FindObjectsSortMode.None);
            Debug.Log($"MatrixItem - All count: {allMatrixItems.Length}");

            manager.matrixItemTypes = new MatrixItemTypes[manager.matrixItemDatasLength];
            for (int i = 0; i < manager.matrixItemDatasLength; i++)
            {
                manager.matrixItemTypes[i] = manager.matrixItemDatas[i].ItemPrefab.GetComponent<IMatrixItem>().MatrixItemType;
            }


            for (int i = 0; i < manager.matrixItemDatasLength; i++)
            {
                var matrixItemType = manager.matrixItemDatas[i].ItemPrefab.GetComponent<IMatrixItem>().MatrixItemType;
                var _items = allMatrixItems.Where(i => i.MatrixItemType == matrixItemType).Select(i => i.Transform).ToArray();
                var itemCount = _items.Length;
                Debug.Log($"MatrixItemType - {matrixItemType} count: {itemCount}");

                matrixData_Type[i] = new MatrixData_Type(matrixItemType);
                var itemIndexesAndTransforms = matrixData_Type[i].matrixItemIndexesAndTransforms;

                foreach (var item in _items)
                {
                    var itemTransform = item.transform;
                    var indices = manager.MatrixCreator.Find_XZ_WithPosition(itemTransform.position);

                    if (indices.I >= 0 && indices.I < Dimension_I && indices.J >= 0 && indices.J < Dimension_J)
                    {
                        itemIndexesAndTransforms.Add(new MatrixItemIndexesAndTransform(indices.I, indices.J, itemTransform.position, itemTransform.rotation, itemTransform.localScale));
                    }
                }
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}