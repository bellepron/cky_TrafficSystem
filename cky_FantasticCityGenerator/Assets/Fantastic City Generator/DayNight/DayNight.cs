using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace FCG
{
    public class DayNight : MonoBehaviour
    {

        //  In the 2 fields below, only the materials that will be alternated in the day/night exchange are registered
        //  When adding your buildings(which will have their own materials), you can register the day and night versions of the materials here.
        //  The index of the daytime version of the material must match the index of the nighttime version of the material
        //  Example: When switching to night scene, materialDay[1] will be replaced by materialNight[1]
        //  (Materials that will be used both night and day do not need to be here)
        public Material[] materialDay;    // Add materials that are only used in the day scene, and are substituted in the night scene
        public Material[] materialNight;  // Add night scene materials that will replace day scene materials. (The sequence must be respected)



        public VolumeProfile volumeProfile_Day;
        public VolumeProfile volumeProfile_Night;

        //Don't forget to add the Directional Light here
        public Light directionalLight;


        [HideInInspector]
        public bool isNight;

        [HideInInspector]
        public bool night;

        [HideInInspector]
        public bool isSpotLights;

        [HideInInspector]
        public bool _spotLights;

        [HideInInspector]
        public float intenseMoonLight = 800f;

        [HideInInspector]
        public float _intenseMoonLight;

        [HideInInspector]
        public float intenseSunLight = 8000f;

        [HideInInspector]
        public float _intenseSunLight;


        [HideInInspector]
        public float temperatureSunLight = 6700f;

        [HideInInspector]
        public float _temperatureSunLight;

        [HideInInspector]
        public float temperatureMoonLight = 9500f;

        [HideInInspector]
        public float _temperatureMoonLight;


        public void ChangeVolume()
        {
            GetComponent<Volume>().profile = (isNight) ? volumeProfile_Night : volumeProfile_Day;
        }

        public void ChangeMaterial()
        {

#if UNITY_2020_1_OR_NEWER

            if (GetComponent<Volume>().profile.TryGet<Exposure>(out var exp))
                exp.compensation.SetValue(new FloatParameter(0));

#endif

            // shift VolumeProfile :  day/night
            GetComponent<Volume>().profile = (isNight) ? volumeProfile_Night : volumeProfile_Day;


            //Configuring the Directional Light as it is day or night (sun/moon)
            SetDirectionalLight();


            /*
            Substituting Night materials for Day materials (or vice versa) in all Mesh Renders within City-Maker
            Only materials that have been added in "materialDay" and "materialNight" Array
            */

            GameObject GmObj = GameObject.Find("City-Maker"); ;
            if (GmObj == null) return;

            Renderer[] children = GmObj.GetComponentsInChildren<Renderer>();

            Material[] myMaterials;

            for (int i = 0; i < children.Length; i++)
            {
                myMaterials = children[i].GetComponent<Renderer>().sharedMaterials;

                for (int m = 0; m < myMaterials.Length; m++)
                {
                    for (int mt = 0; mt < materialDay.Length; mt++)
                        if (isNight)
                        {
                            if (myMaterials[m] == materialDay[mt])
                                myMaterials[m] = materialNight[mt];

                        }
                        else
                        {
                            if (myMaterials[m] == materialNight[mt])
                                myMaterials[m] = materialDay[mt];
                        }


                    children[i].GetComponent<MeshRenderer>().sharedMaterials = myMaterials;
                }


            }


            //Toggles street lamp lights on/off
            SetStreetLights(isNight);



        }

        public void SetDirectionalLight() //Configuring the Directional Light as it is day or night (sun/moon)
        {

            if (directionalLight)
            {
                directionalLight.GetComponent<HDAdditionalLightData>().intensity = (isNight) ? intenseMoonLight : intenseSunLight; // 800 : 8000;

                directionalLight.useColorTemperature = true;
                if (directionalLight.useColorTemperature)
                    directionalLight.colorTemperature = (isNight) ? temperatureMoonLight : temperatureSunLight;

            }
            else
                Debug.LogError("You must set the Directional Light in the Inspector of DayNight Prefab");

        }

        public void SetStreetLights(bool night)
        {
            GameObject[] tempArray = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name == ("_LightV")).ToArray();

            foreach (GameObject lines in tempArray)
            {
                lines.GetComponent<MeshRenderer>().enabled = night;
                if (lines.transform.GetChild(0))
                    lines.transform.GetChild(0).GetComponent<Light>().enabled = (isSpotLights && night);
            }

            tempArray = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(g => g.name == ("_Spot_Light")).ToArray();

            foreach (GameObject lines in tempArray)
                lines.GetComponent<Light>().enabled = (isSpotLights && night);

        }


    }
}