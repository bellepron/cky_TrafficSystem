
namespace cky.GamePanels
{
    public class GamePanelController : PanelControllerAbstract
    {
        private void Start()
        {
            ClosePanel();

            EventManager.Instance.Add_OnGameStart(OnGameStart);
            EventManager.Instance.Add_OnGameSuccess(OnGameSuccess);
            EventManager.Instance.Add_OnGameFail(OnGameFail);
        }

        private void OnGameStart() => OpenPanel();
        private void OnGameSuccess() => ClosePanel();
        private void OnGameFail() => ClosePanel();
    }
}