//using game.HolderController;
//using game.ProductBase;
//using game.Customer;
//using UnityEngine;
//using System;
//using cky.GameZones;

//public class EventBus : MonoBehaviour
//{
//    public static event Action<NotificationType> OnNotify;

//    public static event Action<int> OnCountdown;
//    public static event Action<Vector3, Vector3, SexType> OnDamage_Someone;
//    public static event Action<Vector3, Vector3, SexType> OnDie_Someone;
//    public static event Action<Vector3, Vector3> OnHit_Baseballbat;
//    public static event Action<Vector3, Vector3> OnHit_Sledgehammer;

//    public static event Action<HolderStates> OnHolderStateChange;
//    public static event Action<HolderStates2> OnHolderState2Change;
//    public static event Action<HC_States> OnChange_HC_State;
//    public static event Action<HC_States2> OnChange_HC_State2;
//    public static event Action<ProductBoxAbstract> OnChange_HC_State2_ForHighlighted;

//    public static event Action<float> OnMoneyChange;
//    public static event Action<float> OnSaleIncome;
//    public static event Action<float> OnSupplyExpense;
//    public static event Action<float> OnStoreRestorationExpense;
//    public static event Action OnLevelUp_Store;

//    public static event Action<ProductBoxAbstract> OnShelfActive;
//    public static event Action<ProductBoxAbstract> OnShelfDeactive;

//    public static event Action<ProductModelType, float> OnProductPriceSet;
//    public static event Action<ProductModelType, float> OnGameZonePriceSet;
//    public static event Action<GameZoneType, float> OnGameZoneStartedToUse;

//    public static event Action<int> OnWallBreake;

//    public static event Action<bool> OnUIPanelOpen;
//    public static event Action<bool> OnCrosshairOpen;

//    public static event Action OnStickChangerPanelClose;
//    public static event Action<StickType> OnStickClickedFromUI;
//    public static event Action<StickType> OnStickOpen;

//    public static event Action OnGameSave;

//    public static event Action OnStoreOpen;
//    public static event Action OnStoreClose;
//    public static event Action On1HourBeforeStoreTimeEnd;
//    public static event Action OnStoreTimeEnd;
//    public static event Action OnStoreTimeEndAndAllTheCustomersAreGone;
//    public static event Action OnDayEnd;

//    public static event Action<int> OnExperienceChange;
//    public static event Action<int> OnPlayerLevelUpdate;

//    public static event Action<Customer_StateMachine, CustomerSituation, ProductBaseClass> OnCustomerSituationChange;

//    public static event Action<Customer_StateMachine> OnCustomerEnterTheStore;
//    public static event Action<Customer_StateMachine> OnCustomerExitTheStore;
//    public static event Action OnAllCustomersExitTheStore;

//    public static event Action OnContinueButtonClickedOnGameScene;

//    public static event Action OnMenuButton_Highlight;
//    public static event Action OnMenuButton_Click;
//    public static event Action OnButtonClicked_Back;
//    public static event Action OnButtonClicked_ProductAdd;
//    public static event Action OnButtonClicked_BrandContract;
//    public static event Action OnButtonClicked_CartMinus;
//    public static event Action OnButtonClicked_CartPlus;
//    public static event Action OnButtonClicked_SetPrice;
//    public static event Action OnBuy;
//    public static event Action OnSell;

//    public static event Action OnBoxOpen;
//    public static event Action OnHold_FurnitureBox;
//    public static event Action OnHold_Furniture;
//    public static event Action OnPlace_FurnitureBox;
//    public static event Action OnPlace_Furniture;
//    public static event Action OnPlace_ProductsAsProductGroup;
//    public static event Action OnPlace_ProductToShelf;
//    public static event Action OnCollect_ProductFromGround;
//    public static event Action OnCollect_ProductFromBox;
//    public static event Action OnCollect_ProductFromShelf;
//    public static event Action<Vector3> OnDelivery_Complete;

//    public static event Action<bool, Transform> OnCashRegisterUse;
//    public static event Action OnCashRegisterHold;
//    public static event Action OnCashRegisterPlace;
//    public static event Action OnClickProductAt_CashRegister;

