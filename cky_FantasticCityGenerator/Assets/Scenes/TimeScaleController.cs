using UnityEngine;

namespace cky.TimeScale
{
    public class TimeScaleController : MonoBehaviour
    {
        [SerializeField] private float scale = 1.0f;

        private void Update() => Time.timeScale = scale;
    }
}