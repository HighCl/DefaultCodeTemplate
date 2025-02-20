using UnityEngine.EventSystems;

namespace DefaultSetting
{
    public class UI_Popup : UI_Base
    {
        public override void Init()
        {
            base.Init();
            Managers.UI.SetCanvas(gameObject, true);
        }

        public virtual void ClosePopupUI(PointerEventData _) => ClosePopupUI();
        public virtual void ClosePopupUI()
        {
            Managers.UI.ClosePopupUI(this);
        }
    }
}
