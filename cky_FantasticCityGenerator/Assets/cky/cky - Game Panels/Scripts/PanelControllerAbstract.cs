using System;
using UnityEngine;

namespace cky.GamePanels
{
    [Serializable]
    public abstract class PanelControllerAbstract : MonoBehaviour
    {
        [SerializeField] protected GameObject panel;
        protected void OpenPanel() => panel.SetActive(true);
        protected void ClosePanel() => panel.SetActive(false);
    }
}