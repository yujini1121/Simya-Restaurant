using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string loadToScene;
    private bool sceneChanging = false;
    [SerializeField] private bool isDebugging = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !sceneChanging)
        {
            sceneChanging = true;
            SceneTransition.Instance.ChangeScene(loadToScene);
            if (isDebugging)
            {
                Debug.Log(loadToScene);
            }

        }
    }
}
