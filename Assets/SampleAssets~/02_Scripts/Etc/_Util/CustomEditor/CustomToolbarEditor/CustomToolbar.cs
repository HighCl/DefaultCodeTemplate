#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.SceneManagement;
using System.Linq;

[InitializeOnLoad]
public class CustomToolbar
{
    [InitializeOnLoadMethod]
    private static void InitializeOnLoad() { EditorApplication.update -= OnUpdate; EditorApplication.update += OnUpdate; }

    static VisualElement leftZone;
    static VisualElement rightZone;
    static VisualElement playZone;
    static Slider timeScaleSlider;
    static TextElement textSliderValue;

    private static void OnUpdate()
    {
        // 툴바 가져오기
        var toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
        var toolbars = Resources.FindObjectsOfTypeAll(toolbarType);
        var currentToolbar = toolbars.FirstOrDefault();
        if (currentToolbar == null) return;

        // 툴바 루트의 시각적 요소 얻기
        var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
        var fieldInfo = toolbarType.GetField("m_Root", bindingFlags);
        var toolbar = Convert.ChangeType(currentToolbar, toolbarType);
        var field = fieldInfo.GetValue(toolbar);
        var rootVisualElement = field as VisualElement;

        if (rootVisualElement != null)
        {
            if (leftZone == null) { leftZone = rootVisualElement.Q("ToolbarZoneLeftAlign"); }
            if (rightZone == null) { rightZone = rootVisualElement.Q("ToolbarZoneRightAlign"); }
            if (playZone == null) { playZone = rootVisualElement.Q("ToolbarZonePlayMode"); }

            if (timeScaleSlider == null)
            {
                // <Right Zone>
                SetTimeScaleSlider();

                // <Left Zone>
                SetSceneMenu();
                MakeDeletePlayerPrefsButton();
            }
        }
    }

    private static void SetTimeScaleSlider()
    {
        // 타임 스케일 타이틀
        var sliderTitle = new TextElement() { style = { marginLeft = 5, alignSelf = Align.Center } };
        sliderTitle.text = "TimeScale";
        playZone.Add(sliderTitle);

        // 타임 스케일 슬라이더 바
        timeScaleSlider = new Slider(0, 10) { style = { width = 100 } };
        timeScaleSlider.RegisterValueChangedCallback(v => { Time.timeScale = v.newValue; textSliderValue.text = Time.timeScale.ToString(); });
        playZone.Add(timeScaleSlider);

        // 타임 스케일 값 배경
        var sliderValueBG = new Toolbar() { style = { width = 25 } };

        // 타임 스케일 값
        textSliderValue = new TextElement() { style = { width = sliderValueBG.style.width, unityTextAlign = TextAnchor.MiddleCenter } };
        textSliderValue.text = Time.timeScale.ToString();
        sliderValueBG.Add(textSliderValue);
        playZone.Add(sliderValueBG);

        // 타임 스케일 초기화 버튼
        var btnInitSliderValue = new ToolbarButton(() => { timeScaleSlider.value = 1; Time.timeScale = 1; textSliderValue.text = Time.timeScale.ToString(); });
        btnInitSliderValue.style.marginLeft = 5;
        btnInitSliderValue.tooltip = "Init TimeScale 1";
        var textInitBtn = new TextElement() { style = { width = btnInitSliderValue.style.width, unityTextAlign = TextAnchor.MiddleCenter } };
        textInitBtn.text = "INIT";
        btnInitSliderValue.Add(textInitBtn);
        playZone.Add(btnInitSliderValue);

        timeScaleSlider.value = Time.timeScale;
    }

    private static void SetSceneMenu()
    {
        // 씬 선택 메뉴
        var sceneMenu = new ToolbarMenu() { style = { width = 120, marginLeft = 350 } };
        sceneMenu.text = EditorSceneManager.GetActiveScene().name;
        sceneMenu.tooltip = "Change Select Scene";

        // Assets/Scenes 하위에 있는 씬 검색 (GUID 반환)
        var findScenesGUID = AssetDatabase.FindAssets("t:Scene", new string[] { "Assets/01_Scenes" });
        for (int i = 0; i < findScenesGUID.Length; i++)
        {
            var convertPath = AssetDatabase.GUIDToAssetPath(findScenesGUID[i]);
            var sceneName = convertPath.Replace("Assets/01_Scenes/", "").Replace(".unity", "");

            sceneMenu.menu.AppendAction(sceneName, (drop) =>
            {
                EditorSceneManager.OpenScene(convertPath);
                sceneMenu.text = drop.name;
            });
        }

        leftZone.Add(sceneMenu);
    }

    private static void MakeDeletePlayerPrefsButton()
    {
        // PlayerPrefs 삭제 버튼
        var btnPrefabsClear = new ToolbarButton(() =>
        {
            Debug.Log("DeleteAll PlayerPrefs!");
            PlayerPrefs.DeleteAll();
        });
        btnPrefabsClear.style.marginLeft = 10;
        btnPrefabsClear.tooltip = "DeleteAll PlayerPrefs!";

        // PlayerPrefs 삭제 버튼에 올라갈 이미지
        var iconPrefabsClear = new Image();
        iconPrefabsClear.image = EditorGUIUtility.IconContent("SaveFromPlay").image;
        btnPrefabsClear.Add(iconPrefabsClear);

        leftZone.Add(btnPrefabsClear);
    }
}

#endif
