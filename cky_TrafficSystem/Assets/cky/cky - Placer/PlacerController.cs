//using Rewired;
//using UnityEngine;

//namespace cky.Placer
//{
//    public class PlacerController : MonoBehaviour
//    {
//        //[Header("Rewired")]
//        //public Player RewiredInput;

//        [field: SerializeField] public PlacerData PlacerData { get; private set; }

//        Camera _cam;

//        IPlaceable _canHoldIPlaceable;
//        public IPlaceable holdingIPlaceable;

//        public bool IsHolding;
//        public bool IsHoldingObjectPlaceable;
//        public bool IsRayOnGround;
//        Vector3 hitPosWhenRayIsOnGround;

//        private void Awake()
//        {
//            _cam = Camera.main;
//        }

//        //private void Start()
//        //{
//        //    RewiredInput = ReInput.players.GetPlayer(0);
//        //}

//        private void Update()
//        {
//            if (_canHoldIPlaceable != null && !IsHolding)
//            {
//                Debug.Log("Taþýmak için E bas.");
//            }

//            if (IsHolding)
//            {
//                float mouseWheel = Input.mouseScrollDelta.y;
//                float rotationAmount = mouseWheel * PlacerData.holdRotationDegrees;
//                holdingIPlaceable.Transform.Rotate(Vector3.up, rotationAmount);

//                if (IsRayOnGround)
//                {
//                    Update_HoldingObjectPositionSlowly(hitPosWhenRayIsOnGround);
//                }
//                else
//                {
//                    Update_HoldingObjectPositionSlowly(_cam.transform.position + _cam.transform.forward * PlacerData.holdingOffset_Z);
//                }

//                if (IsHoldingObjectPlaceable)
//                {
//                    if (Input.GetKeyDown(KeyCode.E)/*RewiredInput.GetButtonDown("Use0")*/)
//                    {
//                        holdingIPlaceable.Place(holdingIPlaceable.Transform.position, holdingIPlaceable.Transform.rotation);

//                        holdingIPlaceable = null;
//                        IsHolding = false;
//                        IsRayOnGround = false;
//                    }
//                }
//            }
//            else
//            {
//                if (Input.GetKeyDown(KeyCode.E)/*RewiredInput.GetButtonDown("Use0")*/)
//                {
//                    if (_canHoldIPlaceable != null)
//                    {
//                        holdingIPlaceable = _canHoldIPlaceable;
//                        holdingIPlaceable.Holding(PlacerData);

//                        IsHolding = true;
//                    }
//                    else
//                    {

//                    }
//                }
//            }
//        }

//        void Update_HoldingObjectPositionSlowly(Vector3 targetPosition)
//        {
//            holdingIPlaceable.Transform.position = Vector3.Lerp(holdingIPlaceable.Transform.position, targetPosition, 15 * Time.deltaTime);
//        }

//        void FixedUpdate()
//        {
//            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
//            Ray ray = _cam.ScreenPointToRay(screenCenter);

//            RaycastHit hit;



//            if (IsHolding)
//            {
//                if (Physics.Raycast(ray, out hit, PlacerData.rayDistance, PlacerData.groundLayerMask))
//                {
//                    IsRayOnGround = true;
//                    hitPosWhenRayIsOnGround = hit.point;
//                }
//                else
//                {
//                    var holdingPos = _cam.transform.position + _cam.transform.forward * PlacerData.holdingOffset_Z;
//                    if (Physics.Raycast(holdingPos, Vector3.down, out hit, 50, PlacerData.groundLayerMask))
//                    {
//                        IsRayOnGround = true;
//                        hitPosWhenRayIsOnGround = hit.point;
//                    }
//                    else
//                    {
//                        IsRayOnGround = false;
//                    }
//                }

//                IsHoldingObjectPlaceable = holdingIPlaceable.IsPlaceable(PlacerData.obstacleMask, PlacerData);
//            }
//            else
//            {
//                IsRayOnGround = false;

//                if (Physics.Raycast(ray, out hit, PlacerData.rayDistance, PlacerData.placeableLayerMask))
//                {
//                    if (hit.transform.parent.TryGetComponent<IPlaceable>(out var iPlaceable))
//                    {
//                        if (_canHoldIPlaceable != null)
//                        {
//                            if (_canHoldIPlaceable != iPlaceable)
//                            {
//                                _canHoldIPlaceable.Highlight(false, PlacerData);
//                                _canHoldIPlaceable = null;
//                            }
//                        }

//                        _canHoldIPlaceable = iPlaceable;
//                        _canHoldIPlaceable.Highlight(true, PlacerData);
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
//        }

//        void UnHighlight()
//        {
//            if (_canHoldIPlaceable != null)
//            {
//                _canHoldIPlaceable.Highlight(false, PlacerData);
//                _canHoldIPlaceable = null;
//            }
//        }
//    }
//}