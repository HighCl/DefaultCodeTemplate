using DefaultSetting.Utility;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_ExampleScene : UI_Scene
    {
        enum Buttons
        {
            ExampleButton,
        }

        enum TMPs
        {
            ExampleTxt,
        }

        public override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<TextMeshProUGUI>(typeof(TMPs));

            GetButton((int)Buttons.ExampleButton).gameObject.BindEvent(
                (PointerEventData data) =>
                {
                    Managers.Scene.LoadScene(Define.Scene.InGame);
                    print("버튼 클릭");
                });
        }

        public void Update()
        {
            //GetTMP((int)TMPs.ExampleTxt).text = $"ExampleTxt\n{Time.time}";
        }
    }
}
