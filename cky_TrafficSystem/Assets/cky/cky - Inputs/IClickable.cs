using UnityEngine;

namespace cky.Inputs
{
    public interface IClickable
    {
        void OnClick(Vector3 clickedPosition);
    }
}