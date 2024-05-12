using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DefaultSetting.Utility
{
    public static class ScObjExtension
    {
        // 사용법
        // SingleUse 시
        // this.AutoLoadAsset();

        // MultiUse 시 
        // string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
        // string assetName = Path.GetFileNameWithoutExtension(assetPath);
        // this.AutoLoadAsset(assetName);
#if UNITY_EDITOR
        public static void AutoLoadAsset(this UnityEngine.ScriptableObject scriptable, string assetName = null)
        {
            //스크립터블의 필드를 모두 찾고
            FieldInfo[] fields = scriptable.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            fields = fields.Where(e => e.FieldType.IsPrimitive == false && e.FieldType != typeof(string)).ToArray();

            foreach (var field in fields)
            {
                string fieldname = field.Name;

                //언더바 제거
                if (fieldname[0].Equals("_"[0]))
                    fieldname = fieldname.Replace("_", "");

                //첫 문자 대문자
                fieldname = char.ToUpper(fieldname[0]) + fieldname.Substring(1);

                //에셋 이름 구분하여 찾는 경우 추가
                string targetFile = assetName + fieldname;

                var findAssets = AssetDatabase.FindAssets(targetFile, new string[] { "Assets" }).Where(e => Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(e)) == targetFile).ToArray();

                switch (findAssets.Length)
                {
                    case 0:
                        Debug.LogError($"타겟 파일명 [{targetFile}]와 일치하는 파일을 찾지 못했습니다.\n");
                        continue;
                    case 1:
                        //파일 발견
                        break;
                    default:
                        Debug.LogError($"타겟 파일명 [{targetFile}]와 동일한 이름의 파일이 {findAssets.Length}개 존재합니다.\n");
                        continue;
                }

                var path = AssetDatabase.GUIDToAssetPath(findAssets[0]);
                object obj = AssetDatabase.LoadAssetAtPath(path, field.FieldType);
                field.SetValue(scriptable, obj);
            }
        }
#endif
    }
}
