using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(Fade(1, 0)); // �� ���� �� ���̵���
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    private IEnumerator Transition(string sceneName)
    {
        yield return Fade(0, 1); // ���̵�ƿ�
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return new WaitForSeconds(0.1f); // �� �ε� �Ϸ� �� ��¦ ���
        yield return Fade(1, 0); // ���̵���
    }

    private IEnumerator Fade(float from, float to)
    {
        float time = 0f;
        fadeCanvasGroup.alpha = from;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
    }
}