//    public static event Action<Vector3> OnCreate_TrashBag;
//    public static event Action OnHold_TrashBag;
//    public static event Action<TrashBag> OnThrow_TrashBag;
//    public static event Action<Vector3, float> OnHit_TrashBag;
//    public static event Action<ProductBoxAbstract> OnThrow_BigBox;
//    public static event Action<ProductBoxAbstract, float, IckySound> OnHit_BigBox;
//    public static event Action<ProductBoxAbstract> OnThrow_ProductBox;
//    public static event Action<ProductBoxAbstract, float, IckySound> OnHit_ProductBox;

//    public static event Action OnChange_Wall;
//    public static event Action OnChange_Floor;

//    public static event Action<float> OnChange_MouseSensivity;
//    public static event Action<int> OnChange_Antialiasing;
//    public static event Action<bool> OnChange_MotinBlur;
//    public static event Action<bool> OnChange_Fog;
//    public static event Action<string> OnChange_Language;

//    public static event Action OnClick_SettingsOKButton;

//    public static event Action<Color> OnChange_LightingColor;
//    public static event Action<Color> OnChange_MainStoreLightingColor;
//    public static event Action<string> OnChange_MainStoreName;
//    public static event Action<int> OnChange_MainStoreMusic;


//    private void Awake()
//    {
//        OnNotify = null;

//        OnCountdown = null;
//        OnDamage_Someone = null;
//        OnDie_Someone = null;
//        OnHit_Baseballbat = null;
//        OnHit_Sledgehammer = null;

//        OnHolderStateChange = null;
//        OnHolderState2Change = null;
//        OnChange_HC_State = null;
//        OnChange_HC_State2 = null;
//        OnChange_HC_State2_ForHighlighted = null;

//        OnMoneyChange = null;
//        OnSaleIncome = null;
//        OnSupplyExpense = null;
//        OnStoreRestorationExpense = null;
//        OnLevelUp_Store = null;

//        OnShelfActive = null;
//        OnShelfDeactive = null;

//        OnProductPriceSet = null;
//        OnGameZonePriceSet = null;
//        OnGameZoneStartedToUse = null;

//        OnWallBreake = null;

//        OnUIPanelOpen = null;
//        OnCrosshairOpen = null;

//        OnStickChangerPanelClose = null;
//        OnStickClickedFromUI = null;
//        OnStickOpen = null;

//        OnGameSave = null;

//        OnStoreOpen = null;
//        OnStoreClose = null;
//        On1HourBeforeStoreTimeEnd = null;
//        OnStoreTimeEnd = null;
//        OnStoreTimeEndAndAllTheCustomersAreGone = null;
//        OnDayEnd = null;

//        OnExperienceChange = null;
//        OnPlayerLevelUpdate = null;

//        OnCustomerSituationChange = null;

//        OnCustomerEnterTheStore = null;
//        OnCustomerExitTheStore = null;
//        OnAllCustomersExitTheStore = null;

//        OnContinueButtonClickedOnGameScene = null;

//        OnMenuButton_Highlight = null;
//        OnMenuButton_Click = null;
//        OnButtonClicked_Back = null;
//        OnButtonClicked_ProductAdd = null;
//        OnButtonClicked_BrandContract = null;
//        OnButtonClicked_CartMinus = null;
//        OnButtonClicked_CartPlus = null;
//        OnButtonClicked_SetPrice = null;
//        OnBuy = null;
//        OnSell = null;

//        OnBoxOpen = null;
//        OnHold_FurnitureBox = null;
//        OnHold_Furniture = null;
//        OnPlace_FurnitureBox = null;
//        OnPlace_Furniture = null;
//        OnPlace_ProductsAsProductGroup = null;
//        OnPlace_ProductToShelf = null;
//        OnCollect_ProductFromGround = null;
//        OnCollect_ProductFromBox = null;
//        OnCollect_ProductFromShelf = null;
//        OnDelivery_Complete = null;

//        OnCashRegisterUse = null;
//        OnCashRegisterHold = null;
//        OnCashRegisterPlace = null;
//        OnClickProductAt_CashRegister = null;

