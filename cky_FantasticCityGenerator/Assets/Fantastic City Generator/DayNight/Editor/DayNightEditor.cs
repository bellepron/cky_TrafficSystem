using UnityEngine;
using UnityEditor;

namespace FCG
{
    [CustomEditor(typeof(DayNight))]
    public class DayNightEditor : Editor
    {

        DayNight dayNight;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            dayNight = (DayNight)target;


            if (dayNight.gameObject.activeInHierarchy)
            {

                GUILayout.Space(10);

                if (!dayNight.directionalLight)
                {
                    Debug.LogWarning("You need to set Directional Light on DayNight prefab");
                    GUILayout.Label("Warning: You need to set Directional Light");
                    GUILayout.Space(10);
                }


                if (GUILayout.Button("Day"))
                {
                    dayNight.isNight = false;
                }

                GUILayout.Space(5);

                if (GUILayout.Button("Night"))
                {
                    dayNight.isNight = true;
                }

                GUILayout.Space(5);

                if (!dayNight.isNight && dayNight.directionalLight)
                {
                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("Sun Intensity: " + dayNight.intenseSunLight);

                    dayNight.intenseSunLight = GUILayout.HorizontalSlider(dayNight.intenseSunLight, 4000, 12000, GUILayout.Width(200));
                    if (dayNight._intenseSunLight != dayNight.intenseSunLight)
                    {
                        dayNight._intenseSunLight = dayNight.intenseSunLight;
                        dayNight.SetDirectionalLight();
                    }
                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(5);

                    EditorGUILayout.BeginHorizontal();

                    GUILayout.Label("Sun Temperature: " + dayNight.temperatureSunLight);

                    dayNight.temperatureSunLight = GUILayout.HorizontalSlider(dayNight.temperatureSunLight, 1000, 20000, GUILayout.Width(200));
                    if (dayNight._temperatureSunLight != dayNight.temperatureSunLight)
                    {
                        dayNight._temperatureSunLight = dayNight.temperatureSunLight;
                        dayNight.SetDirectionalLight();
                    }

                    EditorGUILayout.EndHorizontal();

                    GUILayout.Space(20);
                }



                if (dayNight.night != dayNight.isNight)
                {
                    dayNight.night = dayNight.isNight;
                    dayNight.ChangeMaterial();
                }


                if (dayNight.isNight)
                {

                    if (dayNight.directionalLight)
                    {
                        EditorGUILayout.BeginHorizontal();

                        GUILayout.Label("MoonLight Intensity: " + dayNight.intenseMoonLight);

                        dayNight.intenseMoonLight = GUILayout.HorizontalSlider(dayNight.intenseMoonLight, 400, 1200, GUILayout.Width(200));
                        if (dayNight._intenseMoonLight != dayNight.intenseMoonLight)
                        {
                            dayNight._intenseMoonLight = dayNight.intenseMoonLight;
                            dayNight.SetDirectionalLight();
                        }
                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(5);

                        EditorGUILayout.BeginHorizontal();

                        GUILayout.Label("MoonLight Temperature: " + dayNight.temperatureMoonLight);

                        dayNight.temperatureMoonLight = GUILayout.HorizontalSlider(dayNight.temperatureMoonLight, 1000, 20000, GUILayout.Width(200));
                        if (dayNight._temperatureMoonLight != dayNight.temperatureMoonLight)
                        {
                            dayNight._temperatureMoonLight = dayNight.temperatureMoonLight;
                            dayNight.SetDirectionalLight();
                        }

                        EditorGUILayout.EndHorizontal();

                        GUILayout.Space(20);
                    }

                    dayNight.isSpotLights = GUILayout.Toggle(dayNight.isSpotLights, " SpotLights on Street lighting", GUILayout.Width(240));

                    if (dayNight._spotLights != dayNight.isSpotLights)
                        dayNight.SetStreetLights(dayNight.isNight);

                    dayNight._spotLights = dayNight.isSpotLights;

                }


                GUILayout.Space(20);



            }

        }
        private void OnEnable()
        {
            dayNight = (DayNight)target;

            if (PrefabUtility.GetPrefabAssetType(dayNight.gameObject) != PrefabAssetType.NotAPrefab)
                PrefabUtility.UnpackPrefabInstance((GameObject)dayNight.gameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);

            dayNight.ChangeMaterial();
        }


    }
}