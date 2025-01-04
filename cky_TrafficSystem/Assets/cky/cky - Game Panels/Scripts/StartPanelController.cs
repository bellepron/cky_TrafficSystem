using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace cky.GamePanels
{
    public class StartPanelController : PanelControllerAbstract
    {
        private void Start()
        {
            OpenPanel();

            Button button = panel.AddComponent<Button>();
            button.onClick.AddListener(GameStartButton);
        }

        public void GameStartButton() => StartCoroutine(MyUpdate());

        IEnumerator MyUpdate()
        {
            bool isUpdating = true;

            while (isUpdating == true)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    yield return null;

                    ClosePanel();
                    EventManager.Instance.GameStartEvent();

                    isUpdating = false;
                }

                yield return null;
            }
        }
    }
}