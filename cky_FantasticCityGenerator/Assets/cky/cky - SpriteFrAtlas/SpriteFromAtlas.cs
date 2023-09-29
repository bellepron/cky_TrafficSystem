using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine;

namespace cky.SpriteFrAtlas
{
    public class SpriteFromAtlas : MonoBehaviour
    {
        [SerializeField] SpriteAtlas atlas;
        [SerializeField] string spriteName;
        [SerializeField] Image image;

        private void Start()
        {
            image.sprite = atlas.GetSprite(spriteName);
        }
    }
}