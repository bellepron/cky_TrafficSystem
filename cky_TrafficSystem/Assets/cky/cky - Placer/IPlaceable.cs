using UnityEngine;

namespace cky.Placer
{
    public interface IPlaceable
    {
        public Transform Transform { get; }
        public bool IsHighligted { get; }
        public bool IsHolding { get; }
        void Highlight(bool b, PlacerData placerData);
        void Holding(PlacerData placerData);
        void Place(Vector3 pos, Quaternion rot);
        bool IsPlaceable(LayerMask obstacleMask, PlacerData placerData);
    }
}