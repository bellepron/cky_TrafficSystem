//using UnityEngine;

//namespace cky.Changer.FloorChange
//{
//    public class FloorChanger : MonoBehaviour
//    {
//        GameSaveData _gameSaveData;
//        GameData _gameData;

//        [SerializeField] Animator animator;

//        Camera _cam;
//        [SerializeField] float rayDistance = 10f;

//        [SerializeField] LayerMask layerMask;
//        [SerializeField] FloorTypes floorType;
//        float _currentFloorPrice;
//        int _currentFloorExperience;

//        IFloor _iFloor;

//        bool _isActive;

//        private void OnEnable()
//        {
//            animator.CrossFade(AnimatorHelper.state_Idle, 0);
//        }

//        public void Deactivate(bool b)
//        {
//            _isActive = !b;

//            UnHighlight();
//        }

//        public void Initialize()
//        {
//            var gameSaveManager = GameObject.FindWithTag(TagHelper.GameSaveManager).GetComponent<GameSaveManager>();
//            _gameSaveData = gameSaveManager.Get_GameSaveData();
//            _gameData = gameSaveManager.Get_GameData();

//            var firstElement = _gameData.floorAllDatas[0];
//            ChangeType(firstElement.type);

//            _cam = Camera.main;

//            EventBus.OnUIPanelOpen += Deactivate;
//        }

//        public void OnMouseClick()
//        {
//            Debug.Log("Floor Changer click");

//            if (!_isActive) return;

//            if (_iFloor != null)
//            {
//                if (_gameSaveData.Money >= _currentFloorPrice)
//                {
//                    animator.ResetTrigger(AnimatorHelper.trigger_Attack);
//                    animator.SetTrigger(AnimatorHelper.trigger_Attack);

//                    _iFloor.ChangeFloor(floorType);

//                    EventBus.OnChange_Floor_EventTrigger();

//                    EventBus.OnMoneyChange_EventTrigger(-_currentFloorPrice);
//                    EventBus.OnStoreRestorationExpense_EventTrigger(-_currentFloorPrice);
//                    EventBus.OnExperienceChange_EventTrigger(_currentFloorExperience);
//                }
//                else
//                {
//                    EventBus.OnNotify_EventTrigger(NotificationType.CantBuy_NotEnoughMoney);
//                }
//            }
//        }

//        void FixedUpdate()
//        {
//            if (!_isActive) return;

//            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
//            Ray ray = _cam.ScreenPointToRay(screenCenter);

//            RaycastHit hit;

//            if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
//            {
//                if (hit.transform.TryGetComponent<IFloor>(out var iFloor))
//                {
//                    if (floorType != iFloor.CurrentFloorType)
//                    {
//                        if (_iFloor != null && iFloor == _iFloor)
//                        {

//                        }
//                        else
//                        {
//                            UnHighlight();

//                            _iFloor = iFloor;
//                            _iFloor.Highlight(true, floorType);
//                        }
//                    }
//                    else
//                    {
//                        UnHighlight();
//                    }
//                }
//                else
//                {
//                    UnHighlight();
//                }
//            }
//            else
//            {
//                UnHighlight();
//            }
//        }

//        void UnHighlight()
//        {
//            if (_iFloor != null)
//            {
//                _iFloor.Highlight(false);
//                _iFloor = null;
//            }
//        }

//        public void ChangeType(FloorTypes newType)
//        {
//            floorType = newType;
//            var floorAllData = _gameData.Get_FloorAllData(newType);
//            _currentFloorPrice = floorAllData.price;
//            _currentFloorExperience = floorAllData.experience;
//        }
//    }
//}