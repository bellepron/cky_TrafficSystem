//using UnityEngine;
//using UnityEditor;

//namespace cky.TrafficSystem
//{
//    [CustomEditor(typeof(TrafficPedestrian))]
//    public class TCarEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            base.OnInspectorGUI();

//            TrafficPedestrian TF = (TrafficPedestrian)target;

//            if (GUILayout.Button("Set"))
//            {
//                if (TF.gameObject.activeInHierarchy)
//                    TF.Configure();
//                else
//                    Debug.LogWarning("Place the object in the hierarchy");
//            }
//        }
//    }
//}