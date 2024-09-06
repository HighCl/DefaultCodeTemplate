using DefaultSetting.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_ExamplePopup : UI_Popup
    {
        [SerializeField] private Button exampleButton;
        [SerializeField] private TextMeshProUGUI exampleText;

        public override void Init()
        {
            base.Init();

            exampleButton.gameObject.BindEvent((_) => { print("버튼 클릭"); });
        }

        private void Update()
        {
            exampleText.text = Time.time.ToString();
        }
    }
}
