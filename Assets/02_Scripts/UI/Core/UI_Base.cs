using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultSetting
{
    public abstract class UI_Base : MonoBehaviour
    {
        protected bool isInit = false;
        [ReadOnly] public List<LocalizedSetting> localizedList = null;
        [Space(10)]
        [ReadOnly, SerializeField] private string _ = "------------------------------------------------------------------------------------------------------------------------------------------------------------";

        protected virtual void Reset()
        {
#if UNITY_EDITOR
            // 현재 프리팹 편집 모드인지 확인
            UnityEditor.SceneManagement.PrefabStage prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (!(prefabStage != null && prefabStage.stageHandle.IsValid()))
                return;

            Debug.Log($"필드 할당 시작");

            //필드 찾고
            //TODO: Component만 찾게 설정해야 할듯?
            System.Reflection.FieldInfo[] fields = this.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            fields = fields
                .Where(
                e => e.FieldType.IsSubclassOf(typeof(Component))
                    && !e.FieldType.IsArray
                    && !e.FieldType.IsGenericType)
                .ToArray();

            GameObject prefabRoot = prefabStage.prefabContentsRoot;
            foreach (System.Reflection.FieldInfo field in fields)
            {
                string fieldName = NameModifier(field.Name);
                Component[] getComponents = prefabRoot.GetComponentsInChildren(field.FieldType);
                foreach (var comp in getComponents)
                {
                    if (!comp.name.Equals(fieldName))
                        continue;

                    //Debug.Log($"{fieldName} 할당");
                    field.SetValue(this, comp);
                    break;
                }
            }
            Debug.Log($"필드 할당 종료");

            string NameModifier(string fieldName)
            {
                //프로퍼티 처리
                fieldName = System.Text.RegularExpressions.Regex.Replace(fieldName, @"[<>]|k__BackingField", "");

                //언더바 제거
                if (fieldName.StartsWith("_"))
                    fieldName = fieldName.Substring(1);

                //첫글자 대문자
                fieldName = char.ToUpper(fieldName[0]) + fieldName.Substring(1);

                return fieldName;
            }
#endif
        }

        public virtual void Init()
        {
            isInit = true;
            Managers.UI.updateUIAction -= UpdateUI;
            Managers.UI.updateUIAction += UpdateUI;
            Managers.UI.updateLocalizationAction -= UpdateLocalization;
            Managers.UI.updateLocalizationAction += UpdateLocalization;
            UpdateLocalization();
        }

        protected void Start()
        {
            if (!isInit)
            {
                isInit = true;
                Init();
            }
        }

        public void CloseSetting()
        {
            Managers.UI.updateUIAction -= UpdateUI;
            Managers.UI.updateLocalizationAction -= UpdateLocalization;
        }

        public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
        {
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

            switch (type)
            {
                case Define.UIEvent.Click:
                    evt.OnClickHandler -= action;
                    evt.OnClickHandler += action;
                    break;
                case Define.UIEvent.BeginDrag:
                    evt.OnBeginDragHandler -= action;
                    evt.OnBeginDragHandler += action;
                    break;
                case Define.UIEvent.Drag:
                    evt.OnDragHandler -= action;
                    evt.OnDragHandler += action;
                    break;
                case Define.UIEvent.EndDrag:
                    evt.OnEndDragHandler -= action;
                    evt.OnEndDragHandler += action;
                    break;
                case Define.UIEvent.Enter:
                    evt.OnEnterHandler -= action;
                    evt.OnEnterHandler += action;
                    break;
                case Define.UIEvent.Exit:
                    evt.OnExitHandler -= action;
                    evt.OnExitHandler += action;
                    break;
                case Define.UIEvent.Up:
                    evt.OnUpHandler -= action;
                    evt.OnUpHandler += action;
                    break;
                case Define.UIEvent.Down:
                    evt.OnDownHandler -= action;
                    evt.OnDownHandler += action;
                    break;
            }
        }

        public static void DeleteEvent(GameObject go)
        {
            UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);
            evt.OnClickHandler = null;
            evt.OnDragHandler = null;
        }

        public virtual void UpdateUI()
        {

        }

        [ContextMenu("Set LocalizedID")]
        public void SetLocalizedID()
        {
            localizedList = GetComponentsInChildren<LocalizedSetting>(includeInactive: true).ToList();
            string className = GetType().Name;
            foreach (var localized in localizedList)
            {
                localized.SetDefaultID(className);
            }
        }

        public void UpdateLocalization()
        {
            if (localizedList == null)
                return;

            foreach (var localized in localizedList)
            {
                if (localized == null)
                {
                    Debug.LogWarning("존재X, 임의로 재설정합니다.");
                    SetLocalizedID();
                    UpdateLocalization();
                    break;
                }
                localized.UpdateLocalized();
            }
        }
    }

}