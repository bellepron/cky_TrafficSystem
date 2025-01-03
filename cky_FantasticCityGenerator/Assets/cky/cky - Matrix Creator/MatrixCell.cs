using System.Collections.Generic;
using CKY_Pooling;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace cky.MatrixCreation
{
    [System.Serializable]
    public class ItemTransforms
    {
        public List<Transform> items = new List<Transform>();
    }

    public class MatrixCell : MonoBehaviour
    {
        [Space(5)]
        public MatrixCreatorManager Manager;

        [Space(5)]
        public int I;
        public int J;
        public List<MatrixCell> neighbours = new List<MatrixCell>();

        [Space(5)]
        [SerializeField] MatrixData_Type[] matrixData_Types;

        [Space(5)]
        [SerializeField] List<Transform> activeObjects = new List<Transform>();

        bool _active;

        public void Init(MatrixCreatorManager manager, int I, int J)
        {
            Manager = manager;
            this.I = I;
            this.J = J;
        }

        public void Init2()
        {
            matrixData_Types = new MatrixData_Type[Manager.matrixItemDatasLength];
            for (int i = 0; i < Manager.matrixItemDatasLength; i++)
            {
                matrixData_Types[i] = new MatrixData_Type(Manager.matrixItemTypes[i]);
                matrixData_Types[i].matrixItemIndexesAndTransforms = Manager.MatrixSettings.matrixData_Type[i].matrixItemIndexesAndTransforms.Where(n => n.I == I && n.J == J).ToList();

                Debug.Log(matrixData_Types[i].matrixItemIndexesAndTransforms.Count);
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

        public void Open()
        {
            _active = true;
            //Debug.Log($"Opened - {I}:{J}");

            var length = matrixData_Types.Length;

            for (int i = 0; i < length; i++)
            {
                var itemPrefab = Manager.matrixItemDatas[i].ItemPrefab;
                var elements = matrixData_Types[i].matrixItemIndexesAndTransforms;

                foreach (var element in elements)
                {
                    var obj = CKY_PoolManager.Spawn(itemPrefab, element.position, element.rotation);
                    if (Manager.matrixItemDatas[i].UseScale) obj.localScale = element.scale;

                    if (obj.TryGetComponent<IMatrixItem>(out var iMatrixItem))
                    {
                        iMatrixItem.ReSpawn();
                    }

                    activeObjects.Add(obj);
                }
            }
        }

        public void Close()
        {
            _active = false;
            //Debug.Log($"Closed - {I}:{J}");

            foreach (var obj in activeObjects)
            {
                CKY_PoolManager.Despawn(obj);
            }

            activeObjects.Clear();
        }



        #region Gizmos

        private void OnDrawGizmos()
        {
            Gizmos.color = _active ? Manager.colorCell_Open : Manager.colorCell_Close;
            Gizmos.DrawWireCube(transform.position, new Vector3(transform.localScale.x, 0f, transform.localScale.z));
        }

        #endregion

    }
}