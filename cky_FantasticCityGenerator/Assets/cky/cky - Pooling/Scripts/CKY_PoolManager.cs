using System.Collections.Generic;
using UnityEngine;

namespace CKY_Pooling
{
    [AddComponentMenu("CKY Pooling/CKY_PoolManager")]
    public class CKY_PoolManager : MonoBehaviour
    {
        public List<CKY_PrefabPoolOption> prefabPoolOptions = new List<CKY_PrefabPoolOption>();
        public bool showDebugLog = false;
        public bool isRootExpanded = true;
        public bool autoAddMissingPrefabPool = false;
        public bool usePoolManager = true;

        private static Dictionary<string, CKY_PrefabPool> Pools = new Dictionary<string, CKY_PrefabPool>();
        private static Transform parentTransform;
        private static CKY_PoolManager instance;

        private List<CKY_PrefabPoolOption> itemsMarkedForDeletion = new List<CKY_PrefabPoolOption>();

        public static CKY_PoolManager Instance
        {
            get
            {
                return instance;
            }
        }

        void Awake()
        {
            instance = this;

            parentTransform = this.transform;

            Pools.Clear();
            itemsMarkedForDeletion.Clear();

            if (!usePoolManager)
                return;

            // loop through all the pre-allocated pools and initialize all the pool
            for (var i = 0; i < prefabPoolOptions.Count; ++i)
            {
                var item = prefabPoolOptions[i];
                var prefabTransform = item.prefabTransform;
                var name = prefabTransform.name;

                if (item.instancesToPreload <= 0 && !item.poolCanGrow)
                {
                    itemsMarkedForDeletion.Add(item);
                    continue; // no need to pre-allocate any game obj, nothing else to do
                }

                if (prefabTransform == null)
                {
                    Debug.LogWarning("Item at index " + (i + 1) + " in the Pool has no prefab !");
                    continue;
                }

                if (Pools.ContainsKey(name))
                {
                    Debug.LogWarning("Duplicates found in the Pool : " + name);
                }

                // pre-allocate the game objs
                var tmpList = new List<Transform>();

                for (var j = 0; j < item.instancesToPreload; ++j)
                {
                    var newTransform = GameObject.Instantiate(prefabTransform, Vector3.zero, prefabTransform.rotation) as Transform;
                    newTransform.name = name;
                    newTransform.parent = parentTransform;
                    newTransform.gameObject.SetActive(false);

                    tmpList.Add(newTransform);
                }

                var newPrefabPool = new CKY_PrefabPool(tmpList);
                newPrefabPool.showDebugLog = item.showDebugLog;
                newPrefabPool.poolCanGrow = item.poolCanGrow;
                newPrefabPool.parentTransform = parentTransform;

                newPrefabPool.cullDespawned = item.cullDespawned;
                newPrefabPool.cullAbove = item.cullAbove;
                newPrefabPool.cullDelay = item.cullDelay;
                newPrefabPool.cullAmount = item.cullAmount;

                newPrefabPool.enableHardLimit = item.enableHardLimit;
                newPrefabPool.hardLimit = item.hardLimit;

                newPrefabPool.recycle = item.recycle;

                Pools.Add(name, newPrefabPool); //add the pool to the Dictionary
            }

            foreach (var item in itemsMarkedForDeletion)
            {
                prefabPoolOptions.Remove(item);
            }

            itemsMarkedForDeletion.Clear();
        }

        void Update()
        {
            foreach (var item in Pools)
            {
                var prefabPool = Pools[item.Key];

                prefabPool.Poll();
            }
        }

        private static void CreateMissingPrefabPool(Transform missingTrans, string name)
        {
            var newPrefabPool = new CKY_PrefabPool();

            //Set the new pool options here
            newPrefabPool.parentTransform = parentTransform;
            newPrefabPool.poolCanGrow = true;

            Pools.Add(name, newPrefabPool);

            // for the Inspector only
            var newPrefabPoolOption = new CKY_PrefabPoolOption();
            newPrefabPoolOption.prefabTransform = missingTrans;
            newPrefabPoolOption.poolCanGrow = true;
            CKY_PoolManager.Instance.prefabPoolOptions.Add(newPrefabPoolOption);

            if (CKY_PoolManager.Instance.showDebugLog)
            {
                Debug.Log("CKY_PoolManager created Pool Item for missing item : " + name);
            }
        }

        public static Transform Spawn(Transform transToSpawn, Vector3 position, Quaternion rotation)
        {
            if (transToSpawn == null)
            {
                Debug.LogWarning("No Transform passed to Spawn() !");
                return null;
            }

            if (!CKY_PoolManager.Instance.usePoolManager)
            {
                var newTransform = GameObject.Instantiate(transToSpawn, Vector3.zero, transToSpawn.rotation) as Transform;
                newTransform.name = transToSpawn.name;
                newTransform.parent = parentTransform;

                return newTransform;
            }

            var name = transToSpawn.name;
            if (!Pools.ContainsKey(name))
            {
                if (CKY_PoolManager.Instance.autoAddMissingPrefabPool)
                {
                    CreateMissingPrefabPool(transToSpawn, name);
                }
                else
                {
                    Debug.LogWarning(name + " passed to Spawn() is not in the Pool Manager.");
                    return null;
                }
            }

            return Pools[name].Spawn(transToSpawn, position, rotation);
        }

        public static void Despawn(Transform transToDespawn)
        {
            if (transToDespawn == null)
            {
                Debug.LogWarning("No Transform passed to Despawn() !");
                return;
            }

            if (!CKY_PoolManager.Instance.usePoolManager)
            {
                GameObject.Destroy(transToDespawn.gameObject);
                return;
            }

            if (!transToDespawn.gameObject.activeInHierarchy)
            {
                return;
            }

            var name = transToDespawn.name;
            if (!Pools.ContainsKey(name))
            {
                Debug.LogWarning(name + " passed to Despawn() is not in the Pool.");
                return;
            }

            Pools[name].Despawn(transToDespawn);
        }

        public static CKY_PrefabPool GetPool(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            if (!Pools.ContainsKey(name))
            {
                return null;
            }

            return Pools[name];
        }
    }

}