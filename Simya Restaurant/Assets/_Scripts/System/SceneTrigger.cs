using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private string loadToScene;
    private bool sceneChanging = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !sceneChanging)
        {
            sceneChanging = true;
            SceneTransition.Instance.ChangeScene(loadToScene);
            Debug.Log(loadToScene);
        }
    }
}
