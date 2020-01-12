//  Copyright (c) 2016-2017 amlovey
//  
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System;
using System.Threading;

#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
#endif

#if UNITY_5_3_OR_NEWER
using UnityEditor.SceneManagement;
#endif


namespace APlusFree
{
    public class APCache
    {
        public const string LOAD_FROM_LOCAL_KEY = "LOAD_FROM_LOCAL_KEY";
        static string ASSETS_PATH = Path.Combine(Application.persistentDataPath, "A+AssetsExplorerFree.cache");
        static string ICON_CACHE_PATH = Path.Combine(Application.persistentDataPath, "A+CacheIcon.cache");

        // data cache to improve preformance
        //
        private static Dictionary<int, Dictionary<string, APAsset>> AssetsCache = new Dictionary<int, Dictionary<string, APAsset>>();
        private static Dictionary<string, string> IconCache = new Dictionary<string, string>();

        /// <summary>
        /// Load cache data
        /// </summary>
        public static void LoadDataIntoCache(System.Action callback = null)
        {
            if (EditorApplication.isPlaying)
            {
                EditorUtility.DisplayDialog("A+ Assets Explorer", "Refresh cache operation is not allowed in play mode", "OK");
                return;
            }

            EditorCoroutine.StartCoroutine(LoadDataIntoCacheCoroutine(callback));
        }

        private static IEnumerator LoadDataIntoCacheCoroutine(System.Action callback = null)
        {
            try
            {
                // Fix bug that Assets will be unloaded when refresh cache data.
                // -----
                // Resources.UnloadAsset() will unload asset in memory, objects loaded 
                // in current Scene will be unloaded too. that's not the correct way and 
                // will crash Unity Editor in some case. (Bug reported by Pete Rivett-Carnac)
                //
#if UNITY_5_3_OR_NEWER && UNITY_EDITOR
                var currentScene = EditorSceneManager.GetActiveScene();
                if (currentScene.isDirty)
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                }

                string currentScenePath = currentScene.path;
                if (!EditorApplication.isPlaying)
                {
                    EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
                }
#else
                string currentScene = EditorApplication.currentScene;
                if (EditorApplication.isSceneDirty)
                {
                    EditorApplication.SaveCurrentSceneIfUserWantsTo();
                }

                EditorApplication.NewScene();
#endif
                float totalCategoriesCount = 2f;
                ShowProcessBar(0);
                LoadResourcesIntoCache(APAssetType.Texture);
                ShowProcessBar(1.0f / totalCategoriesCount);
                LoadResourcesIntoCache(APAssetType.MovieTexture);

                if (IsShowProcessBarState())
                {
                    Debug.Log("A+ Assets Explorer cache data was created.");
                }

                EditorPrefs.DeleteKey(LOAD_FROM_LOCAL_KEY);
                SaveToLocal();
                ShowProcessBar(.0f / totalCategoriesCount);
                System.GC.Collect();
                ShowProcessBar(1);

                if (callback != null)
                {
                    callback.Invoke();
                }

#if UNITY_5_3_OR_NEWER
                if (!string.IsNullOrEmpty(currentScenePath))
                {
                    EditorSceneManager.OpenScene(currentScenePath, OpenSceneMode.Single);
                }
#else
                if (!string.IsNullOrEmpty(currentScene))
                {
                    EditorApplication.OpenScene(currentScene);
                }
#endif
            }
            catch
            {
                throw;
            }
            finally
            {
                // Need to clear progress bar whether load cache successs
                // or not
                //
                EditorUtility.ClearProgressBar();
            }

            yield return Resources.UnloadUnusedAssets();
        }

        public static void SaveToLocal()
        {
            SaveToLocal(ASSETS_PATH, AssetsCache);
            SaveToLocal(ICON_CACHE_PATH, IconCache);
        }

        private static EditorCoroutine saveAsync;
        public static void SaveToLocalAsync()
        {
            if (saveAsync != null)
            {
                saveAsync.Stop();
            }
            
            saveAsync = EditorCoroutine.StartCoroutine(SaveToLocalAsyncCortinue());
        }

        private static IEnumerator SaveToLocalAsyncCortinue()
        {
            SaveToLocal();
            yield return null;
        }

        public static bool LoadFromLocal()
        {
            LoadFromLocal<Dictionary<string, string>>(ICON_CACHE_PATH, ref IconCache);
            bool result = LoadFromLocal<Dictionary<int, Dictionary<string, APAsset>>>(ASSETS_PATH, ref AssetsCache);
            if (result)
            {
                LoadResourcesIntoCache(APAssetType.Blacklist);
            }

            return result;
        }

