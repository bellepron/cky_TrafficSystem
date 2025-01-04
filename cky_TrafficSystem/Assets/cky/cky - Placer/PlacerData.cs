using UnityEngine;

namespace cky.Placer
{
    [CreateAssetMenu(fileName = "Placer Data", menuName = "Datas/Placer Data")]
    public class PlacerData : ScriptableObject
    {
        public Material highlightMat;
        public Material holdingMatPlaceable;
        public Material holdingMatCantPlaceable;

        public float rayDistance = 10f;
        public float holdingOffset_Z = 4;
        public float holdRotationDegrees = 30;

        public LayerMask placeableLayerMask;
        public LayerMask groundLayerMask;
        public LayerMask obstacleMask;
    }
}