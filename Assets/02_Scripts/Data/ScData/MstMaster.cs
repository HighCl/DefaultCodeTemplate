using DefaultSetting.Utility;
using UnityEngine;

namespace DefaultSetting
{
    [CreateAssetMenu]
    public class MstMaster : ScriptableObjectEx
    {
        [SerializeField]
        private MstLocalizeDataScript MstLocalizeDataAsset;

        [SerializeField]
        private BgmSc _bgmData;

        public MstLocalizeDataScript MstLocalizeDataScript => MstLocalizeDataAsset;
        public BgmSc BgmData => _bgmData;

        public override void AutoFind()
        {
#if UNITY_EDITOR
            this.AutoLoadAsset();
#endif
        }
    }
}
