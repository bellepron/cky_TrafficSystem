//using RootMotion.FinalIK;
//using cky.GameZones;
//using game.Customer;
//using System.Linq;
//using UnityEngine;

//public class HandPoserController : MonoBehaviour
//{
//    [SerializeField] Animator Animator;
//    [SerializeField] FullBodyBipedIK fbbIK;

//    [SerializeField] Transform left_HandTarget;
//    [SerializeField] Transform right_HandTarget;

//    [Space(25)]
//    [Header("Game Zone")]
//    [SerializeField] Transform left_HandGamepadTarget;
//    [SerializeField] MeshRenderer[] playStationObjects;
//    [SerializeField] MeshRenderer[] virtualRealityObjects;
//    [SerializeField] MeshRenderer[] drivingKitObjects;
//    [SerializeField] MeshRenderer[] rhythmBladeObjects;
//    [SerializeField] MeshRenderer[] pcTable0Objects;
//    [SerializeField] MeshRenderer[] pcTable1Objects;

//    string _scriptableObjectsFilePath = "Assets/cky/cky - HandPoserSetter (FinalIK)/Hand Datas/";



//    //private void Update()
//    //{
//    //    if (Input.GetKeyDown(KeyCode.H))
//    //    {
//    //        var gameZones = FindObjectsOfType<GameZone>();
//    //        var closestGameZone = gameZones.OrderBy(d => Vector3.Distance(d.transform.position, transform.position)).FirstOrDefault();

//    //        if (closestGameZone != null)
//    //        {
//    //            var stateMachine = GetComponent<Customer_StateMachine>();
//    //            stateMachine.targetGameZone = closestGameZone;
//    //            StartUse_GameZone(stateMachine);
//    //        }
//    //    }
//    //}



//    private void OnEnable()
//    {
//        Pose_Normal();
//    }

//    public void StartUse_GameZone(Customer_StateMachine stateMachine, float durationToBeUsed)
//    {
//        Close_AllObjects();

//        var targetGameZone = stateMachine.targetGameZone;
//        GameZone_User userData = targetGameZone.UserData_Current;
//        transform.position = userData.usingTransform.position;
//        transform.rotation = userData.usingTransform.rotation;
//        left_HandTarget = userData.left_HandTarget;
//        right_HandTarget = userData.right_HandTarget;

//        targetGameZone.Start_Use(stateMachine, durationToBeUsed);

//        switch (targetGameZone.GameZoneType)
//        {
//            case GameZoneType.PlayStation:
//                Pose_Gamepad();
//                break;
//            case GameZoneType.VirtualReality:
//                Pose_VR();
//                break;
//            case GameZoneType.DrivingKit:
//                Pose_SteeringWheel();
//                break;
//            case GameZoneType.RhythmBlade:
//                Pose_RhythmBlade();
//                break;
//            case GameZoneType.PCTable0:
//                Pose_PCTable0();
//                break;
//            case GameZoneType.PCTable1:
//                Pose_PCTable1();
//                break;
//        }
//    }

//    public void Pose_Normal()
//    {
//        fbbIK.solver.IKPositionWeight = 0;
//        Set_HandsSituation(null, null);

//        Close_AllObjects();
//    }

//    public void Pose_Gamepad()
//    {
//        fbbIK.solver.IKPositionWeight = 1;
//        Set_HandsSituation(left_HandGamepadTarget, null);

//        foreach (MeshRenderer mr in playStationObjects) mr.enabled = true;

//        Animator.CrossFade(AnimatorHelper.stateName_Using_PlayStation, 0);
//    }

//    public void Pose_VR()
//    {
//        fbbIK.solver.IKPositionWeight = 0;
//        Set_HandsSituation(null, null);

//        foreach (MeshRenderer mr in virtualRealityObjects) mr.enabled = true;

//        Animator.CrossFade(AnimatorHelper.stateName_Using_VirtualReality, 0);
//    }

//    public void Pose_SteeringWheel()
//    {
//        fbbIK.solver.IKPositionWeight = 1;
//        Set_HandsSituation(left_HandTarget, right_HandTarget);

//        foreach (MeshRenderer mr in drivingKitObjects) mr.enabled = true;

//        Animator.CrossFade(AnimatorHelper.stateName_Using_DrivingKit, 0);
//    }

//    public void Pose_RhythmBlade()
//    {
//        fbbIK.solver.IKPositionWeight = 0;
//        Set_HandsSituation(null, null);

//        foreach (MeshRenderer mr in rhythmBladeObjects) mr.enabled = true;

//        Animator.CrossFade(AnimatorHelper.stateName_Using_RhythmBlade, 0);
//    }

//    public void Pose_PCTable0()
//    {
//        fbbIK.solver.IKPositionWeight = 1;
//        Set_HandsSituation(left_HandTarget, right_HandTarget);