        private static void SaveToLocal(string filePath, object data)
        {
            System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter serializer = new BinaryFormatter();
                    serializer.Serialize(stream, data);

                    byte[] bytes = new byte[stream.Length];

                    // Have to reset the stream Position to 0 to ensure read real bytes
                    //
                    stream.Position = 0;
                    stream.Read(bytes, 0, bytes.Length);
                    var compressBytes = CLZF2.Compress(bytes);
                    fileStream.Write(compressBytes, 0, compressBytes.Length);
                }
            }
        }

        private static bool LoadFromLocal<T>(string filePath, ref T data) where T : class
        {
            System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");

            try
            {
                if (File.Exists(filePath))
                {
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] fileBytes = new byte[fileStream.Length];
                        fileStream.Read(fileBytes, 0, fileBytes.Length);
                        var realContentBytes = CLZF2.Decompress(fileBytes);
                        using (MemoryStream memoryStream = new MemoryStream(realContentBytes))
                        {
                            BinaryFormatter serializer = new BinaryFormatter();

                            try
                            {
                                data = serializer.Deserialize(memoryStream) as T;
                            }
                            catch
                            {
                                // Debug.LogError(e);
                                return false;
                            }

                            if (AssetsCache == null)
                            {
                                return false;
                            }

                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsShowProcessBarState()
        {
            return !EditorApplication.isPlayingOrWillChangePlaymode
                && !EditorApplication.isPlaying
                && !EditorApplication.isPaused
                && !EditorPrefs.HasKey(LOAD_FROM_LOCAL_KEY);
        }

        private static void ShowProcessBar(float progress)
        {
            if (IsShowProcessBarState())
            {
                string tile = "A+ Assets Explorer";
                string info = "A+ is creating cache data...";
                EditorUtility.DisplayProgressBar(tile, info, progress);
            }
        }

        public static void UpdateUsedStatus(HashSet<string> usedFiles)
        {
            SetAllAssetsToUnUsed();

            // check the assets in :
            //  1. the build report
            //  2. the enabled scenes
            //  3. the AssetBundles
            //  4. the PlayerSettings
            //
            UpdateUnusedStatusInternal(usedFiles);
            UpdateUsedStatusFromScene();
            UpdateUsedStatusFromAssetBundle();
            UpdateUnusedForProjectSettings();
        }

        public static void SetAllAssetsToUnUsed()
        {
            foreach (var assetDict in AssetsCache.Values)
            {
                foreach (var keyVal in assetDict)
                {
                    keyVal.Value.Used = false;
                }
            }
        }

        public static void UpdateUnusedForProjectSettings()
        {
            HashSet<string> unusedAssetsSet = new HashSet<string>();
            var textures = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown);
            foreach (var tex in textures)
            {
                var path = AssetDatabase.GetAssetPath(tex);
                if (!string.IsNullOrEmpty(path))
                {
                    unusedAssetsSet.Add(path);
                }
            }

#if UNITY_5_5_OR_NEWER
            if (PlayerSettings.defaultCursor != null)
            {
                unusedAssetsSet.Add(AssetDatabase.GetAssetPath(PlayerSettings.defaultCursor));
            }
#endif

            UpdateUnusedStatusInternal(unusedAssetsSet);
        }

        private static void UpdateUnusedStatusInternal(HashSet<string> usedFiles, bool includedInAssetBundle = false)
        {
            Utility.DebugLog("UpdateUnusedStatusInternal");
            foreach (var assetDict in AssetsCache.Values)
            {
                foreach (var keyVal in assetDict)
                {
                    if ((keyVal.Value.Path.Contains("/Resources/") ||
                        Utility.IsStreamingAssetsFile(keyVal.Value.Path)) && !includedInAssetBundle)
                    {
                        keyVal.Value.Used = true;
                    }
                    else
                    {
                        string filePath = keyVal.Value.Path.Replace('\\', '/');

                        // if it's the asset bundle assets set, we just set InAssetBundle to True
                        //
                        if (includedInAssetBundle)
                        {
                            keyVal.Value.InAssetBundle = usedFiles.Contains(filePath);
                        }
                        else
                        {
                            if (usedFiles.Contains(filePath))
                            {
                                keyVal.Value.Used = true;
                            }
                        }
                    }

                    Utility.UpdateJsonInAsset(keyVal.Value);
                }
            }
        }

        private static void UpdateUsedStatusFromScene()
        {
            var ScenesList = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
            var unusedAssets = AssetDatabase.GetDependencies(ScenesList);
            HashSet<string> unusedAssetsSet = new HashSet<string>();

            foreach (var path in unusedAssets)
            {
                unusedAssetsSet.Add(path);
            }

            UpdateUnusedStatusInternal(unusedAssetsSet);
        }

        public static void UpdateUsedStatusFromAssetBundle()
        {
            var assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            HashSet<string> pathSet = new HashSet<string>();
            foreach (var name in assetBundleNames)
            {
                var paths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPathsFromAssetBundle(name));
                foreach (var path in paths)
                {
                    pathSet.Add(path);
                }
            }

            UpdateUnusedStatusInternal(pathSet, true);
        }

        public static void ReloadCache(System.Action callback = null)
        {
            AssetsCache.Clear();
            IconCache.Clear();
            LoadDataIntoCache(callback);
        }

        public static void MoveTo(string assetid, string newPath)
        {
            foreach (var KeyValue in AssetsCache)
            {
                if (KeyValue.Value.ContainsKey(assetid))
                {
                    var asset = KeyValue.Value[assetid];
                    asset.Path = newPath;
                    asset.Used = null;
                    Utility.UpdateJsonInAsset(asset);

                    KeyValue.Value[assetid] = asset;
                    break;
                }
            }
        }

        public static void Remove(string assetid)
        {
            foreach (var KeyValue in AssetsCache)
            {
                if (KeyValue.Value.ContainsKey(assetid))
                {
                    KeyValue.Value.Remove(assetid);
                    break;
                }
            }
        }

        public static void Remove(APAssetType category, string assetid)
        {
            if (HasAsset(category, assetid))
            {
                AssetsCache[(int)category].Remove(assetid);
            }
        }

        public static APAsset GetValue(string assetid)
        {
            foreach (var keyVaue in AssetsCache)
            {
                if (keyVaue.Value.ContainsKey(assetid))
                {
                    return keyVaue.Value[assetid];
                }
            }

            return null;
        }

        public static APAsset GetValue(APAssetType category, string assetid)
        {
            if (HasAsset(category, assetid))
            {
                return AssetsCache[(int)category][assetid];
            }
            else
            {
                return null;
            }
        }

        public static void Add(APAssetType category, string assetid, APAsset value)
        {
            Utility.UpdateJsonInAsset(value);
            SetValue(category, assetid, value);
        }

        public static bool HasCategory(APAssetType category)
        {
            return AssetsCache.ContainsKey((int)category);
        }

        public static bool HasAsset(string assetid)
        {
            foreach (var keyVaue in AssetsCache)
            {
                if (keyVaue.Value.ContainsKey(assetid))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAsset(APAssetType category, string assetid)
        {
            if (HasCategory(category))
            {
                return AssetsCache[(int)category].ContainsKey(assetid);
            }

            return false;
        }

        public static void SetValue(APAssetType category, string assetid, APAsset value)
        {
            Utility.UpdateJsonInAsset(value);
            if (HasCategory(category))
            {
                if (AssetsCache[(int)category].ContainsKey(assetid))
                {
                    AssetsCache[(int)category][assetid] = value;
                }
                else
                {
                    AssetsCache[(int)category].Add(assetid, value);
                }
            }
            else
            {
                var assetDict = new Dictionary<string, APAsset>();
                assetDict.Add(assetid, value);
                AssetsCache.Add((int)category, assetDict);
            }
        }

        public static List<T> GetAssetsListByTypeFromCache<T>(APAssetType type) where T : class
        {
            if (HasCategory(type))
            {
                List<T> data = new List<T>();
                foreach (var item in AssetsCache[(int)type].Values)
                {
                    data.Add(item as T);
                }

                return data;
            }
            else
            {
                return null;
            }
        }

        public static string GetAssetsLitJsonByTypeFromCache(APAssetType type)
        {
            if (HasCategory(type))
            {
                return GetJsonFromList(AssetsCache[(int)type].Values);
            }
            else
            {
                return "[]";
            }
        }

        public static string GetJsonFromList(IEnumerable<APAsset> assets)
        {
            if (assets == null || assets.Count() == 0)
            {
                return "[]";
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (var item in assets)
            {
                if (item != null && !string.IsNullOrEmpty(item.Json))
                {
                    sb.Append(item.Json);
                    sb.Append(",");
                }
            }

            if (sb.Length > 1)
            {
                sb = sb.Remove(sb.Length - 1, 1);
            }

            sb.Append("]");
            return sb.ToString();
        }

        private static void LoadResourcesIntoCache(APAssetType type)
        {
            IList assets = null;
            Dictionary<string, APAsset> dict = new Dictionary<string, APAsset>();

            switch (type)
            {
                case APAssetType.Texture:
                    assets = APResources.GetTextures();
                    break;
                case APAssetType.MovieTexture:
                    assets = APResources.GetMovies();
                    break;
            }

            if (assets == null)
            {
                return;
            }

            foreach (var item in assets)
            {
                var asset = item as APAsset;
                if (asset == null && string.IsNullOrEmpty(asset.Id))
                {
                    continue;
                }

                Utility.UpdateJsonInAsset(asset);

                if (!dict.ContainsKey(asset.Id))
                {
                    dict.Add(asset.Id, asset);
                }
                else
                {
                    dict[asset.Id] = asset;
                }
            }

            int key = (int)type;

            if (AssetsCache.ContainsKey(key))
            {
                AssetsCache[key] = dict;
            }
            else
            {
                AssetsCache.Add(key, dict);
            }
        }

        public static void AddIcon(string key, string data)
        {
            if (IconCache.ContainsKey(key))
            {
                IconCache[key] = data;
            }
            else
            {
                IconCache.Add(key, data);
            }
        }

        public static string GetIconCacheJSON()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            foreach (var keyVal in IconCache)
            {
                sb.Append(string.Format("\"{0}\":\"{1}\",", keyVal.Key, Utility.SafeJson(keyVal.Value)));
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("}");

            return sb.ToString();
        }
    }
}
#endif