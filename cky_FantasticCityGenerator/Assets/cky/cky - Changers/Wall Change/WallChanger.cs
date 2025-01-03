//using UnityEngine;

//namespace cky.Changer.WallChange
//{
//    public class WallChanger : MonoBehaviour
//    {
//        GameSaveData _gameSaveData;
//        GameData _gameData;

//        [SerializeField] Animator animator;

//        Camera _cam;
//        [SerializeField] float rayDistance = 10f;

//        [SerializeField] LayerMask layerMask;
//        [SerializeField] WallTypes wallType;
//        float _currenWallPrice;
//        int _currenWallExperience;

//        IWall _iWall;

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

//            var firstElement = _gameData.wallAllDatas[0];
//            wallType = firstElement.type;
//            _currenWallPrice = firstElement.price;

//            _cam = Camera.main;

//            EventBus.OnUIPanelOpen += Deactivate;
//        }

//        public void OnMouseClick()
//        {
//            Debug.Log("Wall Changer click");

//            if (!_isActive) return;

//            if (_iWall != null)
//            {
//                if (_gameSaveData.Money >= _currenWallPrice)
//                {
//                    animator.ResetTrigger(AnimatorHelper.trigger_Attack);
//                    animator.SetTrigger(AnimatorHelper.trigger_Attack);

//                    _iWall.ChangeWall(wallType);

//                    EventBus.OnChange_Wall_EventTrigger();

//                    EventBus.OnMoneyChange_EventTrigger(-_currenWallPrice);
//                    EventBus.OnStoreRestorationExpense_EventTrigger(-_currenWallPrice);
//                    EventBus.OnExperienceChange_EventTrigger(_currenWallExperience);
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
//                if (hit.transform.TryGetComponent<IWall>(out var iWall))
//                {
//                    if (wallType != iWall.CurrentWallType)
//                    {
//                        if (_iWall != null && iWall == _iWall)
//                        {

//                        }
//                        else
//                        {
//                            UnHighlight();

//                            _iWall = iWall;
//                            _iWall.Highlight(true, wallType);
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
//            if (_iWall != null)
//            {
//                _iWall.Highlight(false);
//                _iWall = null;
//            }
//        }

//        public void ChangeType(WallTypes newType)
//        {
//            wallType = newType;
//            var wallAllData = _gameData.Get_WallAllData(newType);
//            _currenWallPrice = wallAllData.price;
//            _currenWallExperience = wallAllData.experience;
//        }
//    }
//}