//        foreach (MeshRenderer mr in pcTable0Objects) mr.enabled = true;

//        Animator.CrossFade(AnimatorHelper.stateName_Using_PCTable0, 0);
//    }

//    public void Pose_PCTable1()
//    {
//        fbbIK.solver.IKPositionWeight = 1;
//        Set_HandsSituation(left_HandTarget, right_HandTarget);

//        foreach (MeshRenderer mr in pcTable1Objects) mr.enabled = true;

//        Animator.CrossFade(AnimatorHelper.stateName_Using_PCTable1, 0);
//    }



//    #region Hands Situation

//    void Set_HandsSituation(Transform left_HandTarget, Transform right_HandTarget)
//    {
//        Set_LeftHandTarget(left_HandTarget);
//        Set_RightHandTarget(right_HandTarget);

//        fbbIK.solver.IKPositionWeight = left_HandTarget || right_HandTarget ? 1 : 0;
//    }

//    void Set_LeftHandTarget(Transform left_HandTarget)
//    {
//        fbbIK.solver.leftHandEffector.target = left_HandTarget;

//        if (left_HandTarget != null)
//        {
//            fbbIK.solver.leftHandEffector.positionWeight = 1;
//            fbbIK.solver.leftHandEffector.rotationWeight = 1;
//            fbbIK.solver.leftHandEffector.maintainRelativePositionWeight = 1;
//            fbbIK.solver.leftArmChain.bendConstraint.weight = 0.5f;
//        }
//        else
//        {
//            fbbIK.solver.leftHandEffector.positionWeight = 0;
//            fbbIK.solver.leftHandEffector.rotationWeight = 0;
//            fbbIK.solver.leftHandEffector.maintainRelativePositionWeight = 0;
//            fbbIK.solver.leftArmChain.bendConstraint.weight = 0;
//        }
//    }

//    void Set_RightHandTarget(Transform right_HandTarget)
//    {
//        fbbIK.solver.rightHandEffector.target = right_HandTarget;

//        if (right_HandTarget != null)
//        {
//            fbbIK.solver.rightHandEffector.positionWeight = 1;
//            fbbIK.solver.rightHandEffector.rotationWeight = 1;
//            fbbIK.solver.rightHandEffector.maintainRelativePositionWeight = 1;
//            fbbIK.solver.rightArmChain.bendConstraint.weight = 0.5f;
//        }
//        else
//        {
//            fbbIK.solver.rightHandEffector.positionWeight = 0;
//            fbbIK.solver.rightHandEffector.rotationWeight = 0;
//            fbbIK.solver.rightHandEffector.maintainRelativePositionWeight = 0;
//            fbbIK.solver.rightArmChain.bendConstraint.weight = 0;
//        }
//    }

//    #endregion



//    #region Close All Objects

//    void Close_AllObjects()
//    {
//        foreach (MeshRenderer mr in playStationObjects) mr.enabled = false;
//        foreach (MeshRenderer mr in virtualRealityObjects) mr.enabled = false;
//        //foreach (MeshRenderer mr in drivingKitObjects) mr.enabled = false;
//        foreach (MeshRenderer mr in rhythmBladeObjects) mr.enabled = false;
//        //foreach (MeshRenderer mr in pcTable0Objects) mr.enabled = false;
//        //foreach (MeshRenderer mr in pcTable1Objects) mr.enabled = false;
//    }

//    #endregion



//    #region Set

//    public void Set()
//    {
//        Animator = GetComponent<Animator>();
//        fbbIK = GetComponent<FullBodyBipedIK>();

//        Generate_ArmChainBendGoals();
//    }

//    private void Generate_ArmChainBendGoals()
//    {
//        var left_ACBG = transform.Find("Left - Arm Chain Bend Goal");
//        if (left_ACBG == null)
//        {
//            left_ACBG = new GameObject("Left - Arm Chain Bend Goal").transform;
//            left_ACBG.parent = transform;
//        }
//        left_ACBG.localRotation = new Quaternion(0, 0, 0, 0);
//        left_ACBG.localPosition = new Vector3(-0.5f, 0, 0);
//        fbbIK.solver.leftArmChain.bendConstraint.bendGoal = left_ACBG;

//        var right_ACBG = transform.Find("Right - Arm Chain Bend Goal");
//        if (right_ACBG == null)
//        {
//            right_ACBG = new GameObject("Right - Arm Chain Bend Goal").transform;
//            right_ACBG.parent = transform;
//        }
//        right_ACBG.localRotation = new Quaternion(0, 0, 0, 0);
//        right_ACBG.localPosition = new Vector3(0.5f, 0, 0);
//        fbbIK.solver.rightArmChain.bendConstraint.bendGoal = right_ACBG;
//    }

//    #endregion
//}