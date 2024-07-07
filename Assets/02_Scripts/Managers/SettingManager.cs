using com.cyborgAssets.inspectorButtonPro;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DefaultSetting
{
    public class SettingManager : MonoBehaviour
    {
        [SerializeField] private bool isSteamWorks = false;
        [SerializeField] private bool isNewInputSystem = true;

        [ProButton]
        public void OnSetting()
        {
            Debug.Log("설정 시작");

            SetEnableSteamWorks();
            SettingInputSystem();
        }

        public void SetEnableSteamWorks()
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

        public void SettingInputSystem()
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
