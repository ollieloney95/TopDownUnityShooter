//  Copyright (c) 2016-2017 amlovey
//  
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

namespace APlusFree
{
    public class AssetNotification : AssetPostprocessor
    {
        private static void HandleScriptsChange(string[] importedAssets, string[] deletedAssets, string[] movedAssets)
        {
            bool hasScriptAssets = false;

            string[] assets = new string[0];
            ArrayUtility.AddRange(ref assets, importedAssets);
            ArrayUtility.AddRange(ref assets, deletedAssets);
            ArrayUtility.AddRange(ref assets, movedAssets);

            foreach (var item in assets)
            {
                if (Utility.IsScriptAsset(item))
                {
                    hasScriptAssets = true;
                    break;
                }
            }

            if (hasScriptAssets)
            {
                APCache.SaveToLocal();
            }
        }

        private static void HanleImportedAssets(string[] importedAssets)
        {
            foreach (var assetPath in importedAssets)
            {
                var id = AssetDatabase.AssetPathToGUID(assetPath);

                if (!APCache.HasAsset(id))
                {
                    AddNewImportAssets(assetPath);
                }
                else
                {
                    UpdateReimportExistAssets(assetPath);
                }
            }

            webCommunicationService.RefreshIconCache();
        }

        private static void HandleDeletedAssets(string[] deletedAssets)
        {
            foreach (var assetPath in deletedAssets)
            {
                Utility.DebugLog(string.Format("Deleted: {0}", assetPath));
                var id = AssetDatabase.AssetPathToGUID(assetPath);
                var asset = APCache.GetValue(AssetDatabase.AssetPathToGUID(assetPath));
                if (asset != null)
                {
                    APCache.Remove(id);
                    SyncManager.DeleteAssets.Enqueue(asset);
                }
            }
        }

        private static void HandleMovedAssets(string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (var i = 0; i < movedAssets.Length; i++)
            {
                Utility.DebugLog(string.Format("moved {0} to {1}", movedFromAssetPaths[i], movedAssets[i]));
                var sid = AssetDatabase.AssetPathToGUID(movedAssets[i]);
               
                APCache.MoveTo(sid, movedAssets[i]);
                var asset = APCache.GetValue(sid);

                if (asset != null)
                {
                    Utility.DebugLog(asset.ToString());
                    SyncManager.MovedFromAssets.Enqueue(sid);
                    SyncManager.MovedToAssets.Enqueue(asset);
                }
            }
        }

        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            HanleImportedAssets(importedAssets);
            HandleDeletedAssets(deletedAssets);
            HandleMovedAssets(movedAssets, movedFromAssetPaths);
            HandleScriptsChange(importedAssets, deletedAssets, movedAssets);
        }

        private static void AddNewImportAssets(string assetPath)
        {
            Utility.DebugLog(string.Format("New: {0}", assetPath));

            if (!File.Exists(assetPath) && Directory.Exists(assetPath))
            {
                return;
            }

            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));

            APAsset asset = null;

            // if new path
            //
            if (obj is Texture)
            {
                if (obj is MovieTexture)
                {
                    var movie = APResources.GetAPAssetByPath(APAssetType.MovieTexture, guid);
                    if (movie != null)
                    {
                        APCache.SetValue(APAssetType.MovieTexture, movie.Id, movie);
                    }
                    SyncManager.AddedAssets.Enqueue(movie);
                    return;
                }

                asset = APResources.GetAPAssetByPath(APAssetType.Texture, guid);
                if (asset != null)
                {
                    APCache.SetValue(APAssetType.Texture, asset.Id, asset);
                }
            }

            if (asset != null)
            {
                SyncManager.AddedAssets.Enqueue(asset);
            }

            Resources.UnloadUnusedAssets();
        }

        private static void UpdateReimportExistAssets(string assetPath)
        {
            Utility.DebugLog(string.Format("Update: {0}", assetPath));

            var guid = AssetDatabase.AssetPathToGUID(assetPath);

            UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));

            if (obj is MovieTexture)
            {
                webCommunicationService.UpdateObjectsIntoCache(APAssetType.MovieTexture, guid, SyncManager.ModifiedAssets);
            }
            else if (obj is Texture)
            {
                webCommunicationService.UpdateObjectsIntoCache(APAssetType.Texture, guid, SyncManager.ModifiedAssets);
            }

            APResources.UnloadAsset(obj);
        }

        public static WebViewCommunicationService webCommunicationService;

        [InitializeOnLoadAttribute]
        public static class PrepareOnLoad
        {
            static PrepareOnLoad()
            {
                webCommunicationService = ScriptableObject.CreateInstance<WebViewCommunicationService>();
                webCommunicationService.hideFlags = HideFlags.HideAndDontSave;

                EditorApplication.update += BackgroundUpdate;
                EditorApplication.playmodeStateChanged += PlaymodeStateChanged;
                time = EditorApplication.timeSinceStartup;

                if (EditorPrefs.HasKey(APCache.LOAD_FROM_LOCAL_KEY)
                    || EditorApplication.isPlayingOrWillChangePlaymode
                    || EditorApplication.isCompiling
                    || EditorApplication.isPaused
                    || !PreferencesItems.AutoRefreshCacheOnProjectLoad)
                {
                    EditorPrefs.DeleteKey(APCache.LOAD_FROM_LOCAL_KEY);
                    if (!APCache.LoadFromLocal())
                    {
                        APCache.LoadDataIntoCache(CheckUnusedState);
                    }
                    else
                    {
                        CheckUnusedState();
                    }
                }
                else
                {
                    APCache.LoadDataIntoCache(CheckUnusedState);
                }
            }

            public const string AFTERBUILD_A_PLUS = "AFTERBUILD_A_PLUS";

            private static void CheckUnusedState()
            {
                Utility.DebugLog("CheckUnusedState");
                string key = AssetsUsageChecker.GetUniqueAssetCheckerKey();
                Utility.DebugLog("Checking key: " + key + ", with result = " + EditorPrefs.HasKey(key));
                if (EditorPrefs.HasKey(key))
                {
                    List<string> assets = AssetsUsageChecker.Check();

                    if (assets != null && assets.Count > 0)
                    {
                        HashSet<string> usedFiles = new HashSet<string>();
                        foreach (var item in assets)
                        {
                            usedFiles.Add(item);
                        }

                        APCache.UpdateUsedStatus(usedFiles);
                        APCache.SaveToLocal();
                        EditorPrefs.SetString(AFTERBUILD_A_PLUS, AFTERBUILD_A_PLUS);
                        EditorPrefs.DeleteKey(AssetsUsageChecker.GetUniqueAssetCheckerKey());
                    }
                }
            }

            private static void PlaymodeStateChanged()
            {
                if (EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    Utility.DebugLog("APCache.SaveToLocalAsync()");
                    APCache.SaveToLocalAsync();
                }
            }

            private static double time;
            private static void BackgroundUpdate()
            {
                TrackingWorkaround();
            }

            public const double INTERVAL = 1.5;
            private static void TrackingWorkaround()
            {
                if (EditorApplication.timeSinceStartup - time > INTERVAL)
                {
                    SyncManager.Process(webCommunicationService);
                }
            }
        }
    }
}

#endif