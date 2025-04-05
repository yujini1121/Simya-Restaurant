using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// SceneAsset은 에디터 전용 클래스이며, 빌드 시 포함되지 않습니다.
/// LoadScene()을 사용하려면, nextSceneName에 입력된 씬이
/// Build Settings에 등록되어 있어야 정상 작동합니다.
/// </summary>

public class SimpleSceneLoader : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset nextSceneAsset;
#endif
    [SerializeField] private string nextSceneName;

#if UNITY_EDITOR
    // 씬 에셋 이름을 자동으로 sceneName에 넣어주는 처리
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
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Scene name is empty. 연결된 SceneAsset이 없을 수 있음.");
        }
    }
}
