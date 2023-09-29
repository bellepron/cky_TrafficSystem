using UnityEngine;
using UnityEngine.UI;

namespace cky.UI.Store
{
    public class Item : MonoBehaviour
    {
        [field: SerializeField] public ItemTypes ItemType { get; set; }
        public Cell Cell { get { return transform.parent.GetComponent<Cell>(); } }
        private Image _image;

        private void Awake() => _image = GetComponent<Image>();

        private void Start() => _image.color = GetColorWithItemType(ItemType);

        public void SetScale(float value) => transform.localScale = Vector3.one * value;

        private Color GetColorWithItemType(ItemTypes type)
        {
            switch (type)
            {
                case ItemTypes.Type0:
                    return Color.black;
                case ItemTypes.Type1:
                    return Color.gray;
                case ItemTypes.Type2:
                    return Color.green;
                case ItemTypes.Type3:
                    return Color.cyan;
                case ItemTypes.Type4:
                    return Color.magenta;
                case ItemTypes.Type5:
                    return Color.red;

                default:
                    return Color.white;
            }
        }
    }
}