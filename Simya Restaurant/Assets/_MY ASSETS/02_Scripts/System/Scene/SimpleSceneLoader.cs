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

	[Header("Fade 관련")]
	[SerializeField] private CanvasGroup fadeCanvasGroup;	// 알파 조절용
	[SerializeField] private TextMeshProUGUI loadingText;	// "Loading..." 텍스트
	[SerializeField] private float fadeDuration = 1f;		// 페이드 시간

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
			Debug.LogWarning("Scene name is empty. 연결된 SceneAsset이 없을 수 있음.");
		}
	}

	private IEnumerator LoadSceneRoutine()
	{
		// 페이드 아웃
		yield return StartCoroutine(Fade(0, 1));

		if (loadingText != null) loadingText.gameObject.SetActive(true);

		AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);
		operation.allowSceneActivation = false;

		while (operation.progress < 0.9f)
		{
			yield return null;
		}

		operation.allowSceneActivation = true;

		// 씬 전환이 끝날 때까지 기다림
		while (!operation.isDone)
		{
			yield return null;
		}

		// 페이드 인
		yield return StartCoroutine(Fade(1, 0));
	}


	private IEnumerator Fade(float from, float to)
	{
		float time = 0f;
		if (fadeCanvasGroup == null)
		{
			Debug.LogWarning("Fade Canvas Group이 연결되어 있지 않습니다.");
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
