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
    // �����Ϳ��� �� ���� �̸��� �ڵ����� sceneName�� �־��ִ� ó��
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
            Debug.LogWarning("Scene name is empty. ����� SceneAsset�� ���� �� ����.");
        }
    }
}