//        OnCreate_TrashBag = null;
//        OnHold_TrashBag = null;
//        OnThrow_TrashBag = null;
//        OnHit_TrashBag = null;
//        OnThrow_BigBox = null;
//        OnHit_BigBox = null;
//        OnThrow_ProductBox = null;
//        OnHit_ProductBox = null;

//        OnChange_Wall = null;
//        OnChange_Floor = null;

//        OnChange_MouseSensivity = null;
//        OnChange_Antialiasing = null;
//        OnChange_MotinBlur = null;
//        OnChange_Fog = null;
//        OnChange_Language = null;

//        OnClick_SettingsOKButton = null;

//        OnChange_LightingColor = null;
//        OnChange_MainStoreLightingColor = null;
//        OnChange_MainStoreName = null;
//        OnChange_MainStoreMusic = null;
//    }



//    public static void OnNotify_EventTrigger(NotificationType warningType) => OnNotify?.Invoke(warningType);



//    public static void OnCountdown_EventTrigger(int countdown) => OnCountdown?.Invoke(countdown);
//    public static void OnDamage_Someone_EventTrigger(Vector3 hitPoint, Vector3 hitDirection, SexType sexType) => OnDamage_Someone?.Invoke(hitPoint, hitDirection, sexType);
//    public static void OnDie_Someone_EventTrigger(Vector3 hitPoint, Vector3 hitDirection, SexType sexType) => OnDie_Someone?.Invoke(hitPoint, hitDirection, sexType);

//    public static void OnHit_Baseballbat_EventTrigger(Vector3 hitPoint, Vector3 hitDirection) => OnHit_Baseballbat?.Invoke(hitPoint, hitDirection);
//    public static void OnHit_Sledgehammer_EventTrigger(Vector3 hitPoint, Vector3 hitDirection) => OnHit_Sledgehammer?.Invoke(hitPoint, hitDirection);



//    public static void OnHolderStateChanges_EventTrigger(HolderStates newState) => OnHolderStateChange?.Invoke(newState);
//    public static void OnHolderState2Changes_EventTrigger(HolderStates2 newState) => OnHolderState2Change?.Invoke(newState);
//    public static void OnChange_HC_State_EventTrigger(HC_States newState) => OnChange_HC_State?.Invoke(newState);
//    public static void OnChange_HC_State2_EventTrigger(HC_States2 newState) => OnChange_HC_State2?.Invoke(newState);
//    public static void OnChange_HC_State2_ForHighlighted_EventTrigger(ProductBoxAbstract pba) => OnChange_HC_State2_ForHighlighted?.Invoke(pba);



//    public static void OnMoneyChange_EventTrigger(float amountOfChange) => OnMoneyChange?.Invoke(amountOfChange);
//    public static void OnSaleIncome_EventTrigger(float amountOfChange) => OnSaleIncome?.Invoke(amountOfChange);
//    public static void OnSupplyExpense_EventTrigger(float amountOfChange) => OnSupplyExpense?.Invoke(amountOfChange);
//    public static void OnStoreRestorationExpense_EventTrigger(float amountOfChange) => OnStoreRestorationExpense?.Invoke(amountOfChange);
//    public static void OnLevelUp_Store_EventTrigger() => OnLevelUp_Store?.Invoke();



//    public static void OnShelfActive_EventTrigger(ProductBoxAbstract pba) => OnShelfActive?.Invoke(pba);
//    public static void OnShelfDeactive_EventTrigger(ProductBoxAbstract pba) => OnShelfDeactive?.Invoke(pba);



//    public static void OnProductPriceSet_EventTrigger(ProductModelType targetPMT, float newSellPrice) => OnProductPriceSet?.Invoke(targetPMT, newSellPrice);
//    public static void OnGameZonePriceSet_EventTrigger(ProductModelType targetPMT, float newSellPrice) => OnGameZonePriceSet?.Invoke(targetPMT, newSellPrice);
//    public static void OnGameZoneStartedToUse_EventTrigger(GameZoneType gameZoneType, float durationToBeUsed) => OnGameZoneStartedToUse?.Invoke(gameZoneType, durationToBeUsed);



//    public static void OnWallBreake_EventTrigger(int index) => OnWallBreake?.Invoke(index);



