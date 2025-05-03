using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance
    {
        get
        {
            return instance;
        }
    }

    private static SceneTransition instance;

    [SerializeField] private CanvasGroup fadImg;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private bool isDebug = false;

    private float fadeDuration = 4.0f;

    private void Awake()
    {
        if (isDebug)
        {
            Debug.Log("Awake() : 호출됨");
        }

        if (instance != null)
        {
            DestroyImmediate(this.gameObject);

            return;
        }

        instance = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        if (isDebug)
        {
            Debug.Log($"ChangeScene(string sceneName) : 호출됨");
        }

        Time.timeScale = 0;

        StartCoroutine(FadeOutAndLoadingScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadingScene(string sceneName)
    {
        if (isDebug)
        {
            Debug.Log($"FadeOutAndLoadingScene(string sceneName) : 호출됨");
        }

        if(PlayerController.instance != null) // 타이틀 화면에서는 플레이어 데이터 저장할 필요가 없는데 해서 오류나서 수정
        {
            PlayerController.instance.CallWhenSceneEnd();
        }
        fadImg.blocksRaycasts = true;

        loadingText.text = "Loading...";
        loadingText.gameObject.SetActive(true);

        for (float t = 0; t < fadeDuration; t += Time.unscaledDeltaTime)
        {
            if (isDebug)
            {
                Debug.Log($"FadeOutAndLoadingScene(string sceneName) : t = {t}, fadeDuration = {fadeDuration}");
            }

            fadImg.alpha = t / fadeDuration;
            yield return null;
        }
        fadImg.alpha = 1;

        if (isDebug)
        {
            Debug.Log($"FadeOutAndLoadingScene(string sceneName) : 첫 와일문 통과");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (isDebug)
            {
                Debug.Log($"FadeOutAndLoadingScene(string sceneName) : 윗부분 progress = {progress} / asyncLoad.progress = {asyncLoad.progress} / asyncLoad.isDone = {asyncLoad.isDone}");
            }

            if (asyncLoad.progress >= 0.9f)
            {
                if (isDebug)
                {
                    Debug.Log($"FadeOutAndLoadingScene(string sceneName) : 거의 로드 됨! progress = {progress} / asyncLoad.progress = {asyncLoad.progress} / asyncLoad.isDone = {asyncLoad.isDone}");
                }

                //yield return new WaitForSeconds(2.0f); // << 여기 정말 문제에요! 다음씬으로 넘어가지지 않아요!
                if (isDebug)
                {
                    Debug.Log($"FadeOutAndLoadingScene(string sceneName) : asyncLoad.allowSceneActivation = true");
                }

                asyncLoad.allowSceneActivation = true;

                if (isDebug)
                {
                    Debug.Log($"FadeOutAndLoadingScene(string sceneName) : WaitForSeconds 종료됨!");
                }
            }

            if (isDebug)
            {
                Debug.Log($"FadeOutAndLoadingScene(string sceneName) : 아랫부분 progress = {progress} / asyncLoad.progress = {asyncLoad.progress} / asyncLoad.isDone = {asyncLoad.isDone}");
            }

            yield return null;
        }
        if (isDebug)
        {
            Debug.Log($"FadeOutAndLoadingScene(string sceneName) : 두 번째 와일문 통과");
        }

        yield return StartCoroutine(FadeIn());

        Time.timeScale = 1.0f;
    }

    private IEnumerator FadeIn()
    {
        if (isDebug)
        {
            Debug.Log($"FadeIn() : 호출됨");
        }

        float currentAlpha = 1.0f;

        while (currentAlpha > 0)
        {
            currentAlpha -= Time.unscaledDeltaTime / fadeDuration;
            fadImg.alpha = Mathf.Clamp01(currentAlpha);
            yield return null;
        }

        if (isDebug)
        {
            Debug.Log($"FadeIn() : 페이드 인 완료");
        }

        if(PlayerController.instance != null)
        {
            PlayerController.instance.CallWhenSceneStart();
        }
        fadImg.alpha = 0;
        fadImg.blocksRaycasts = false;
        loadingText.gameObject.SetActive(false);
    }

}
