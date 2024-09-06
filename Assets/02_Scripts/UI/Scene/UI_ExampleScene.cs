using DefaultSetting.Utility;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_ExampleScene : UI_Scene
    {
        [SerializeField] private Button exampleButton;
        [SerializeField] private TextMeshProUGUI exampleTxt;

        public override void Init()
        {
            base.Init();

            exampleButton.gameObject.BindEvent(
                (PointerEventData data) =>
                {
                    Managers.Scene.LoadScene(Define.Scene.InGame);
                    print("버튼 클릭");
                });
        }

        public void Update()
        {
            exampleTxt.text = $"Example Text\n{Time.time}";
        }
    }
}
