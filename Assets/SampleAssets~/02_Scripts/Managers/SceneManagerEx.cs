using DefaultSetting.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultSetting
{
    public class SceneManagerEx : MonoBehaviour
    {
        [field: SerializeField]
        public float loadingFadeTime { get; private set; } = 0.2f;
        [field: SerializeField]
        public float minLoadingTime { get; private set; } = 0.7f;

        public WaitForSecondsRealtime fadeWfs = null;
        public WaitForSecondsRealtime delayWfs = null;

        public BaseScene CurrentScene { get { return FindFirstObjectByType<BaseScene>(); } }

        public void Init()
        {
            fadeWfs = new WaitForSecondsRealtime(loadingFadeTime);
            delayWfs = new WaitForSecondsRealtime(minLoadingTime);

            SceneManager.sceneLoaded -= OnLoadSetting;
            SceneManager.sceneLoaded += OnLoadSetting;
        }

        public void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLoadSetting;
        }

        public void LoadSceneAsync(Define.Scene type)
        {
            StartCoroutine(Co_LoadSceneAsync(type));
        }

        IEnumerator Co_LoadSceneAsync(Define.Scene type)
        {
            //사전 조건
            DebugUtility.Log($"씬 로드 시작");
            float startTime = Time.time;

            //로직
            yield return StartCoroutine(Managers.UI.LoadingPopup.BeginLoading());

            AsyncOperation operation = SceneManager.LoadSceneAsync(GetSceneName(type));
            operation.allowSceneActivation = false;
            yield return StartCoroutine(Managers.UI.LoadingPopup.StartLoadingAsync(operation));

            float elapsedTime = Time.time - startTime;
            yield return StartCoroutine(Managers.UI.LoadingPopup.WaitRemainingTimeAsync(elapsedTime));

            //사후 조건
            DebugUtility.Log($"씬 로드 종료");
            Managers.Clear();
            operation.allowSceneActivation = true;
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(Managers.UI.LoadingPopup.CompleteLoad());
        }

        public void OnLoadSetting(Scene scene, LoadSceneMode sceneMode)
        {
            Managers.Video.CheckLetterBox();
        }

        string GetSceneName(Define.Scene type)
        {
            string name = System.Enum.GetName(typeof(Define.Scene), type);
            return name;
        }

        public void Clear()
        {
            CurrentScene.Clear();
        }
    }
}