//    public static void OnUIPanelOpen_EventTrigger(bool b) => OnUIPanelOpen?.Invoke(b);
//    public static void OnCrosshairOpen_EventTrigger(bool b) => OnCrosshairOpen?.Invoke(b);



//    public static void OnStickChangerPanelClose_EventTrigger() => OnStickChangerPanelClose?.Invoke();
//    public static void OnStickClickedFromUI_EventTrigger(StickType stickType) => OnStickClickedFromUI?.Invoke(stickType);
//    public static void OnStickOpen_EventTrigger(StickType stickType) => OnStickOpen?.Invoke(stickType);


//    public static void OnGameSave_EventTrigger() => OnGameSave?.Invoke();


//    public static void OnStoreOpen_EventTrigger() => OnStoreOpen?.Invoke();
//    public static void OnStoreClose_EventTrigger() => OnStoreClose?.Invoke();
//    public static void On1HourBeforeStoreTimeEnd_EventTrigger() => On1HourBeforeStoreTimeEnd?.Invoke();
//    public static void OnStoreTime_EventTrigger() => OnStoreTimeEnd?.Invoke();
//    public static void OnStoreTimeEndAndAllTheCustomersAreGone_EventTrigger() => OnStoreTimeEndAndAllTheCustomersAreGone?.Invoke();
//    public static void OnDayEnd_EventTrigger() => OnDayEnd?.Invoke();


//    public static void OnExperienceChange_EventTrigger(int delta_XP) => OnExperienceChange?.Invoke(delta_XP);
//    public static void OnPlayerLevelUpdate_EventTrigger(int playerLevel) => OnPlayerLevelUpdate?.Invoke(playerLevel);



//    public static void OnCustomerSituationChange_EventTrigger(Customer_StateMachine customer_StateMachine, CustomerSituation situation, ProductBaseClass productBaseClass) => OnCustomerSituationChange?.Invoke(customer_StateMachine, situation, productBaseClass);



//    public static void OnCustomerEnterTheStore_EventTrigger(Customer_StateMachine customer_StateMachine) => OnCustomerEnterTheStore?.Invoke(customer_StateMachine);
//    public static void OnCustomerExitTheStore_EventTrigger(Customer_StateMachine customer_StateMachine) => OnCustomerExitTheStore?.Invoke(customer_StateMachine);
//    public static void OnAllCustomersExitTheStore_EventTrigger() => OnAllCustomersExitTheStore?.Invoke();



//    public static void OnContinueButtonClickedOnGameScene_EventTrigger() => OnContinueButtonClickedOnGameScene?.Invoke();



//    public static void OnMenuButton_Highlight_EventTrigger() => OnMenuButton_Highlight?.Invoke();
//    public static void OnMenuButton_Click_EventTrigger() => OnMenuButton_Click?.Invoke();
//    public static void OnButtonClicked_Back_EventTrigger() => OnButtonClicked_Back?.Invoke();
//    public static void OnButtonClicked_ProductAdd_EventTrigger() => OnButtonClicked_ProductAdd?.Invoke();
//    public static void OnButtonClicked_BrandContract_EventTrigger() => OnButtonClicked_BrandContract?.Invoke();
//    public static void OnButtonClicked_CartMinus_EventTrigger() => OnButtonClicked_CartMinus?.Invoke();
//    public static void OnButtonClicked_CartPlus_EventTrigger() => OnButtonClicked_CartPlus?.Invoke();
//    public static void OnButtonClicked_SetPrice_EventTrigger() => OnButtonClicked_SetPrice?.Invoke();
//    public static void OnBuy_EventTrigger() => OnBuy?.Invoke();
//    public static void OnSell_EventTrigger() => OnSell?.Invoke();



