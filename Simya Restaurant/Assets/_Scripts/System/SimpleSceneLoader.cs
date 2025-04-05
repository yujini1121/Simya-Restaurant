using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// SceneAsset�� ������ ���� Ŭ�����̸�, ���� �� ���Ե��� �ʽ��ϴ�.
/// LoadScene()�� ����Ϸ���, nextSceneName�� �Էµ� ����
/// Build Settings�� ��ϵǾ� �־�� ���� �۵��մϴ�.
/// </summary>

public class SimpleSceneLoader : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private SceneAsset nextSceneAsset;
#endif
    [SerializeField] private string nextSceneName;

#if UNITY_EDITOR
    // �� ���� �̸��� �ڵ����� sceneName�� �־��ִ� ó��
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
