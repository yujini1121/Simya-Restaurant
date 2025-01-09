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
            Debug.Log("Awake() : ȣ���");
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
            Debug.Log($"ChangeScene(string sceneName) : ȣ���");
        }

        Time.timeScale = 0;

        StartCoroutine(FadeOutAndLoadingScene(sceneName));
    }

    private IEnumerator FadeOutAndLoadingScene(string sceneName)
    {
        if (isDebug)
        {
            Debug.Log($"FadeOutAndLoadingScene(string sceneName) : ȣ���");
        }

        if(PlayerController.instance != null) // Ÿ��Ʋ ȭ�鿡���� �÷��̾� ������ ������ �ʿ䰡 ���µ� �ؼ� �������� ����
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
            Debug.Log($"FadeOutAndLoadingScene(string sceneName) : ù ���Ϲ� ���");
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (isDebug)
            {
                Debug.Log($"FadeOutAndLoadingScene(string sceneName) : ���κ� progress = {progress} / asyncLoad.progress = {asyncLoad.progress} / asyncLoad.isDone = {asyncLoad.isDone}");
            }

            if (asyncLoad.progress >= 0.9f)
            {
                if (isDebug)
                {
                    Debug.Log($"FadeOutAndLoadingScene(string sceneName) : ���� �ε� ��! progress = {progress} / asyncLoad.progress = {asyncLoad.progress} / asyncLoad.isDone = {asyncLoad.isDone}");
                }

                //yield return new WaitForSeconds(2.0f); // << ���� ���� ��������! ���������� �Ѿ���� �ʾƿ�!
                if (isDebug)
                {
                    Debug.Log($"FadeOutAndLoadingScene(string sceneName) : asyncLoad.allowSceneActivation = true");
                }

                asyncLoad.allowSceneActivation = true;

                if (isDebug)
                {
                    Debug.Log($"FadeOutAndLoadingScene(string sceneName) : WaitForSeconds �����!");
                }
            }

            if (isDebug)
            {
                Debug.Log($"FadeOutAndLoadingScene(string sceneName) : �Ʒ��κ� progress = {progress} / asyncLoad.progress = {asyncLoad.progress} / asyncLoad.isDone = {asyncLoad.isDone}");
            }

            yield return null;
        }
        if (isDebug)
        {
            Debug.Log($"FadeOutAndLoadingScene(string sceneName) : �� ��° ���Ϲ� ���");
        }

        yield return StartCoroutine(FadeIn());

        Time.timeScale = 1.0f;
    }

    private IEnumerator FadeIn()
    {
        if (isDebug)
        {
            Debug.Log($"FadeIn() : ȣ���");
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
            Debug.Log($"FadeIn() : ���̵� �� �Ϸ�");
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
