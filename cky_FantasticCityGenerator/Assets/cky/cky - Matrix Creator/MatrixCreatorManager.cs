using UnityEditor;
using UnityEngine;

namespace cky.MatrixCreation
{
    public class MatrixCreatorManager : MonoBehaviour
    {
        [field: SerializeField] public MatrixSettings MatrixSettings { get; private set; }
        [field: SerializeField] public Transform PlayerTransform { get; set; }
        [field: SerializeField] public MatrixCell PlayerCell_Previous { get; set; }
        [field: SerializeField] public MatrixCell PlayerCell_Current { get; set; }

        [field: SerializeField] public GameObject MatrixCreatorGO { get; set; }
        [field: SerializeField] public MatrixCreator MatrixCreator { get; set; }
        [field: SerializeField] public GameObject MatrixCellPrefab { get; set; }
        [field: SerializeField] public float MatrixCellPercent { get; set; } = 0.98f;
        [field: SerializeField] public float AreaWidth_I { get; set; } = 4000f;
        [field: SerializeField] public float AreaWidth_J { get; set; } = 10000f;
        [field: SerializeField] public int Dimension_I { get; set; } = 20;
        [field: SerializeField] public int Dimension_J { get; set; } = 20;
        [field: SerializeField] public MatrixCell[] Matrix { get; set; }

        [Space(15)]
        [Header("Execution")]
        [SerializeField] private float executionFrequency = 0.2f;

        [Space(15)]
        [Header("Open-Close Cell Color")]
        public Color colorCell_Open = Color.cyan;
        public Color colorCell_Close = Color.black;
        [SerializeField] Color color_OuterRectangleFrame = Color.black;

        [Space(15)]
        [Header("Controllers")]
        public MatrixItemData[] matrixItemDatas;
        public MatrixItemTypes[] matrixItemTypes;
        public int matrixItemDatasLength;
        [SerializeField] int countRandom_Min = 25;
        [SerializeField] int countRandom_Max = 100;

        private void Awake()
        {
            if (!PlayerTransform)
            {
                PlayerTransform = GameObject.FindWithTag("Player").transform;
            }

            Initialize(PlayerTransform);
        }

        public void Initialize(Transform playerTransform)
        {
            PlayerTransform = playerTransform;

            MatrixCreator.Find_PlayerCell();

            InvokeRepeating(nameof(Toggle), 0, executionFrequency);
        }

        private void Toggle()
        {
            MatrixCreator.ToggleCellAndNeighbours();
        }



        #region Editor Matrix Set

        public void MatrixSet()
        {
            matrixItemDatasLength = matrixItemDatas.Length;

            CreateMatrixCreator();

            MatrixSettings.Set(this, Dimension_I, Dimension_J);

            MatrixCreator.CompleteMatrix();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(MatrixCreator);
#endif
        }

        private void CreateMatrixCreator()
        {
            for (int i = transform.childCount - 1; i >= 0; i--) DestroyImmediate(transform.GetChild(i).gameObject);
            MatrixCreatorGO = new GameObject($"Matrix Creator");
            MatrixCreatorGO.transform.SetParent(transform, false);
            MatrixCreator = MatrixCreatorGO.AddComponent<MatrixCreator>();

            MatrixCreator.Create(this);
        }

        #endregion



        #region Place Random

        public void PlaceRandom()
        {
            var ObjectsParentTransform = new GameObject("Objects Parent").transform;

            matrixItemDatasLength = matrixItemDatas.Length;

            for (int i = 0; i < matrixItemDatasLength; i++)
            {
                var prefab = matrixItemDatas[i].ItemPrefab;
                var randomCount = UnityEngine.Random.Range(countRandom_Min, countRandom_Max);

                for (int j = 0; randomCount > j; j++)
                {
                    var obj = Instantiate(prefab, GetRandomPositionRelativeToObject(), GetRandomRotation());
                    obj.parent = ObjectsParentTransform;
                }
            }
        }

        public Vector3 GetRandomPositionRelativeToObject()
        {
            float randomX = UnityEngine.Random.Range(-AreaWidth_J / 2f, AreaWidth_J / 2f);
            float randomZ = UnityEngine.Random.Range(-AreaWidth_I / 2f, AreaWidth_I / 2f);

            return transform.position + new Vector3(randomX, 0f, randomZ);
        }

        public Quaternion GetRandomRotation()
        {
            float randomY = UnityEngine.Random.Range(0f, 360f); ;
            return Quaternion.Euler(0f, randomY, 0f);
        }

        #endregion



        #region Gizmos

        private void OnDrawGizmos()
        {
            Gizmos.color = color_OuterRectangleFrame;
            Vector3 center = transform.position;
            Gizmos.DrawWireCube(center, new Vector3(AreaWidth_J, -0.01f, AreaWidth_I));
        }

        #endregion
    }
}