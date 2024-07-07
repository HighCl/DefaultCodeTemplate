using com.cyborgAssets.inspectorButtonPro;
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
        [Header("SteamWorks Settings")]
        [SerializeField] private bool isSteamWorks = false;

        [Header("InputSystem Settings")]
        [SerializeField] private bool isNewInputSystem = true;

        [Header("TMP Settings")]
        [SerializeField] private bool enbaleRayCastTarget = false;
        [Tooltip("파일명 입력 시 프로젝트 내에서 검색합니다.")]
        [SerializeField] private string targetFont = "NanumGothicSDF";


        [ProButton]
        private void OnSetting()
        {
            Debug.Log("설정 시작");

            SetEnableSteamWorks();
            SetInputSystem();
            SetTMPs();

            Debug.Log("설정 종료");
        }

        private void SetEnableSteamWorks()
        {
            string DEFINE_DISABLESTEAMWORKS = "DISABLESTEAMWORKS";

            if (isSteamWorks)
            {
                RemoveDefineSymbol(DEFINE_DISABLESTEAMWORKS);
            }
            else
            {
                AddDefineSymbol(DEFINE_DISABLESTEAMWORKS);
            }
        }

        private void SetInputSystem()
        {
            string DEFINE_NEW_INPUT_SYSTEM = "INPUT_TYPE_NEW";
            string DEFINE_LEGACY_INPUT_SYSTEM = "INPUT_TYPE_LEGACY";

            if (isNewInputSystem)
            {
                AddDefineSymbol(DEFINE_NEW_INPUT_SYSTEM);
                RemoveDefineSymbol(DEFINE_LEGACY_INPUT_SYSTEM);
            }
            else
            {
                AddDefineSymbol(DEFINE_LEGACY_INPUT_SYSTEM);
                RemoveDefineSymbol(DEFINE_NEW_INPUT_SYSTEM);
            }
        }

        private void SetTMPs()
        {
            TMP_Settings tmpSettings = TMP_Settings.GetSettings();

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
        }

        public static void AddDefineSymbol(string targetSymbol)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);

            if (!definesString.Contains(targetSymbol))
            {
                definesString += $";{targetSymbol}";
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    EditorUserBuildSettings.selectedBuildTargetGroup, definesString);
            }
        }

        public static void RemoveDefineSymbol(string targetSymbol)
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);

            var defines = definesString.Split(';').ToList();
            defines.Remove(targetSymbol);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", defines));
        }
    }
}
