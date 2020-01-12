#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace APlusFree
{
    public class PreferencesItems
    {
        #region Window Style Config
        private const string DockableWindowKey = "A+ASSETSEXPLORER_DOCKABLEWINDOWS";
        public static bool IsDockableWindowStyle
        {
            get
            {
                return EditorPrefs.GetBool(DockableWindowKey, false);
            }
            set
            {
                EditorPrefs.SetBool(DockableWindowKey, value);
            }
        }

        private static void DockableStyleWindowUI()
        {
            string itemText = "Use dockable style window";
            string toolTip = "Use dockable style window";
            IsDockableWindowStyle = EditorGUILayout.Toggle(new GUIContent(itemText, toolTip), IsDockableWindowStyle);
        }
        #endregion

        private const string AutoRefreshCacheOnProjectLoadKey = "A+ASSETSEXPLORER_AUTOREFRESHCACHEONLOAD";
        public static bool AutoRefreshCacheOnProjectLoad
        {
            get
            {
                return EditorPrefs.GetBool(AutoRefreshCacheOnProjectLoadKey, false);
            }
            set
            {
                EditorPrefs.SetBool(AutoRefreshCacheOnProjectLoadKey, value);
            }
        }

        private static void AutoRefreshCacheOnProjectLoadUI()
        {
            string itemText = "Creating cache automatically";
            string toolTip = "Creating cache automatically on project launch";
            AutoRefreshCacheOnProjectLoad = EditorGUILayout.Toggle(new GUIContent(itemText, toolTip), AutoRefreshCacheOnProjectLoad);
        }

        [PreferenceItem("Assets Explorer")]
        public static void PreferencesItemUI()
        {
            EditorGUILayout.Space();
            AutoRefreshCacheOnProjectLoadUI();
            DockableStyleWindowUI();
        }
    }
}

#endif