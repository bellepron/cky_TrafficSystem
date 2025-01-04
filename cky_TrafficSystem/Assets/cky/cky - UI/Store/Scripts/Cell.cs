using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace cky.UI.Store
{
    public class Cell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        Vector3 _prevMousePos;
        public bool _selected;
        [SerializeField] CellController _controller;

        public Item Item { get { if (transform.childCount > 0) { return transform.GetChild(0).GetComponent<Item>(); } else { return null; } } }
        public bool IsFull { get { if (transform.childCount > 0) { return true; } else { return false; } } }

        public bool IssFuLL() => IsFull;

        private Image _image;

        private void Awake()
        {
            _controller = GetComponentInParent<CellController>();
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _selected = true;

            _controller.SetCurrentItem(this);

            if (Item != null)
            {
                Item.SetScale(1.2f);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _selected = false;

            if (Item != null)
            {
                Item.SetScale(1f);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _controller.CellPointerCurrentlyOn(this);
            Highlighted(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _controller.CellPointerCurrentlyOn(null);
            Highlighted(false);
        }

        public void Highlighted(bool v)
        {
            var c = v ? Color.yellow : Color.white;
            ColorChange(c);
        }
        public void ItemChangedEffect()
        {
            _image.color = Color.blue;
            StartCoroutine(DelayedColorWhite(0.25f));
        }
        public void ItemChangedEffectWhenDraggedOnCellItemComes()
        {
            _image.color = Color.red;
            StartCoroutine(DelayedColorWhite(0.25f));
        }
        IEnumerator DelayedColorWhite(float t)
        {
            yield return new WaitForSeconds(t);
            ColorChange(Color.white);
        }
        private void ColorChange(Color c) => _image.color = c;
    }
}