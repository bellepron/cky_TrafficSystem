using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace cky.GamePanels
{
    public class SuccessPanelController : PanelControllerAbstract
    {
        private void Start()
        {
            ClosePanel();

            EventManager.Instance.Add_OnGameSuccess(OnGameSuccess);

            Button button = panel.AddComponent<Button>();
            button.onClick.AddListener(SuccessButtonClicked);
        }

        private void OnGameSuccess() => OpenPanel();

        private void SuccessButtonClicked() => ReloadScene();

        public void ReloadScene()
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
            Time.timeScale = 1;
        }
    }
}