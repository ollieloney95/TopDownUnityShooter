#if UNITY_EDITOR
//  Copyright (c) 2016 amlovey
//  
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;
using System;

namespace APlusFree
{
    public class DataExporter
    {
        enum DataType
        {
            CSV,
        }

        [MenuItem("Tools/A+ Assets Explorer(Free)/Data Explorer/Export as CSV...", false, 33)]
        public static void ExportToCSV()
        {
            string title = "Export to CSV";
            SaveDataWithDialog(title, DataType.CSV);
        }

        private static void SaveDataWithDialog(string title, DataType type)
        {
            string folderPath = EditorUtility.OpenFolderPanel(title, Application.dataPath, "");
            Debug.Log(folderPath);
            if (!string.IsNullOrEmpty(folderPath))
            {
                switch (type)
                {
                    case DataType.CSV:
                        SaveCSV(folderPath);
                        break;
                }
            }
        }

        private static void SaveCSV(string folderPath)
        {
            SaveToLocal(Path.Combine(folderPath, "textures.csv"), GetTextureCSV());

            string message = string.Format("Saved to folder {0}", folderPath);
            if(EditorUtility.DisplayDialog("Done!", message, "OK"))
            {
                EditorUtility.RevealInFinder(folderPath);
            }
        }

        private static void SaveToLocal(string filePath, string data)
        {
            File.WriteAllText(filePath, data);
        }

        private static string GetTextureCSV()
        {
            string header = "Name,FileSize,StorageSize,RuntimeSize,MaxSize,MipMap,ReadWrite,TextureFormat,TextureType,Width,Height,WidthInPixel,HeightInPixel,Used,Path";
            return GenerateCSV<APTexture>(header, APAssetType.Texture, texture =>
                string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14}",
                            texture.Name,
                            texture.FileSize,
                            texture.StorageSize,
                            texture.RuntimeSize,
                            texture.MaxSize,
                            texture.MipMap,
                            texture.ReadWrite,
                            texture.TextureFormat,
                            texture.TextureType,
                            texture.Width,
                            texture.Height,
                            texture.WidthInPixel,
                            texture.HeightInPixel,
                            texture.Used,
                            texture.Path)
            );

        }

        private static string GenerateCSV<T>(string header, APAssetType type, Func<T, string> rowDataGenerator) where T : APAsset
        {
            var dataSet = APCache.GetAssetsListByTypeFromCache<T>(type);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(header);
            foreach (var item in dataSet)
            {
                sb.AppendLine(rowDataGenerator(item));
            }

            return sb.ToString();
        }
    }
}
#endif
