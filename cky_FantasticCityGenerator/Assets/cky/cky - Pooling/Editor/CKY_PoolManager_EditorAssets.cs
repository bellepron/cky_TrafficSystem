using UnityEngine;

namespace CKY_Pooling
{
    public class TextureData
    {
        public Texture tex;
        public string path;

        public TextureData(Texture t, string s)
        {
            tex = t;
            path = s;
        }
    }

    public static class CKY_PoolManager_EditorAssets
    {
        public static string assetName = "cky/cky - Pooling";

        public static TextureData poolManagerItemLogo = new TextureData(null, "CKY Pooling Logo.psd");
        public static TextureData missingPrefabIcon = new TextureData(null, "missingPrefabIcon.psd");
        public static TextureData poolItemTop = new TextureData(null, "Pool Item Top Logo.psd");
        public static TextureData poolItemBottom = new TextureData(null, "Pool Item Bottom Logo.psd");

        public static Color addBtnColor = new Color(0, 1f, 0);
        public static Color delBtnColor = new Color(1f, 0, 0);
        public static Color shiftPosColor = new Color(0.5f, 0.5f, 0.5f);
    }
}