//    public static void OnBoxOpen_EventTrigger() => OnBoxOpen?.Invoke();
//    public static void OnHold_FurnitureBox_EventTrigger() => OnHold_FurnitureBox?.Invoke();
//    public static void OnHold_Furniture_EventTrigger() => OnHold_Furniture?.Invoke();
//    public static void OnPlace_FurnitureBox_EventTrigger() => OnPlace_FurnitureBox?.Invoke();
//    public static void OnPlace_Furniture_EventTrigger() => OnPlace_Furniture?.Invoke();
//    public static void OnPlace_ProductsAsProductGroup_EventTrigger() => OnPlace_ProductsAsProductGroup?.Invoke();
//    public static void OnPlace_ProductToShelf_EventTrigger() => OnPlace_ProductToShelf?.Invoke();
//    public static void OnGather_ProductFromGround_EventTrigger() => OnCollect_ProductFromGround?.Invoke();
//    public static void OnGather_ProductFromBox_EventTrigger() => OnCollect_ProductFromBox?.Invoke();
//    public static void OnGather_ProductFromShelf_EventTrigger() => OnCollect_ProductFromShelf?.Invoke();
//    public static void OnDelivery_Complete_EventTrigger(Vector3 deliveryTruckPosition) => OnDelivery_Complete?.Invoke(deliveryTruckPosition);



//    public static void OnCashRegisterUse_EventTrigger(bool b, Transform cashRegisterUseTransform) => OnCashRegisterUse?.Invoke(b, cashRegisterUseTransform);
//    public static void OnCashRegisterHold_EventTrigger() => OnCashRegisterHold?.Invoke();
//    public static void OnCashRegisterPlace_EventTrigger() => OnCashRegisterPlace?.Invoke();
//    public static void OnClickProductAt_CashRegister_EventTrigger() => OnClickProductAt_CashRegister?.Invoke();



//    public static void OnCreate_TrashBag_EventTrigger(Vector3 pos) => OnCreate_TrashBag?.Invoke(pos);
//    public static void OnHold_TrashBag_EventTrigger() => OnHold_TrashBag?.Invoke();
//    public static void OnThrow_TrashBag_EventTrigger(TrashBag trashBag) => OnThrow_TrashBag?.Invoke(trashBag);
//    public static void OnHit_TrashBag_EventTrigger(Vector3 pos, float hitMultiplier) => OnHit_TrashBag?.Invoke(pos, hitMultiplier);
//    public static void OnThrow_BigBox_EventTrigger(ProductBoxAbstract pba) => OnThrow_BigBox?.Invoke(pba);
//    public static void OnHit_BigBox_EventTrigger(ProductBoxAbstract pba, float hitMultiplier, IckySound ckySound = null) => OnHit_BigBox?.Invoke(pba, hitMultiplier, ckySound);
//    public static void OnThrow_ProductBox_EventTrigger(ProductBoxAbstract pba) => OnThrow_ProductBox?.Invoke(pba);
//    public static void OnHit_ProductBox_EventTrigger(ProductBoxAbstract pba, float hitMultiplier, IckySound ckySound = null) => OnHit_ProductBox?.Invoke(pba, hitMultiplier, ckySound);



//    public static void OnChange_Wall_EventTrigger() => OnChange_Wall?.Invoke();
//    public static void OnChange_Floor_EventTrigger() => OnChange_Floor?.Invoke();



//    public static void OnChange_MouseSensivity_EventTrigger(float newSensivity) => OnChange_MouseSensivity?.Invoke(newSensivity);
//    public static void OnChange_Antialiasing_EventTrigger(int mode) => OnChange_Antialiasing?.Invoke(mode);
//    public static void OnChange_MotionBlur_EventTrigger(bool isOpen) => OnChange_MotinBlur?.Invoke(isOpen);
//    public static void OnChange_Fog_EventTrigger(bool isOpen) => OnChange_Fog?.Invoke(isOpen);
//    public static void OnChange_Language_EventTrigger(string languageCode) => OnChange_Language?.Invoke(languageCode);



//    public static void OnClick_SettingsOKButton_EventTrigger() => OnClick_SettingsOKButton?.Invoke();



//    public static void OnChange_LightingColor_EventTrigger(Color newColor) => OnChange_LightingColor?.Invoke(newColor);
//    public static void OnChange_MainStoreLightingColor_EventTrigger(Color newColor) => OnChange_MainStoreLightingColor?.Invoke(newColor);
//    public static void OnChange_MainStoreName_EventTrigger(string name) => OnChange_MainStoreName?.Invoke(name);
//    public static void OnChange_MainStoreMusic_EventTrigger(int index) => OnChange_MainStoreMusic?.Invoke(index);

//}