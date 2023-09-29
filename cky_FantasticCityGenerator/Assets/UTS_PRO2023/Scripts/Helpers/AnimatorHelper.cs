using UnityEngine;

namespace cky.UTS.Helpers
{
    public static class AnimatorHelper
    {
        public static readonly int a_MoveSpeed = Animator.StringToHash("MoveSpeed");
        public static readonly int a_OpenDoor = Animator.StringToHash("OpenDoor");
        public static readonly int a_EnterCar = Animator.StringToHash("EnterCar");
        public static readonly int a_ExitCar = Animator.StringToHash("ExitCar");
        public static readonly int a_WaveHands = Animator.StringToHash("WaveHands");
        public static readonly int a_LeanCarDoor = Animator.StringToHash("LeanCarDoor");
        public static readonly int a_NotLeanCarDoor = Animator.StringToHash("NotLeanCarDoor");




        public static readonly string s_EnterCar = "EnterCar";
        public static readonly string s_ExitCar = "ExitCar";
        public static readonly string s_WaveHands = "WaveHands";



        // Horror kit character animation
        public static readonly int a_Idle = Animator.StringToHash("Idle");
        public static readonly int a_DriveSit0 = Animator.StringToHash("DriveSit0");

        public static readonly string s_Jump = "Jump";
        public static readonly string s_Run = "Run";
        public static readonly string s_Idle = "Idle";
        public static readonly string s_TurningRight = "TurningRight";
        public static readonly string s_TurningLeft = "TurningLeft";
        public static readonly string s_ClimbSpeed = "ClimbSpeed";
        public static readonly string s_Crouch = "Crouch";
        public static readonly string s_ClimbLadder = "ClimbLadder";
        public static readonly string s_AnimationSpeed = "AnimationSpeed";
        public static readonly string s_Movement = "Movement";
    }
}