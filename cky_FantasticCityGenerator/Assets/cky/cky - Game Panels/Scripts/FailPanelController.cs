using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace cky.GamePanels
{
    public class FailPanelController : PanelControllerAbstract
    {
        private void Start()
        {
            ClosePanel();

            EventManager.Instance.Add_OnGameFail(OnGameFail);

            Button button = panel.AddComponent<Button>();
            button.onClick.AddListener(FailButtonClicked);
        }

        private void OnGameFail() => OpenPanel();

        private void FailButtonClicked() => ReloadScene();

        public void ReloadScene()
        {
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
            Time.timeScale = 1;
        }
    }
}