using DefaultSetting.Utility;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultSetting
{
    public class UI_Loading : UI_Popup
    {
        [SerializeField] private Image _loadingScreen;
        [SerializeField] private Slider _loadingBar;

        public float fadeTime => Managers.Scene.loadingFadeTime;
        public float minLoadingTime => Managers.Scene.minLoadingTime;
        public float loadingScreenTargetFadeAlpha = 1f;

        public override void Init()
        {
            base.Init();

            SetActiveLoadingGroup(false);
        }

        private void SetActiveLoadingGroup(bool isActive)
        {
            _loadingScreen.gameObject.SetActive(isActive);
            _loadingBar.gameObject.SetActive(isActive);
        }

        public IEnumerator BeginLoading()
        {
            SetActiveLoadingGroup(true);
            _loadingScreen.color = _loadingScreen.color.GetChangeAlpha(0);
            _loadingBar.value = 0f;
            yield return StartCoroutine(Extension.Co_FadePlay(null, _loadingScreen, Extension.Ease.Linear, fadeTime, 0, loadingScreenTargetFadeAlpha, isRealTime: true));
        }

        public IEnumerator StartLoadingAsync(AsyncOperation operation)
        {
            //DebugUtility.Log($"StartLoadingAsync");
            while (operation.progress < 0.9f)
            {
                //DebugUtility.Log($"로드중 {operation.isDone}, {operation.progress}");
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
                _loadingBar.value = progressValue;
                yield return null;
            }
        }

        public IEnumerator WaitRemainingTimeAsync(float elapsedTime)
        {
            float startFill = _loadingBar.value;
            float targetFill = 0.9f;
            float progressEndWaitTime = 0.3f;
            float remainingProgressTime = Mathf.Max(0, minLoadingTime - elapsedTime) - progressEndWaitTime;
            float currentProgressTime = 0;
            //DebugUtility.Log($"로딩 완료, 남은 대기 시간: {remainingProgressTime}");

            while (currentProgressTime < remainingProgressTime)
            {
                float t = currentProgressTime / remainingProgressTime;
                _loadingBar.value = Mathf.Lerp(startFill, targetFill, t);
                currentProgressTime += Time.deltaTime;
                yield return 0;
            }
            _loadingBar.value = 0.9f;
            yield return new WaitForSeconds(progressEndWaitTime);
            _loadingBar.value = 1;
        }

        public IEnumerator CompleteLoad()
        {
            StartCoroutine(Extension.Co_FadePlay(null, _loadingScreen, Extension.Ease.Linear, fadeTime, loadingScreenTargetFadeAlpha, 0, isRealTime: true));
            StartCoroutine(Extension.Co_FadePlay(null, _loadingBar, Extension.Ease.Linear, fadeTime, loadingScreenTargetFadeAlpha, 0, isRealTime: true));
            yield return new WaitForSeconds(fadeTime);
            SetActiveLoadingGroup(false);
        }
    }
}
