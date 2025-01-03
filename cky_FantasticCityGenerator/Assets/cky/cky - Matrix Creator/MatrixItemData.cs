using System.Collections.Generic;
using UnityEngine;

namespace cky.MatrixCreation
{
    [System.Serializable]
    public class MatrixData_Type
    {
        public MatrixItemTypes Type;
        public List<MatrixItemIndexesAndTransform> matrixItemIndexesAndTransforms;

        public MatrixData_Type(MatrixItemTypes type)
        {
            Type = type;
            matrixItemIndexesAndTransforms = new List<MatrixItemIndexesAndTransform>();
        }
    }

    [System.Serializable]
    public class MatrixItemIndexesAndTransform
    {
        public int I;
        public int J;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;

        public MatrixItemIndexesAndTransform(int i, int j, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            I = i;
            J = j;
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }

    [System.Serializable]
    public class MatrixItemData
    {
        public Transform ItemPrefab;
        public bool UseScale;
        public bool WillCreate;
    }
}