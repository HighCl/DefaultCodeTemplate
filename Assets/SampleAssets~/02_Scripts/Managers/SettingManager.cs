using com.cyborgAssets.inspectorButtonPro;
using DefaultSetting.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace DefaultSetting
{
    public class SettingManager : MonoBehaviour
    {
        [Header("Set SteamWorks")]
        [SerializeField] private bool isSteamWorks = false;

        [Header("Set InputSystem")]
        [SerializeField] private bool isNewInputSystem = true;

        [Header("Set TMP")]
        [SerializeField] private bool enbaleRayCastTarget = false;
        [Tooltip("파일명 입력 시 프로젝트 내에서 검색합니다.")]
        [SerializeField] private string targetFont = "NanumGothicSDF";

        [Header("Make Editor Config")]
        [SerializeField] private bool makeEditorConfig = true;

        [ProButton]
        private void OnSetting()
        {
            Debug.Log("설정 시작");

            SetEnableSteamWorks();
            SetInputSystem();
            SetTMPs();
            MakeEditorConfig();

            Debug.Log("설정 종료");
        }

        private void SetEnableSteamWorks()
        {
#if UNITY_EDITOR
            string DEFINE_DISABLESTEAMWORKS = "DISABLESTEAMWORKS";

            if (isSteamWorks)
            {
                Extension.RemoveDefineSymbol(DEFINE_DISABLESTEAMWORKS);
            }
            else
            {
                Extension.AddDefineSymbol(DEFINE_DISABLESTEAMWORKS);
            }
#endif
        }

        private void SetInputSystem()
        {
#if UNITY_EDITOR
            string DEFINE_NEW_INPUT_SYSTEM = "INPUT_TYPE_NEW";
            string DEFINE_LEGACY_INPUT_SYSTEM = "INPUT_TYPE_LEGACY";

            if (isNewInputSystem)
            {
                Extension.AddDefineSymbol(DEFINE_NEW_INPUT_SYSTEM);
                Extension.RemoveDefineSymbol(DEFINE_LEGACY_INPUT_SYSTEM);
            }
            else
            {
                Extension.AddDefineSymbol(DEFINE_LEGACY_INPUT_SYSTEM);
                Extension.RemoveDefineSymbol(DEFINE_NEW_INPUT_SYSTEM);
            }
#endif
        }

        private void SetTMPs()
        {
#if UNITY_EDITOR
            TMP_Settings tmpSettings = TMP_Settings.GetSettings();
            if (tmpSettings == null)
            {
                Debug.LogWarning("Not Import TMP Essentials");
                return;
            }

            Type type = tmpSettings.GetType();

            //Set enable Raycast Target
            FieldInfo enableRaycastTargetField = type.GetField("m_EnableRaycastTarget", BindingFlags.NonPublic | BindingFlags.Instance);
            if (enableRaycastTargetField != null)
            {
                enableRaycastTargetField.SetValue(tmpSettings, enbaleRayCastTarget);
            }
            else
            {
                throw new InvalidOperationException("Field not found");
            }

            //Set Default Font Asset
            FieldInfo defaultFontAssetField = type.GetField("m_defaultFontAsset", BindingFlags.NonPublic | BindingFlags.Instance);
            if (defaultFontAssetField != null)
            {
                List<string> findAssetPath = AssetDatabase.FindAssets(targetFont, new string[] { "Assets" })
                    .Where(e => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(e)) == targetFont)
                    .Select(asset => AssetDatabase.GUIDToAssetPath(asset))
                    .ToList();

                switch (findAssetPath.Count)
                {
                    case 0:
                        Debug.LogError($"폰트 탐색에 실패했습니다.");
                        break;
                    case 1:
                        object obj = AssetDatabase.LoadAssetAtPath(findAssetPath[0], typeof(TMP_FontAsset));
                        defaultFontAssetField.SetValue(tmpSettings, obj);
                        break;
                    default:
                        Debug.LogError($"탐색된 폰트가 여러개입니다. \n개수: {findAssetPath.Count}");
                        break;
                }
            }
            else
            {
                throw new InvalidOperationException("Field not found");
            }
#endif
        }

        /// <summary>EditorConfig 파일이 없으면 생성합니다.</summary>
        private void MakeEditorConfig()
        {
#if UNITY_EDITOR
            if (!makeEditorConfig)
                return;

            string projectPath = Application.dataPath.Substring(0, Application.dataPath.Length - "/Assets".Length);
            string filePath = Path.Combine(projectPath, ".editorconfig");

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, "root = true\r\n\r\n[*]\r\ncharset = utf-8");
                Debug.Log("File created: " + filePath);
            }
            else
            {
                Debug.Log("File already exists: " + filePath);
            }
#endif
        }
    }
}
