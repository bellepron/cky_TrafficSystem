//#if UNITY_EDITOR
//using UnityEditor;
//#endif
//using UnityEngine;

//namespace cky.Changer.FloorChange
//{
//    public class FloorPart : MonoBehaviour, IFloor
//    {
//        public GameSaveManager GameSaveManager { get; private set; }
//        [field: SerializeField] public int FloorGroupIndex { get; private set; }
//        public FloorPartChoice[] Choices { get; private set; }
//        public FloorTypes CurrentFloorType { get; set; }



//        public void Initialize(GameSaveManager gameSaveManager, int i, FloorTypes type)
//        {
//            GameSaveManager = gameSaveManager;
//            FloorGroupIndex = i;
//            CurrentFloorType = type;

//            var length = transform.childCount;
//            Choices = new FloorPartChoice[length];
//            for (int j = 0; j < length; j++)
//            {
//                var child = transform.GetChild(j);
//                if (child.TryGetComponent<FloorPartChoice>(out var choice))
//                {
//                    Choices[j] = choice;
//                }
//            }

//            ChangeFloorTo(CurrentFloorType);
//        }

//        public void ChangeFloor(FloorTypes newFloorType)
//        {
//            CurrentFloorType = newFloorType;
//            ChangeFloorTo(CurrentFloorType);

//            GameSaveManager.ChangeFloor(FloorGroupIndex, CurrentFloorType);
//        }

//        public void Highlight(bool b, FloorTypes newFloorType = FloorTypes.None)
//        {
//            if (b)
//            {
//                ChangeFloorTo(newFloorType);
//            }
//            else
//            {
//                ChangeFloorTo(CurrentFloorType);
//            }
//        }

//        void ChangeFloorTo(FloorTypes newFloorType)
//        {
//            foreach (var choice in Choices)
//            {
//                if (choice.floorType == newFloorType)
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
//    }
//}