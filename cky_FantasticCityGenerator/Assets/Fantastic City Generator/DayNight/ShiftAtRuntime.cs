using UnityEngine;

namespace FCG
{
    public class ShiftAtRuntime : MonoBehaviour
    {
        DayNight dayNight;

        private void Start()
        {

            dayNight = FindObjectOfType<DayNight>();

        }

        private void Update()
        {


            if (Input.GetKeyDown(KeyCode.N))
            {
                if (dayNight)
                {
                    dayNight.isNight = !dayNight.isNight;
                    dayNight.ChangeMaterial();

                }
            }

        }

    }
}