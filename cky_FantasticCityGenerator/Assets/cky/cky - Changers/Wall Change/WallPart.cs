//#if UNITY_EDITOR
//using UnityEditor;
//#endif
//using UnityEngine;
//using UnityEngine.AI;

//namespace cky.Changer.WallChange
//{
//    public class WallPart : MonoBehaviour, IWall
//    {
//        public GameSaveManager GameSaveManager { get; private set; }
//        [field: SerializeField] public int WallGroupIndex { get; private set; }
//        [field: SerializeField] public WallPartChoice[] Choices { get; private set; }
//        [field: SerializeField] public WallTypes CurrentWallType { get; set; }

//        [SerializeField] BoxCollider[] colliders;
//        [SerializeField] NavMeshObstacle[] navMeshObstacles;



//        public void Initialize(GameSaveManager gameSaveManager, int i, WallTypes type)
//        {
//            GameSaveManager = gameSaveManager;
//            WallGroupIndex = i;
//            CurrentWallType = type;
//            ChangeWallTo(CurrentWallType);
//        }

//        public void ChangeWall(WallTypes newWallType)
//        {
//            CurrentWallType = newWallType;
//            ChangeWallTo(CurrentWallType);

//            GameSaveManager.ChangeWall(WallGroupIndex, CurrentWallType);
//        }

//        public void Highlight(bool b, WallTypes newWallType = WallTypes.None)
//        {
//            if (b)
//            {
//                ChangeWallTo(newWallType);
//            }
//            else
//            {
//                ChangeWallTo(CurrentWallType);
//            }
//        }

//        void ChangeWallTo(WallTypes newWallType)
//        {
//            Open_ColliderAndNavMeshObstacle(newWallType != WallTypes.None);

//            foreach (var choice in Choices)
//            {
//                if (choice.wallType == newWallType)
//                {
//                    choice.gameObject.SetActive(true);
//                }
//                else
//                {
//                    choice.gameObject.SetActive(false);
//                }
//            }

//#if UNITY_EDITOR
//            EditorUtility.SetDirty(this);
//#endif
//        }

//        void Open_ColliderAndNavMeshObstacle(bool isOpen)
//        {
//            if (colliders.Length == 1)
//            {
//                colliders[0].enabled = isOpen;
//                navMeshObstacles[0].enabled = isOpen;
//            }
//            else
//            {
//                foreach (var collider in colliders)
//                {
//                    collider.enabled = isOpen;
//                }
//                foreach (var navMeshObstacle in navMeshObstacles)
//                {
//                    navMeshObstacle.enabled = isOpen;
//                }
//            }
//        }
//    }
//}