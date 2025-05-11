namespace DefaultSetting
{
    public class ExampleScene : BaseScene
    {
        protected override void Init()
        {
            base.Init();
            SceneType = Define.Scene.TestScene;
            //Managers.UI.ShowSceneUI<UI_ExampleScene>();
        }

        public override void Clear()
        {

        }
    }
}
