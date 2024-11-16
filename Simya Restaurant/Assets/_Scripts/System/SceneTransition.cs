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

    private float fadeDuration = 4.0f;

    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this.gameObject);

            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 0;

        StartCoroutine(FadeOutAndLoadingScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadingScene(string sceneName)
    {
        fadImg.blocksRaycasts = true;

        loadingText.text = "Loading...";
        loadingText.gameObject.SetActive(true);

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            fadImg.alpha = t / fadeDuration;
            yield return null;
        }
        fadImg.alpha = 1;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(2.0f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return StartCoroutine(FadeIn());

        Time.timeScale = 1.0f;
    }

    private IEnumerator FadeIn()
    {
        float currentAlpha = 1.0f;

        while (currentAlpha > 0)
        {
            currentAlpha -= Time.deltaTime / fadeDuration;
            fadImg.alpha = Mathf.Clamp01(currentAlpha);
            yield return null;
        }

        fadImg.alpha = 0;
        fadImg.blocksRaycasts = false;
        loadingText.gameObject.SetActive(false);
    }

}
