using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace cky.TrafficSystem
{
    public class TrafficSystemCanvasController : MonoBehaviour
    {
        TrafficSystem trafficSystem;

        [SerializeField] TextMeshProUGUI nVehiclesTMP;
        [SerializeField] TextMeshProUGUI maxVehiclesTMP;
        [SerializeField] TMP_InputField maxVehiclesInputField;
        [SerializeField] TextMeshProUGUI aroundTMP;
        [SerializeField] Slider aroundSlider;

        //private void Awake()
        //{
        //    trafficSystem = GetComponentInParent<TrafficSystem>();

        //    maxVehiclesTMP.text = $"{trafficSystem.maxVehiclesWithPlayer}";

        //    aroundTMP.text = $"{trafficSystem.around}";
        //    aroundSlider.value = (trafficSystem.around - 100) / 900;

        //    maxVehiclesInputField.onValueChanged.AddListener(OnInputFieldValueChanged);
        //    aroundSlider.onValueChanged.AddListener(OnSliderValueChanged);
        //}

        //private IEnumerator Start()
        //{
        //    yield return new WaitForSeconds(0.1f);

        //    Cursor.lockState = CursorLockMode.Confined;
        //    Cursor.visible = true;
        //}

        ////private void ActivateCursor(bool b)
        ////{
        ////    Cursor.lockState = b ? CursorLockMode.None : CursorLockMode.Confined;
        ////    Cursor.visible = b;
        ////}

        //private void OnInputFieldValueChanged(string newValue)
        //{
        //    trafficSystem.maxVehiclesWithPlayer = int.Parse(newValue);
        //    maxVehiclesTMP.text = $"{newValue}";
        //}

        //private void OnSliderValueChanged(float newValue)
        //{
        //    var around = 100 + aroundSlider.value * 900;
        //    trafficSystem.around = around;
        //    aroundTMP.text = $"{MathF.Floor(around)}";
        //}

        //private void Update()
        //{
        //    nVehiclesTMP.text = $"{trafficSystem.nVehicles}";
        //}
    }
}