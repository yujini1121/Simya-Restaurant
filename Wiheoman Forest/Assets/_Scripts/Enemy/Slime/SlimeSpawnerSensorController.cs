using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawnerSensorController : MonoBehaviour
{
    bool hasSpawned = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name != PlayerController.instance.gameObject.name)
        {
            return;
        }
        if (hasSpawned == false)
        {
            transform.parent.GetChild(0).gameObject.SetActive(true);
            hasSpawned = true;
        }

    }
}
