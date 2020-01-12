#if UNITY_EDITOR
//  Copyright (c) 2016 amlovey
//  
namespace APlusFree
{
    using System;
    using UnityEditor;
    using UnityEngine;
    using System.Collections.Generic;

#if UNITY_5_5_OR_NEWER
    using UnityEngine.Profiling;
#endif

    public class APResources
    {
        public static string[] GetAssetGuidsByType(APAssetType type)
        {
            return AssetDatabase.FindAssets(string.Format("t:{0}", type.ToString()));
        }

        public static APAsset GetAPAssetByPath(APAssetType type, string guid)
        {
            switch (type)
            {
                case APAssetType.Texture:
                    return GetAPTextureFromAssetGuid(guid);
                case APAssetType.MovieTexture:
                    return GetAPMovieTextureFromAssetGuid(guid);
                default:
                    return null;
            }
        }

        public static List<T> GetResourcesListByType<T>(APAssetType type, Func<string, T> parseFunction) where T : class
        {
            var textGuids = GetAssetGuidsByType(type);
            List<T> list = new List<T>();
            for (int i = 0; i < textGuids.Length; i++)
            {
                T obj = parseFunction(textGuids[i]);
                if (obj != null)
                {
                    list.Add(obj);
                }
            }

            return list;
        }

        public static List<APMovieTexture> GetMovies()
        {
            return GetResourcesListByType<APMovieTexture>(APAssetType.MovieTexture, GetAPMovieTextureFromAssetGuid);
        }

        public static APMovieTexture GetAPMovieTextureFromAssetGuid(string guid)
        {
            MovieImporter movieImporter = GetAssetImporterFromAssetGuid<MovieImporter>(guid);

            // if texture is render texture or others, tImporter will to set to null.
            //
            if (movieImporter == null)
            {
                return null;
            }

            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            var texture = AssetDatabase.LoadAssetAtPath(path, typeof(MovieTexture)) as MovieTexture;
            if (texture == null)
            {
                return null;
            }

            APMovieTexture apMovieTexture = new APMovieTexture();
            apMovieTexture.Icon = GetIconID(path);

            apMovieTexture.Size = TextureUtillity.GetStorageMemorySize(texture);
            double approx = (GetVideoBitrateForQuality(movieImporter.quality) + GetAudioBitrateForQuality(movieImporter.quality)) * movieImporter.duration / 8;
            apMovieTexture.Approx = approx;

            apMovieTexture.Name = Utility.GetFileName(path);
            apMovieTexture.Path = path;
            apMovieTexture.Hash = Utility.GetFileMd5(path);
            apMovieTexture.Quality = movieImporter.quality;
            apMovieTexture.FileSize = Utility.GetFileSize(path);

            TimeSpan duration = TimeSpan.FromSeconds(texture.duration);
            apMovieTexture.Duration = Utility.GetTimeDurationString(duration);
            apMovieTexture.Id = guid;

            UnloadAsset(texture);
            return apMovieTexture;
        }
        private static double GetAudioBitrateForQuality(double f)
        {
            return 56000.0 + 200000.0 * f;
        }
        private static double GetVideoBitrateForQuality(double f)
        {
            return 100000.0 + 8000000.0 * f;
        }

        public static List<APTexture> GetTextures()
        {
            return GetResourcesListByType<APTexture>(APAssetType.Texture, GetAPTextureFromAssetGuid);
        }

        public static T GetAssetImporterFromAssetGuid<T>(string guid) where T : class
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            return AssetImporter.GetAtPath(path) as T;
        }

        private static bool InUseOrSelection(UnityEngine.Object obj)
        {
            // In selection
            //
            foreach (var selected in Selection.objects)
            {
                if (selected == obj)
                {
                    return true;
                }
            }

            return false;
        }

        public static void UnloadAsset(UnityEngine.Object obj)
        {
            if (InUseOrSelection(obj) || obj is GameObject || obj is Component || obj is AssetBundle)
            {
                obj = null;
                return;
            }

            Resources.UnloadAsset(obj);
            obj = null;
        }

