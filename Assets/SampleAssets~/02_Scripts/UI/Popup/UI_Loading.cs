using DefaultSetting.Utility;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_Loading : UI_Popup
    {
        [SerializeField] private Image fadeImage;

        public float fadeTime = 0;
        public float delayTime = 0;

        public Coroutine co;

        public override void Init()
        {
            fadeTime = Managers.Scene.loadingFadeTime;
            delayTime = Managers.Scene.loadingDelayTime;

            fadeImage.color = Extension.GetChangeAlpha(fadeImage.color, 0);
            fadeImage.gameObject.SetActive(false);
        }

        public void OnStartFade(Define.Scene changeScene)
        {
            if (co != null)
            {
                StopCoroutine(co);
            }
            co = StartCoroutine(CoFade(changeScene));
        }

        private IEnumerator CoFade(Define.Scene changeScene)
        {
            //사전 조건
            fadeImage.gameObject.SetActive(true);

            yield return StartCoroutine(Extension.Co_FadePlay(null, fadeImage, Extension.Ease.EaseOutCubic, fadeTime, 0, 1, isRealTime: true));
            yield return Managers.Scene.delayWfs;
            yield return StartCoroutine(Extension.Co_FadePlay(null, fadeImage, Extension.Ease.EaseOutCubic, fadeTime, 1, 0, isRealTime: true));

            //사후 조건
            fadeImage.gameObject.SetActive(false);
            co = null;
        }
    }
}
