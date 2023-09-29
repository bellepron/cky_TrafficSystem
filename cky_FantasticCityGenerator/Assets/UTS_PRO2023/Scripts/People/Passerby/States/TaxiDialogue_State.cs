//using cky.Car;
using cky.FCG.Pedestrian;
//using cky.Managers;
//using DG.Tweening;
using UnityEngine;

namespace cky.UTS.People.Passersby.StateMachine
{
    public class TaxiDialogue_State : PasserbyBaseState, IDialogueState
    {
        //DriverSingleton _driverSingleton;
        //UIManager _uiManager;

        public TaxiDialogue_State(PasserbyStateMachine stateMachine) : base(stateMachine)
        {

        }

        public override void Enter()
        {
            ////_driverSingleton = DriverSingleton.Instance;
            ////_uiManager = UIManager.Instance;

            //// Cursor aç.

            //// Black Screen Activate.. When done => Camera will turn customer.
            //var seq = DOTween.Sequence();
            //seq.Append(_uiManager.BlackScreenController.Activate(true));
            //seq.AppendInterval(1.25f);
            //seq.AppendCallback(TransitionToCarDialogueCam);
            //seq.Append(_uiManager.BlackScreenController.Activate(false));


            //// Wait animation ends and throw quest infos..

            //seq.AppendInterval(0.25f);
            //seq.AppendCallback(QuestPopUp);
        }

        private void TransitionToCarDialogueCam()
        {
            //_driverSingleton.CharacterData.CarData.customerDialogueVirtualCamera.SetActive(true);
        }

        private void QuestPopUp()
        {
            //UIManager.Instance.QuestProvider.QuestPopUp();
        }

        public override void Exit()
        {
            //// Cursor kapa.

            //var seq = DOTween.Sequence();
            //seq.Append(_uiManager.BlackScreenController.Activate(true));
            //seq.AppendCallback(() =>
            //{
            //    _uiManager.DialogueCanvasController.DeactivatePanel();
            //    _uiManager.CursorController.ActivateCursor(false);
            //});
            //seq.Append(_uiManager.BlackScreenController.Activate(false));
            //seq.AppendCallback(() =>
            //{
            //    _driverSingleton.CharacterData.CarData.customerDialogueVirtualCamera.SetActive(false);
            //    _driverSingleton.ActivateCarControl(true);
            //});
        }

        public override void Tick(float deltaTime)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                EndDialogueWithYes();
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                EndDialogueWithNo();
            }
        }

        public override void FixedTick(float fixedDeltaTime)
        {

        }

        private void EndDialogueWithNo()
        {
            //UIManager.Instance.DialogueCanvasController.NegativeAnswer(this);
        }
        public void NegativeAnswered()
        {
            stateMachine.AnimatorController.NotLeanCarDoor_Trigger();

            stateMachine.State = PasserbyStates.WalkWithoutCheckingCar;
            stateMachine.ChangeState(stateMachine.State);
        }

        private void EndDialogueWithYes()
        {
            //UIManager.Instance.DialogueCanvasController.PositiveAnswer(this);
        }
        public void PositiveAnswered()
        {
            stateMachine.State = PasserbyStates.EnterCar;
            stateMachine.ChangeState(stateMachine.State);
        }
    }
}