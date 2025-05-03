using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimpleSceneLoader : MonoBehaviour
{
#if UNITY_EDITOR
	[SerializeField] private SceneAsset nextSceneAsset;
#endif
	[SerializeField] private string nextSceneName;

	[Header("Fade ����")]
	[SerializeField] private CanvasGroup fadeCanvasGroup;	// ���� ������
	[SerializeField] private TextMeshProUGUI loadingText;	// "Loading..." �ؽ�Ʈ
	[SerializeField] private float fadeDuration = 1f;		// ���̵� �ð�

#if UNITY_EDITOR
	private void OnValidate()
	{
		if (nextSceneAsset != null)
		{
			nextSceneName = nextSceneAsset.name;
		}
	}
#endif

	public void LoadScene()
	{
		if (!string.IsNullOrEmpty(nextSceneName))
		{
			StartCoroutine(LoadSceneRoutine());
		}
		else
		{
			Debug.LogWarning("Scene name is empty. ����� SceneAsset�� ���� �� ����.");
		}
	}

	private IEnumerator LoadSceneRoutine()
	{
		// ���̵� �ƿ�
		yield return StartCoroutine(Fade(0, 1));

		if (loadingText != null) loadingText.gameObject.SetActive(true);

		AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
		operation.allowSceneActivation = false;

		while (operation.progress < 0.9f)
		{
			yield return null;
		}

		operation.allowSceneActivation = true;

		// �� ��ȯ�� ���� ������ ��ٸ�
		while (!operation.isDone)
		{
			yield return null;
		}

		// ���̵� ��
		yield return StartCoroutine(Fade(1, 0));
	}


	private IEnumerator Fade(float from, float to)
	{
		float time = 0f;
		if (fadeCanvasGroup == null)
		{
			Debug.LogWarning("Fade Canvas Group�� ����Ǿ� ���� �ʽ��ϴ�.");
			yield break;
		}

		while (time < fadeDuration)
		{
			time += Time.deltaTime;
			float alpha = Mathf.Lerp(from, to, time / fadeDuration);
			fadeCanvasGroup.alpha = alpha;
			yield return null;
		}

		fadeCanvasGroup.alpha = to;
	}
}
