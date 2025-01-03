using UnityEngine;

namespace cky.MatrixCreation
{
    public interface IMatrixItem
    {
        MatrixItemTypes MatrixItemType { get; }
        Transform Transform { get; }
        void ReSpawn();
    }
}