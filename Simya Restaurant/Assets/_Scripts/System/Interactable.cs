using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float checkRadius;

    private int playerLayer;


    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        CheckObject();
    }

    void CheckObject()
    {
        if (Input.GetKeyDown(KeyCode.D) && Physics.CheckSphere(transform.position, checkRadius, playerLayer))
        {
            Debug.Log("상호작용");
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, checkRadius);
    }
#endif
}