        public static APTexture GetAPTextureFromAssetGuid(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var texture = AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object));
            if (texture == null
                || texture is MovieTexture)
            {
                return null;
            }

            APTexture apTexture = new APTexture();
            apTexture.Icon = GetIconID(path);
            if (texture is RenderTexture)
            {
                var renderTexture = texture as RenderTexture;
                apTexture.StorageSize = TextureUtillity.GetStorageMemorySize(renderTexture);
#if UNITY_5 && !UNITY_5_6
                apTexture.RuntimeSize = Profiler.GetRuntimeMemorySize(renderTexture);
#else
                apTexture.RuntimeSize = Profiler.GetRuntimeMemorySizeLong(renderTexture);
#endif
                apTexture.Width = renderTexture.width;
                apTexture.Height = renderTexture.height;
                apTexture.TextureType = "Render";
                apTexture.Path = path;
                apTexture.Hash = Utility.GetFileMd5(path);
                apTexture.Name = Utility.GetFileName(path);
                apTexture.FileSize = Utility.GetFileSize(path);
                apTexture.Id = guid;
                UnloadAsset(texture);
                return apTexture;
            }

            TextureImporter tImporter = GetAssetImporterFromAssetGuid<TextureImporter>(guid);
            if (tImporter == null)
            {
                return null;
            }

            var tex = texture as Texture;

            int maxTextureSize = tImporter.maxTextureSize;
#if UNITY_5_5_OR_NEWER
            TextureImporterFormat importerFormat;
            TextureImporterCompression importerCompression = tImporter.textureCompression;
#else
            TextureImporterFormat importerFormat = tImporter.textureFormat;
#endif
            int compressQuality = tImporter.compressionQuality;

            // Get texture settings for different platform
            //
            tImporter.GetPlatformTextureSettings(Utility.BuildTargetToPlatform(EditorUserBuildSettings.activeBuildTarget), out maxTextureSize, out importerFormat, out compressQuality);
            apTexture.StorageSize = TextureUtillity.GetStorageMemorySize(tex);
            apTexture.RuntimeSize = TextureUtillity.GetRuntimeMemorySize(tex);
            apTexture.Name = Utility.GetFileName(path);
            apTexture.ReadWrite = tImporter.isReadable;
#if UNITY_5_5_OR_NEWER
            if ((int)importerFormat > 0)
            {
                apTexture.TextureFormat = importerFormat.ToString();
            }
            else
            {
                apTexture.TextureFormat = "Auto";
            }
#else
            apTexture.TextureFormat = importerFormat.ToString();
