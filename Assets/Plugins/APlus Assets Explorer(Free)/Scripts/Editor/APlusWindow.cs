//  Copyright (c) 2016 amlovey
//  
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace APlusFree
{
    [SerializableAttribute]
    public class APlusWindow : EditorWindow, IHasCustomMenu
    {
        static PropertyInfo getDocked;
        static APlusWindow()
        {
            getDocked = typeof(EditorWindow).GetProperty("docked", ReflectionUtils.BIND_FLAGS);
        }

        public static bool GetDocked(EditorWindow window)
        {
            var obj = getDocked.GetValue(window, null);
            return bool.Parse(obj.ToString());
        }

        public bool InDockableWindowStyle;
        private const float titleHeight = 0;
        public Webview webview;
        private const string EditorHTMLFile = "index.html";
        private const string TITLE = "A+ Assets Explorer(Free)";

        public static APlusWindow Instance = null;

#if APLUS_DEV
        private static bool useServerMode = false;
#endif

        [MenuItem("Tools/A+ Assets Explorer(Free)/Assets Explorer", false, 11)]
        [MenuItem("Assets/A+ Assets Explorer(Free)", false, 111)]
        public static void load()
        {
            if (Instance != null)
            {
                return;
            }

            ClearPrevoiusURL();

            if (!PreferencesItems.IsDockableWindowStyle)
            {
                Instance = GetWindow<APlusWindow>(true, TITLE, true);
                Instance.InitWebView(GUIClip.Unclip(new Rect(0, 0, Instance.position.x, Instance.position.y)));
            }
            else
            {
                Type[] desiredDockNextTo = new Type[] { typeof(SceneView) };
                Instance = EditorWindow.GetWindow<APlusWindow>(TITLE, desiredDockNextTo);
                Instance.SetMinMaxSizes();
                Instance.Show();
            }

            Instance.InDockableWindowStyle = PreferencesItems.IsDockableWindowStyle;
        }

        [MenuItem("Tools/A+ Assets Explorer(Free)/Find unused assets", false, 22)]
        public static void FindUnusedAssets()
        {
            string title = "Find unused files?";
            string message = "Press 'OK' to launch a build setting dialog to start a build.\r\n\r\n";
            if (EditorUtility.DisplayDialog(title, message, "OK", "Cancel"))
            {
                EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
                APCache.SaveToLocal();
                EditorPrefs.SetString(APCache.LOAD_FROM_LOCAL_KEY, APCache.LOAD_FROM_LOCAL_KEY);
            }
        }

        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Reload"), false, new GenericMenu.MenuFunction(this.Reload));
        }

        private void Reload()
        {
            if (webview != null)
            {
                webview.Reload();
            }
        }


        [MenuItem("Tools/A+ Assets Explorer(Free)/Refresh cache", false, 22)]
        public static void RefreshCache()
        {
            EditorPrefs.DeleteKey(APCache.LOAD_FROM_LOCAL_KEY);
            APCache.ReloadCache(() =>
            {
                AssetNotification.webCommunicationService.RefreshAll();
            });
        }

#if APLUS_DEV
        [MenuItem("Tools/A+ Assets Explorer(Free)/Toggle Server mode", false, 111)]
        public static bool ToggleServerMode()
        {
            useServerMode = !useServerMode;
            Menu.SetChecked("Tools/A+ Assets Explorer(Free)/Toggle Server mode", useServerMode);
            return true;
        }
#endif

        public void InitWebView(Rect webviewRect)
        {
            if (webview == null)
            {
                this.webview = ScriptableObject.CreateInstance<Webview>();
                this.webview.hideFlags = HideFlags.HideAndDontSave;
            }

            this.webview.InitWebView(Webview.GetView(this), webviewRect, false);
            AssetNotification.webCommunicationService.Init(this.webview);
            docked = GetDocked(this);
            SetMinMaxSizes();
            LoadEditor();
            SetFocus(true);
            Instance = this;
        }

        #region Load Editor

        private const string ASSETEXPLORER_PREVOIUS_URL = "A+ASSETEXPLORER_PREVOIUS_URL";
        void LoadEditor()
        {
            string path = string.Empty;
#if APLUS_DEV
            if (useServerMode)
            {
                path = "http://127.0.0.1:8080";
            }
            else
            {
                path = string.Format(@"file:///{0}/Plugins/APlus Assets Explorer(Free)/Window/{1}", Application.dataPath, EditorHTMLFile);
            }
#else 
            path = string.Format(@"file:///{0}/Plugins/APlus Assets Explorer(Free)/Window/{1}", Application.dataPath, EditorHTMLFile);
#endif

            if (EditorPrefs.HasKey(ASSETEXPLORER_PREVOIUS_URL))
            {
                path = EditorPrefs.GetString(ASSETEXPLORER_PREVOIUS_URL);
            }

            LoadURL(path);
        }

        public static void ClearPrevoiusURL()
        {
            EditorPrefs.DeleteKey(ASSETEXPLORER_PREVOIUS_URL);
        }

        public void LoadURL(string path)
        {
#if APLUS_DEV
#if UNITY_5_2 || UNITY_5_3 || UNITY_5_4
            this.webview.AllowRightClickMenu(true);
#endif
            if (useServerMode)
            {
                this.webview.LoadURL(path);
            }
            else
            {
                this.webview.LoadURL(path);
            }
#else
            this.webview.LoadURL(path);
#endif
        }

        #endregion

        #region Dockable Window Style
        public void OnBecameInvisible()
        {
            if (!this.InDockableWindowStyle)
            {
                return;
            }

            if (this.webview != null)
            {
                this.webview.SetHostView(null);
                this.webview.Hide();
                this.webview.SetFocus(false);
            }
        }

        private int repeatedShow;
        private bool syncingFocus;
        private void SetFocus(bool value)
        {
            if (!this.syncingFocus)
            {
                this.syncingFocus = true;
                if (this.webview != null)
                {
                    if (value)
                    {
                        this.webview.SetHostView(Webview.GetView(this));
                        this.webview.Show();
                        this.repeatedShow = 5;
                    }

                    this.webview.SetFocus(value);
                }
                this.syncingFocus = false;
            }
        }

        public void OnEnable()
        {
            // re-binding the webview Instance
            //
            if (Instance == null)
            {
                Instance = this;
                AssetNotification.webCommunicationService.Init(this.webview);
            }

            if (!string.IsNullOrEmpty(EditorPrefs.GetString(AssetNotification.PrepareOnLoad.AFTERBUILD_A_PLUS)))
            {
                AssetNotification.webCommunicationService.RefreshAll();
                EditorPrefs.DeleteKey(AssetNotification.PrepareOnLoad.AFTERBUILD_A_PLUS);
            }

            SetMinMaxSizes();
        }

        public void OnLostFocus()
        {
            this.SetFocus(false);
        }

        public void OnFocus()
        {
            SetFocus(true);
        }

        bool docked = false;

        private void SetMinMaxSizes()
        {
            if (InDockableWindowStyle)
            {
                base.minSize = new Vector2(860f, 300f);
                base.maxSize = new Vector2(2048f, 2048f);
            }
            else
            {
                base.minSize = new Vector2(1024, 300);
            }
        }

        void TipUI(float windowWith, float windowHeight)
        {
            string text = "Assets Explorer is Loading...";
            GUIStyle textStyle = new GUIStyle();
            textStyle.normal.background = null;
            textStyle.fontSize = 18;
            float width = 280;
            float height = 36;
            float x = (windowWith - width) / 2;
            float y = windowHeight / 2 - height;
            GUI.Label(new Rect(x, y, width, height), text, textStyle);
        }

        void OnGUI()
        {
            if (docked != GetDocked(this))
            {
                SetMinMaxSizes();
            }

            Rect webViewRect = GUIClip.Unclip(new Rect(0f, 0, base.position.width, base.position.height));

            TipUI(webViewRect.width, webViewRect.height);

            if (this.webview == null)
            {
                this.InitWebView(webViewRect);
            }

            if (this.repeatedShow-- > 0)
            {
                this.Refresh();
            }

            if (Event.current.type == EventType.Repaint && webview != null)
            {
                this.webview.SetSizeAndPosition(webViewRect);
            }

            CheckRefresh();
        }

        public void Refresh()
        {
            this.webview.Hide();
            this.webview.Show();
        }

        public void Destroy()
        {
            if (webview != null)
            {
                webview.OnDestory();
            }

            Instance = null;
            APCache.SaveToLocal();
        }

        void OnDestroy()
        {
            Destroy();
        }

        #endregion

        [PreferenceItem("Assets Explorer")]
        private static void PreferencesItem()
        {
            PreferencesItems.PreferencesItemUI();
        }

        private void CheckRefresh()
        {
            if (!string.IsNullOrEmpty(EditorPrefs.GetString(AssetNotification.PrepareOnLoad.AFTERBUILD_A_PLUS))
                && webview != null)
            {
                AssetNotification.webCommunicationService.RefreshAll();
                AssetNotification.webCommunicationService.Refresh();
                EditorPrefs.DeleteKey(AssetNotification.PrepareOnLoad.AFTERBUILD_A_PLUS);
            }
        }
    }
}
#endif