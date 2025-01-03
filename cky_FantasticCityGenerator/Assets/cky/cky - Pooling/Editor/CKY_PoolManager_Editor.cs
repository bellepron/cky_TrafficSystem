using UnityEngine;
using UnityEditor;

namespace CKY_Pooling
{
    [CustomEditor(typeof(CKY_PoolManager))]
    public class CKY_PoolManager_Editor : Editor
    {
        private CKY_PoolManager poolManager;

        private Vector2 scrollPos = Vector2.zero;

        public override void OnInspectorGUI()
        {
            EditorGUI.indentLevel = 0;
            GUI.contentColor = Color.white;
            bool isDirty = false;
            poolManager = (CKY_PoolManager)target;

            CKY_PoolManager_EditorUtility.DrawTexture(CKY_PoolManager_EditorAssets.poolManagerItemLogo);

            poolManager.autoAddMissingPrefabPool = EditorGUILayout.Toggle("Auto Add Missing Items", poolManager.autoAddMissingPrefabPool);
            poolManager.showDebugLog = EditorGUILayout.Toggle("Show Debug Log", poolManager.showDebugLog);
            poolManager.usePoolManager = EditorGUILayout.Toggle("Use CKY Pool Manager", poolManager.usePoolManager);

            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);

            EditorGUI.indentLevel = 1;

            poolManager.isRootExpanded = EditorGUILayout.Foldout(poolManager.isRootExpanded, string.Format("Pools ({0})", poolManager.prefabPoolOptions.Count));

            // Add expand / collapse buttons if there are items in the list
            if (poolManager.prefabPoolOptions.Count > 0)
            {
                CKY_PoolManager_EditorUtility.BeginColor(CKY_PoolManager_EditorAssets.shiftPosColor);
                var masterCollapse = GUILayout.Button("Collapse All", EditorStyles.toolbarButton, GUILayout.Width(80));

                var masterExpand = GUILayout.Button("Expand All", EditorStyles.toolbarButton, GUILayout.Width(80));
                CKY_PoolManager_EditorUtility.EndColor();

                if (masterExpand)
                {
                    foreach (var item in poolManager.prefabPoolOptions)
                    {
                        item.isPoolExpanded = true;
                    }
                }

                if (masterCollapse)
                {
                    foreach (var item in poolManager.prefabPoolOptions)
                    {
                        item.isPoolExpanded = false;
                    }
                }
            }
            else
            {
                GUILayout.FlexibleSpace();
            }

            //only enable adding of pools when the application is NOT in play state
            if (!Application.isPlaying) //During Editor
            {
                CKY_PoolManager_EditorUtility.BeginColor(CKY_PoolManager_EditorAssets.addBtnColor);
                if (GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.Width(32)))
                {
                    poolManager.prefabPoolOptions.Insert(0, new CKY_PrefabPoolOption()); //add to the top of the list
                    isDirty = true;
                }
                CKY_PoolManager_EditorUtility.EndColor();
            }

            EditorGUILayout.EndHorizontal();

            if (poolManager.isRootExpanded)
            {
                int i_ToRemove = -1;
                int i_ToInsertAt = -1;
                int i_ToShiftUp = -1;
                int i_ToShiftDown = -1;

                EditorGUILayout.BeginVertical();

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(0), GUILayout.Height(0));

