namespace DefaultSetting
{
    //DefaultDefine
    //어떤 게임이든 기본적으로 가지고 있어야 하는 Define 요소들
    //변동되지 않거나 아주 약간씩 바뀌는 요소들을 넣는다.
    public partial class Define
    {
        #region KEY
        public static readonly string REBINDS_KEY = "rebinds";
        public static readonly string LANGUAGE_KEY = "language";
        public static readonly string IS_CURSOR_LOCK_KEY = "CursorLockMode";
        #endregion
        #region Const
        public const string DEBUG_CONDITIONAL = "UNITY_EDITOR";
        public const string TEST_CONDITIONAL = DEBUG_CONDITIONAL;
        #endregion

        public enum ControlSchema
        {
            None = 0,
            KeyboardMouse,
            Gamepad,
        }

        public enum PlayState
        {
            Test,
            Demo_PC,
            Demo_Web,
            Build_PC,
        }

        public enum PlayPlatform
        {
            NotSetting,
            Steam,
            Stove,
            OPGG,
            Zempie,
        }

        public enum UISortingOrder
        {

        }

        public enum UIEvent
        {
            Click,
            BeginDrag,
            Drag,
            EndDrag,
            Enter,
            Exit,
            Down,
            Up,
        }

        public enum UIType
        {
            Scene,
            Popup
        }

        public enum MouseEvent
        {
            Press,
            Click,
        }

        public enum CameraMode
        {
            QuarterView,
        }

        public enum Language
        {
            NotSetting,
            Korean,
            English,
            Japanese,
        }

        public enum Sound
        {
            Bgm,
            Effect,
            MaxCount,
        }
    }

    //ChangeDefine
    //게임에 따라 크게 바뀌는 요소들
    public partial class Define
    {
        #region PATH
        public static string TEMP_PATH = "TEMP";
        #endregion

        #region Data
        public static readonly int TEMP_DATA = 2;
        #endregion

        public enum AchievementList
        {
            None,
        }

        public enum GameMode
        {
            Null,
            Main,
            Test,
        }

        public enum Layer
        {
            Player = 3,
            CantMovePlatform = 7,
            Enemy = 8,
            Trap = 9,
            CanMovePlatform = 10,
            DieCollider = 11,
            Switch = 12,
            ReboundWall = 13,
            Item = 14,
            CanGrapplePlatform = 15,
            CantGrapplePlatform = 16,
        }

        public enum TextKey
        {
            Log_EmptyInputField,
            Log_NotOpenStage,
            Log_NotOpenMode,
            Log_DemoDontPlay,
        }

        public enum Tag
        {
            Dynamic,
            NonDynamic,
            Body,
            CanGrabWall,
        }

        public enum ObjectName
        {
            ReboundObject,
        }

        public enum Scene
        {
            Unknown,
            Main,
            InGame,
            TestScene,
        }

        public enum Stage
        {
            Null,
            NotLoad,
            Tutorial,
            Stage1,
            Stage2,
        }
    }
}
