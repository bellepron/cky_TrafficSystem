using cky.MatrixCreation;
using System.Collections.Generic;
using UnityEngine;

namespace FCG.Pedestrian.Matrix
{
    public class P_SpawnPointMatrix : MonoBehaviour
    {
        [System.Serializable]
        public class MatrixIndex
        {
            public int I;
            public int J;

            public MatrixIndex(int i, int j)
            {
                I = i;
                J = j;
            }
        }

        [field: SerializeField] public P_MatrixCell PlayerCell_Previous { get; set; }
        [field: SerializeField] public P_MatrixCell PlayerCell_Current { get; set; }

        [field: SerializeField] public GameObject MatrixCellPrefab { get; set; }
        [field: SerializeField] public float MatrixCellPercent { get; set; } = 0.98f;
        [field: SerializeField] public float AreaWidth_I { get; set; } = 4000f;
        [field: SerializeField] public float AreaWidth_J { get; set; } = 10000f;
        [field: SerializeField] public int Dimension_I { get; set; } = 20;
        [field: SerializeField] public int Dimension_J { get; set; } = 20;
        [field: SerializeField] public P_MatrixCell[,] Matrix { get; set; }



        [Space(15)]
        [Header("To See")]
        [SerializeField] List<P_MatrixCell> _openCells_Previous = new List<P_MatrixCell>();
        [SerializeField] List<P_MatrixCell> _openCells_Current = new List<P_MatrixCell>();



        public void Create()
        {
            ClearMatrix();
            CreateMatrix();
            AssignCellsNeighbours();

            _openCells_Previous.Clear();
        }

        private void ClearMatrix()
        {
            var cells = transform.GetComponentsInChildren<MatrixCell>();
            foreach (var cell in cells)
            {
                DestroyImmediate(cell.gameObject);
            }
        }
        private void CreateMatrix()
        {
            Matrix = new P_MatrixCell[Dimension_I, Dimension_J];

            for (int i = 0; i < Dimension_I; i++)
            {
                for (int j = 0; j < Dimension_J; j++)
                {
                    GameObject cellGO = Instantiate(MatrixCellPrefab, transform);
                    cellGO.transform.localPosition = new Vector3(
                        -AreaWidth_J * 0.5f + AreaWidth_J * (j + 0.5f) / Dimension_J,
                        0.05f,
                        -AreaWidth_I * 0.5f + AreaWidth_I * (i + 0.5f) / Dimension_I
                    );
                    cellGO.transform.localScale = new Vector3(
                        AreaWidth_J / Dimension_J * MatrixCellPercent,
                        0.1f,
                        AreaWidth_I / Dimension_I * MatrixCellPercent
                    );
                    cellGO.name = $"Cell [{i},{j}]";
                    P_MatrixCell cell = cellGO.GetComponent<P_MatrixCell>();
                    cell.I = i;
                    cell.J = j;
                    Matrix[i, j] = cell;
                }
            }
        }

        private void AssignCellsNeighbours()
        {
            for (int i = 0; i < Dimension_I; i++)
            {
                for (int j = 0; j < Dimension_J; j++)
                {
                    P_MatrixCell currentElement = Matrix[i, j];

                    if (i < Dimension_I - 1 && j > 0) // Sol üst komþu
                    {
                        currentElement.neighbours.Add(Matrix[i + 1, j - 1]);
                    }
                    if (i < Dimension_I - 1) // Üst komþu
                    {
                        currentElement.neighbours.Add(Matrix[i + 1, j]);
                    }
                    if (i < Dimension_I - 1 && j < Dimension_J - 1) // Sað üst komþu
                    {
                        currentElement.neighbours.Add(Matrix[i + 1, j + 1]);
                    }
                    if (j < Dimension_J - 1) // Sað komþu
                    {
                        currentElement.neighbours.Add(Matrix[i, j + 1]);
                    }
                    if (i > 0 && j < Dimension_J - 1) // Sað alt komþu
                    {
                        currentElement.neighbours.Add(Matrix[i - 1, j + 1]);
                    }
                    if (i > 0) // Alt komþu
                    {
                        currentElement.neighbours.Add(Matrix[i - 1, j]);
                    }
                    if (i > 0 && j > 0) // Sol alt komþu
                    {
                        currentElement.neighbours.Add(Matrix[i - 1, j - 1]);
                    }
                    if (j > 0) // Sol komþu
                    {
                        currentElement.neighbours.Add(Matrix[i, j - 1]);
                    }
                }
            }
        }




        public void AssignItemToCell(WpDataSpawnPedestrian wpDataSpawn)
        {
            var indices = Find_XZ(wpDataSpawn.position);

            if (indices.I >= 0 && indices.I < Dimension_I && indices.J >= 0 && indices.J < Dimension_J)
            {
                Matrix[indices.I, indices.J].spawnPoints.Add(wpDataSpawn);
            }
        }
        private MatrixIndex Find_XZ(Vector3 itemPos)
        {
            Vector3 localPosition = transform.InverseTransformPoint(itemPos);
            int iIndex = Mathf.FloorToInt((localPosition.z + AreaWidth_I * 0.5f) / AreaWidth_I * Dimension_I);
            int jIndex = Mathf.FloorToInt((localPosition.x + AreaWidth_J * 0.5f) / AreaWidth_J * Dimension_J);

            return new MatrixIndex(iIndex, jIndex);
        }



        public void ToggleCellAndNeighbours(Transform playerTr, ref List<WpDataSpawnPedestrian> playerAroundSpawnPoints)
        {
            Find_PlayerCell(playerTr);

            if (PlayerCell_Current != PlayerCell_Previous)
            {
                _openCells_Current.Clear();

                _openCells_Current.Add(PlayerCell_Current);
                foreach (var cll in PlayerCell_Current.neighbours)
                {
                    _openCells_Current.Add(cll);
                }

                foreach (var cll in _openCells_Current)
                {
                    if (!_openCells_Previous.Contains(cll))
                    {
                        Matrix[cll.I, cll.J].Open();
                    }
                }
                foreach (var cll in _openCells_Previous)
                {
                    if (!_openCells_Current.Contains(cll))
                    {
                        Matrix[cll.I, cll.J].Close();
                    }
                }



                PlayerCell_Previous = PlayerCell_Current;
                _openCells_Previous = new List<P_MatrixCell>(_openCells_Current);



                playerAroundSpawnPoints.Clear();
                foreach (var cll in _openCells_Current)
                {
                    foreach (var spawnPoint in cll.spawnPoints)
                    {
                        playerAroundSpawnPoints.Add(spawnPoint);
                    }
                }
            }
        }
        public void Find_PlayerCell(Transform playerTr)
        {
            // Dünya konumunu MatrixCreator'ýn yerel konumuna dönüþtür.
            var localPosition = transform.InverseTransformPoint(playerTr.position);
            var pCell_I = Mathf.FloorToInt((localPosition.z + AreaWidth_I * 0.5f) / AreaWidth_I * Dimension_I);
            var pCell_J = Mathf.FloorToInt((localPosition.x + AreaWidth_J * 0.5f) / AreaWidth_J * Dimension_J);

            if (pCell_I >= 0 && pCell_I < Dimension_I && pCell_J >= 0 && pCell_J < Dimension_J)
            {
                PlayerCell_Current = Matrix[pCell_I, pCell_J];
            }

            //Debug.Log($"i:{_imc.PlayerCell_I} - j:{_imc.PlayerCell_J}");
        }







        #region Gizmos

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 center = transform.position;
            Gizmos.DrawWireCube(center, new Vector3(AreaWidth_J, -0.01f, AreaWidth_I));
        }

        #endregion
    }
}