                for (var i = 0; i < poolManager.prefabPoolOptions.Count; ++i)
                {
                    var prefabPoolOption = poolManager.prefabPoolOptions[i];

                    EditorGUI.indentLevel = 2;
                    EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
                    var name = prefabPoolOption.prefabTransform == null ? "[NO PREFAB]" : prefabPoolOption.prefabTransform.name;
                    prefabPoolOption.isPoolExpanded = EditorGUILayout.Foldout(prefabPoolOption.isPoolExpanded, name, EditorStyles.foldout);

                    CKY_PoolManager_EditorUtility.BeginColor(CKY_PoolManager_EditorAssets.shiftPosColor);

                    if (i > 0)
                    {
                        // the up arrow.
                        if (GUILayout.Button("▲", EditorStyles.toolbarButton, GUILayout.Width(24)))
                        {
                            i_ToShiftUp = i;
                        }
                    }
                    else
                    {
                        GUILayout.Space(24);
                    }

                    if (i < poolManager.prefabPoolOptions.Count - 1)
                    {
                        // The down arrow will move things towards the end of the List
                        if (GUILayout.Button("▼", EditorStyles.toolbarButton, GUILayout.Width(24)))
                        {
                            i_ToShiftDown = i;
                        }
                    }
                    else
                    {
                        GUILayout.Space(24);
                    }

                    CKY_PoolManager_EditorUtility.EndColor();

                    //only enable adding or deleting of pools when the application is NOT in play state
                    if (!Application.isPlaying) //During Editor
                    {
                        CKY_PoolManager_EditorUtility.BeginColor(CKY_PoolManager_EditorAssets.addBtnColor);
                        if (GUILayout.Button("Add", EditorStyles.toolbarButton, GUILayout.Width(32)))
                        {
                            i_ToInsertAt = i + 1;
                        }
                        CKY_PoolManager_EditorUtility.EndColor();

                        CKY_PoolManager_EditorUtility.BeginColor(CKY_PoolManager_EditorAssets.delBtnColor);
                        if (GUILayout.Button("Del", EditorStyles.toolbarButton, GUILayout.Width(32)))
                        {
                            i_ToRemove = i;
                        }
                        CKY_PoolManager_EditorUtility.EndColor();
                    }

                    EditorGUILayout.EndHorizontal();

                    if (prefabPoolOption.isPoolExpanded)
                    {
                        EditorGUI.indentLevel = 1;

                        CKY_PoolManager_EditorUtility.DrawTexture(CKY_PoolManager_EditorAssets.poolItemTop);

                        EditorGUILayout.BeginHorizontal();
                        GUILayout.Space(12); //offset the prefab preview towards the center

                        Texture prefabPreviewIcon = null;
                        if (prefabPoolOption.prefabTransform && prefabPoolOption.prefabTransform.GetComponent<Renderer>() != null)
                        {
                            prefabPreviewIcon = AssetPreview.GetAssetPreview(prefabPoolOption.prefabTransform.gameObject);
                        }
                        else
                        {
                            prefabPreviewIcon = CKY_PoolManager_EditorUtility.LoadTexture(CKY_PoolManager_EditorAssets.missingPrefabIcon);
                        }

                        while (AssetPreview.IsLoadingAssetPreviews())
                        {
                            if (prefabPoolOption.prefabTransform)
                            {
                                prefabPreviewIcon = AssetPreview.GetAssetPreview(prefabPoolOption.prefabTransform.gameObject);
                            }
                            else
                            {
                                prefabPreviewIcon = CKY_PoolManager_EditorUtility.LoadTexture(CKY_PoolManager_EditorAssets.missingPrefabIcon);
                            }

                            System.Threading.Thread.Sleep(5);
                        }

                        CKY_PoolManager_EditorUtility.DrawTexture(prefabPreviewIcon, 100, 100);

                        EditorGUILayout.BeginVertical(GUILayout.MinHeight(prefabPreviewIcon.height));


                        if (!Application.isPlaying) //During Editor
                        {
                            prefabPoolOption.prefabTransform = (Transform)EditorGUILayout.ObjectField("Prefab", prefabPoolOption.prefabTransform, typeof(Transform), false);
                            prefabPoolOption.showDebugLog = EditorGUILayout.Toggle("Show Debug Log", prefabPoolOption.showDebugLog);
                            prefabPoolOption.instancesToPreload = EditorGUILayout.IntSlider("Preload Qty", prefabPoolOption.instancesToPreload, 0, 10000);


                            prefabPoolOption.poolCanGrow = EditorGUILayout.Toggle("Allow Pool to grow", prefabPoolOption.poolCanGrow);

                            if (prefabPoolOption.instancesToPreload == 0 && !prefabPoolOption.poolCanGrow)
                            {
                                EditorGUILayout.LabelField("*Preload Qty is 0 and Pool cannot grow. This pool will not be created.", EditorStyles.miniLabel);
                            }

                            if (prefabPoolOption.poolCanGrow)
                            {
                                prefabPoolOption.enableHardLimit = EditorGUILayout.Toggle("Enable Hard Limit", prefabPoolOption.enableHardLimit);

                                if (prefabPoolOption.enableHardLimit)
                                {
                                    prefabPoolOption.hardLimit = EditorGUILayout.IntSlider("Hard Limit", prefabPoolOption.hardLimit, 0, 10000);
                                }
                            }

                            prefabPoolOption.cullDespawned = EditorGUILayout.Toggle("Cull Despawned", prefabPoolOption.cullDespawned);

                            if (prefabPoolOption.cullDespawned)
                            {
                                prefabPoolOption.cullAbove = EditorGUILayout.IntSlider("Cull Above", prefabPoolOption.cullAbove, 0, 1000);
                                prefabPoolOption.cullDelay = EditorGUILayout.Slider("Cull Delay", prefabPoolOption.cullDelay, 0, 100);
                                prefabPoolOption.cullAmount = EditorGUILayout.IntSlider("Cull Amt", prefabPoolOption.cullAmount, 0, 100);
                            }

                            prefabPoolOption.recycle = EditorGUILayout.Toggle("Allow Pool to recycle", prefabPoolOption.recycle);
                        }
                        else //During Play mode
                        {
                            if (prefabPoolOption.prefabTransform != null)
                            {
                                var itemInfo = CKY_PoolManager.GetPool(name);
                                if (itemInfo != null)
                                {
                                    var spawnedCount = itemInfo.spawnedList.Count;
                                    var despawnedCount = itemInfo.despawnedList.Count;
                                    EditorGUILayout.LabelField(string.Format("{0} / {1} Spawned", spawnedCount, despawnedCount + spawnedCount), EditorStyles.boldLabel);
                                    EditorGUILayout.Separator();
                                }
                            }

                            EditorGUILayout.LabelField("Preload Qty : " + prefabPoolOption.instancesToPreload, EditorStyles.miniLabel);
                            EditorGUILayout.LabelField("Allow Pool to grow : " + prefabPoolOption.poolCanGrow, EditorStyles.miniLabel);

                            if (prefabPoolOption.poolCanGrow)
                            {
                                if (prefabPoolOption.enableHardLimit)
                                {
                                    EditorGUILayout.LabelField("Hard Limit : " + prefabPoolOption.hardLimit, EditorStyles.miniLabel);
                                }
                            }

                            if (prefabPoolOption.cullDespawned)
                            {
                                EditorGUILayout.LabelField("Cull Above : " + prefabPoolOption.cullAbove, EditorStyles.miniLabel);
                                EditorGUILayout.LabelField("Cull Delay : " + prefabPoolOption.cullDelay, EditorStyles.miniLabel);
                                EditorGUILayout.LabelField("Cull Amt : " + prefabPoolOption.cullAmount, EditorStyles.miniLabel);
                            }

                            EditorGUILayout.LabelField("Allow Pool to recycle : " + prefabPoolOption.recycle, EditorStyles.miniLabel);

                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.EndHorizontal();

                        CKY_PoolManager_EditorUtility.DrawTexture(CKY_PoolManager_EditorAssets.poolItemBottom);
                    }
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();

                if (i_ToRemove != -1)
                {
                    poolManager.prefabPoolOptions.RemoveAt(i_ToRemove);
                    isDirty = true;
                }
                if (i_ToInsertAt != -1)
                {
                    poolManager.prefabPoolOptions.Insert(i_ToInsertAt, new CKY_PrefabPoolOption());
                    isDirty = true;
                }
                if (i_ToShiftUp != -1)
                {
                    var item = poolManager.prefabPoolOptions[i_ToShiftUp];
                    poolManager.prefabPoolOptions.Insert(i_ToShiftUp - 1, item);
                    poolManager.prefabPoolOptions.RemoveAt(i_ToShiftUp + 1);
                    isDirty = true;
                }

                if (i_ToShiftDown != -1)
                {
                    var index = i_ToShiftDown + 1;
                    var item = poolManager.prefabPoolOptions[index];
                    poolManager.prefabPoolOptions.Insert(index - 1, item);
                    poolManager.prefabPoolOptions.RemoveAt(index + 1);
                    isDirty = true;
                }
            }



            if (GUI.changed || isDirty)
            {
                EditorUtility.SetDirty(target);
            }

            this.Repaint();
        }
    }
}