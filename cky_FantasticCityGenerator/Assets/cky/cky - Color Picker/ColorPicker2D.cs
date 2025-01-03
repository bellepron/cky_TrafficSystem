using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace cky.CarCustomization
{
    public class ColorPicker2D : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public Image colorPaletteImage;
        public Image selectedColorDisplay;
        public RectTransform marker;

        private Texture2D colorPaletteTexture;

        Color selectedColor;

        void Start()
        {
            if (colorPaletteImage != null)
            {
                colorPaletteTexture = colorPaletteImage.mainTexture as Texture2D;
                // Ensure the texture is readable
                if (colorPaletteTexture != null && !colorPaletteTexture.isReadable)
                {
                    Debug.LogError("ColorPalette texture is not readable. Please set it to be readable in the import settings.");
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            UpdateColor(eventData);

            //EventBus.OnChange_LightingColor_EventTrigger(selectedColor);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateColor(eventData);

            //EventBus.OnChange_LightingColor_EventTrigger(selectedColor);
        }

        private void UpdateColor(PointerEventData eventData)
        {
            RectTransform rectTransform = colorPaletteImage.GetComponent<RectTransform>();
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            // Adjust localCursor from RectTransform space to normalized [0, 1] space
            localCursor.x = (localCursor.x - rectTransform.rect.x) / rectTransform.rect.width;
            localCursor.y = (localCursor.y - rectTransform.rect.y) / rectTransform.rect.height;

            // Clamp values to ensure they are within the texture bounds
            localCursor.x = Mathf.Clamp01(localCursor.x);
            localCursor.y = Mathf.Clamp01(localCursor.y);

            // Convert normalized coordinates to texture coordinates
            float texPosX = localCursor.x * colorPaletteTexture.width;
            float texPosY = localCursor.y * colorPaletteTexture.height;

            selectedColor = colorPaletteTexture.GetPixel((int)texPosX, (int)texPosY);

            // Update marker position
            if (marker != null)
            {
                marker.anchoredPosition = new Vector2(localCursor.x * rectTransform.rect.width, localCursor.y * rectTransform.rect.height) - new Vector2(rectTransform.rect.width * 0.5f, rectTransform.rect.height * 0.5f);
            }

            // Update the selected color display
            if (selectedColorDisplay != null)
            {
                selectedColorDisplay.color = selectedColor;
            }
        }
    }
}