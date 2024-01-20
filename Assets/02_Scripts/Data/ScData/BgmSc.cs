using UnityEngine;

namespace DefaultSetting
{

    [CreateAssetMenu(fileName = "BgmData", menuName = "Scriptable Object/Bgm Data", order = 0)]
    public class BgmSc : ScriptableObjectEx
    {
        [SerializeField]
        private AudioClip _mainBgm;
        [SerializeField]
        private AudioClip _inGameBgm;

        public AudioClip MainBgm => _mainBgm;
        public AudioClip InGameBgm => _inGameBgm;

        public override void AutoFind()
        {
#if UNITY_EDITOR
            this.AutoLoadAsset();
#endif
        }
    }
}
