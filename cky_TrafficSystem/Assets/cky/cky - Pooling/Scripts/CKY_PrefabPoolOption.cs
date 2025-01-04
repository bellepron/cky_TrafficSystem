using UnityEngine;
using System;

namespace CKY_Pooling
{
    [Serializable]
    public class CKY_PrefabPoolOption
    {
        public Transform prefabTransform;
        public int instancesToPreload = 1;
        public bool isPoolExpanded = true;
        public bool showDebugLog = false;
        public bool poolCanGrow = false;
        public bool cullDespawned = false;
        public int cullAbove = 10;
        public float cullDelay = 2f;
        public int cullAmount = 1;
        public bool enableHardLimit = false;
        public int hardLimit = 10;
        public bool recycle = false;
    }

}
