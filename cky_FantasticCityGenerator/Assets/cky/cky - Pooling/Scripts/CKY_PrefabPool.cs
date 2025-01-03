using System.Collections.Generic;
using UnityEngine;

namespace CKY_Pooling
{
    public class CKY_PrefabPool
    {
        public bool showDebugLog = false;
        public bool poolCanGrow = false;
        public bool cullDespawned = false;
        public int cullAbove = 10;
        public float cullDelay = 2f;
        public int cullAmount = 1;
        public bool enableHardLimit = false;
        public int hardLimit = 10;
        public bool recycle = false;
        public Transform parentTransform = null;
        public List<Transform> spawnedList = new List<Transform>();
        public List<Transform> despawnedList = new List<Transform>();

        private float TimeOfLastCull = 0f;

        public CKY_PrefabPool()
        {
            spawnedList.Clear();
            despawnedList.Clear();
        }

        public CKY_PrefabPool(List<Transform> list)
        {
            spawnedList.Clear();
            despawnedList = list;
        }

        public Transform Spawn(Transform transToSpawn, Vector3 position, Quaternion rotation)
        {
            if (despawnedList.Count == 0)
            {
                if (!poolCanGrow)
                {
                    if (recycle)
                    {
                        if (showDebugLog)
                            Debug.Log(transToSpawn.name + " has been recycled. Despawning and Spawning Immediately.");

                        Despawn(spawnedList[0]);
                        return Spawn(transToSpawn, position, rotation);
                    }
                    else
                    {
                        Debug.LogWarning(transToSpawn.name + " has used up all the free preallocated instances. Please increase your Preload Amount.");
                        return null;
                    }
                }
                else
                {
                    if (enableHardLimit)
                    {
                        if (spawnedList.Count >= hardLimit)
                        {
                            if (recycle)
                            {
                                if (showDebugLog)
                                    Debug.Log(transToSpawn.name + " has been recycled. Despawning and Spawning Immediately.");

                                Despawn(spawnedList[0]);
                                return Spawn(transToSpawn, position, rotation);
                            }
                            else
                            {
                                Debug.LogWarning(transToSpawn.name + " has already reached its hard limit. Please increase your hard limit Qty.");
                                return null;
                            }
                        }
                    }

                    // Instantiate a new one
                    var newTransform = GameObject.Instantiate(transToSpawn, Vector3.zero, transToSpawn.rotation) as Transform;
                    newTransform.name = transToSpawn.name;
                    newTransform.parent = parentTransform;
                    newTransform.gameObject.SetActive(false);

                    despawnedList.Add(newTransform);

                    if (showDebugLog)
                    {
                        Debug.Log("CKY_PoolManager Instantiated an extra '" + transToSpawn.name);
                    }
                }
            }

            var freeObject = despawnedList[0];

            if (freeObject == null)
            {
                Debug.LogWarning("User cannot destroy a gameobject while in the despawnedList.");
                return null;
            }

            freeObject.position = position;
            freeObject.rotation = rotation;
            freeObject.gameObject.SetActive(true);

            if (showDebugLog)
            {
                Debug.Log("CKY_PoolManager spawned " + transToSpawn.name);
            }

            freeObject.BroadcastMessage("OnSpawned", SendMessageOptions.DontRequireReceiver);

            despawnedList.Remove(freeObject);
            spawnedList.Add(freeObject);

            return freeObject;
        }

        public void Despawn(Transform transToDespawn)
        {
            transToDespawn.BroadcastMessage("OnDespawned", SendMessageOptions.DontRequireReceiver);

            transToDespawn.parent = parentTransform;
            transToDespawn.gameObject.SetActive(false);

            if (showDebugLog)
            {
                Debug.Log("CKY_PoolManager despawned " + transToDespawn.name);
            }

            spawnedList.Remove(transToDespawn);
            despawnedList.Add(transToDespawn);
        }

        public void Poll()
        {
            if (Time.time > TimeOfLastCull + cullDelay)
            {
                if (!cullDespawned || despawnedList.Count <= cullAbove)
                    return;

                TimeOfLastCull = Time.time;

                for (int i = 0; i < cullAmount; ++i)
                {
                    if (despawnedList.Count == 0)
                        return;

                    if (showDebugLog)
                        Debug.Log(despawnedList[0].name + " Culled!");

                    GameObject.Destroy(despawnedList[0].gameObject);
                    despawnedList.RemoveAt(0);
                }
            }
        }
    }
}