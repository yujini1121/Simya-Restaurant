using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SimpleSceneLoader : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset nextSceneAsset;
#endif
    [SerializeField] private string nextSceneName;

#if UNITY_EDITOR
    // 에디터에서 씬 에셋 이름을 자동으로 sceneName에 넣어주는 처리
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
