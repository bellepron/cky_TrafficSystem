using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace cky.GamePanels
{
    public class GamePanelInput : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public static event Action OnDown, OnUp;
        public static event Action<Vector3> OnMove;
        Vector3 _prevMousePos;

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnDown?.Invoke();

            _prevMousePos = Input.mousePosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var direction = Input.mousePosition - _prevMousePos;
            direction.z = direction.y;
            direction.y = 0;

            OnMove?.Invoke(direction);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            OnUp?.Invoke();
        }
    }
}