using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_ExamplePopup : UI_Popup
    {
        enum Images
        {
            SilhouetteImage,
        }

        enum Buttons
        {
            BackButton,
        }


        public override void Init()
        {
            base.Init();

            Bind<Button>(typeof(Buttons));
            Bind<Image>(typeof(Images));

            //GetButton((int)Buttons.BackButton).gameObject.BindEvent((PointerEventData data) =>
            //{
            //    Managers.Sound.Play(Managers.Data.MstMaster.UIData.UIClickSound);
            //    Managers.UI.ClosePopupUI();
            //});

            GetImage((int)Images.SilhouetteImage).sprite = Managers.Resource.Load<Sprite>($"Sprite/UI/{Managers.Game.currentStage}Silhouette");
        }
    }
}