#endif
            apTexture.TextureType = tImporter.textureType.ToString();
            apTexture.Path = path;
            apTexture.Hash = Utility.GetFileMd5(path);
            apTexture.MipMap = tImporter.mipmapEnabled;
            apTexture.MaxSize = maxTextureSize;
            apTexture.Width = tex.width;
            apTexture.Height = tex.height;
            apTexture.FileSize = Utility.GetFileSize(path);
            int widthInPixel = 0;
            int heightInPixel = 0;
            TextureUtillity.GetImageSize(texture as Texture2D, out widthInPixel, out heightInPixel);

            apTexture.WidthInPixel = widthInPixel;
            apTexture.HeightInPixel = heightInPixel;

            apTexture.Id = guid;

            UnloadAsset(texture);

            return apTexture;
        }


        public static string GetIconID(string assetPath, bool isAnimation = false)
        {
            if (isAnimation)
            {
                var iconData1 = "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";
                var key1 = Utility.MD5(iconData1);
                APCache.AddIcon(key1, iconData1);
                return key1;
            }

            var texture = AssetDatabase.GetCachedIcon(assetPath);
            if (texture == null)
            {
                return GetNullKey();
            }

            var format = (texture as Texture2D).format;

            if (!IsSupportedFormat(format))
            {
                return GetNullKey();
            }

            var newTexture = new Texture2D(texture.width, texture.height, (texture as Texture2D).format, false);
            newTexture.LoadRawTextureData((texture as Texture2D).GetRawTextureData());

            var bytes = new byte[0];

            try
            {
                bytes = newTexture.EncodeToPNG();
            }
            catch
            {
                return GetNullKey();
            }

            if (bytes.Length > 0)
            {
                var iconData = System.Convert.ToBase64String(newTexture.EncodeToPNG());
                var key = Utility.MD5(iconData);
                APCache.AddIcon(key, iconData);
                return key;
            }
            else
            {
                return GetNullKey();
            }
        }

        private static bool IsSupportedFormat(TextureFormat format)
        {
            // Supported float for PNG export: ARGB32, RGB32, RGB24, Aplha8 or one of float formats
            // 
            return format == TextureFormat.ARGB32
                   || format == TextureFormat.RGBA32
                   || format == TextureFormat.RGB24
                   || format == TextureFormat.Alpha8
                   || format.ToString().Contains("Float");
        }

        private static string GetNullKey()
        {
            var nullKey = "AA625B9BDB3AE0E4";
            var nullIcon = "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAFBUlEQVRYCdVXa0wcVRT+dvbN7rK0LE8tVBpJTLU/tBFC+OEPhURrfCQ1aSXRxkQwFZOKMdUmNYZqgqbBEEvBV4zGlNYfNlGDIGgULNrq1opoghEIBDbyKrvssju7OzOeMzC8dtqFJU30JGfuuXfuPee7555z5o4BiSTU1dWdThxeGVEUBQMDA+MdHR1v0qhILK+83Zxk0JmeMTc3d1VnHGyYWZZlSJKEeDyOcDiM6elplJaWGmnNpoHoAiClSQFoIKLRqArC5/OhrKxs0yBMejs1mXSHlz3AaxgAe4CJvZKVlYXJyUkpOzubF0vqiw08BL05giDgWmw0GtV33DJQbs1mM6xWK1wuF6amphiV/g50jOkCMBgM0GMGpY2zrIFgmcFYLBY4nU4GESNbZh17CUObArDaOMuaYfYAM/c1EBSYUbJmSbC4bmDDruJgY4rFYur5MwAtIzgWIpEIBgcHEQqFEAwG1cwgEKLH47HTsoi6WOehmwWkeE0WsIGv3zuO/Iw4Pursxf6yvfhrYggfnr2Cintuwvc/jeL5w4cwO34FcOyGYDejrv4k2ru8mJ+fR0lJiYNsL+jYx4YA8K6feOB27CjMRVG+G5+cu4jvBiZAAaF6of7cGDKz8sgzccTFBVh+roEjehX7jrSpHmOvFBcX83FwbKyh6wJgF7OrNWrqilCwWdguIVewGEDykkw1yKBQfkqIdj0DQZnBwaNn1YLFG+AjKigoWFG2pDRpDDR1BmGz2VQjDiuloEEiebHgCdQyEKoEBIZZhiIoaL9wCQfv360GJmcKMwcnEWNeUy11s4BnMvHun6twwkoxZDWIFNIibEIUNmMcaaYYsYQ0MzH1LTTHIodhkxfQ1vk77TikpqaWnksArIuaV57XBaBNSzMrZEiB06LATkbtJplASLCSLMgihHgIl/t9cGEEJ448hNKyclzw71U3sLpekL6EtEx6BAzCZmavyYhJClpbf8X0yC/kdRNM8gwkIR1Gu4CIrw25jz+M2oYP8G1zPb7p/BINd1TgaFU5q1CDlZqEGEgYoEkZq9NwaGgIj1a9A7MkwpBxM6RwACazHYJZxH1V+1B5dw5ciojuzs+x885KDP/Qh8eePoB3G0+hq/s8ent62L5K5I1tJMwtddUmKYBAIID91R/D4XbgpZcPIEyV3kUBpRCgsWgA2+UIFkJR7MpxIduThZgiofGV1/FI9VPI3+ZCjtulHgXHE3ECgKQx4HBQDZn0Ykd2DkZDIooyLch0K/D5w7jNZoYjFsF4/48403QCf7Y3o/bBPZgb6cH5k8fgou/C+lRevXuWk3qAJ/0z40f/rB0FGUYcf7IczZ/2YoLKrX8+iJ62M7joD2KP24K/L/fBmeGBNTyLW2/xQLJtR82Lr2kpmJoHGEBOphtTaSF8deoFFOWmY7zvfXzRcBjDnzXiN28fKnflQYjOI9OTjsDsBNVcCX0DY1STYmoNYB3Xog15gBf7RoeQV1Ck6vF6vbDb7cjNzVXvAJzrr77xFuaGvQgHpiErBqQ7HdhZWIhnj/G1cZH0YmBDAERRXP7casrWt3zWraffxh+Xuqn8yrir7F4cqq5VA1CbmzIATcFWWz0ASbNgq0aTrf//AaipqYHf71/eGPe3Qil5wO12L9tsaWnhS+hyf7NCSgDWG+F/glRpQ2mYqvL16/6TWaDnAb4j8FfrRhDfthf/526E9lR0/gs8uBE8u4t8mgAAAABJRU5ErkJggg==";
            APCache.AddIcon(nullKey, nullIcon);
            return nullKey;
        }
    }
}
#endif
