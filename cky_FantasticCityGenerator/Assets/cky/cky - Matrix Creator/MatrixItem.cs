using UnityEngine;

namespace cky.MatrixCreation
{
    public class MatrixItem : MonoBehaviour, IMatrixItem
    {
        [field: SerializeField] public MatrixItemTypes MatrixItemType { get; private set; }
        public Transform Transform => transform;

        public void ReSpawn()
        {
            Debug.Log($"Matrix Item Spawned: {transform.name}");
        }
    }
}