using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationYOffset = 180f;

    private void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        //transform.LookAt(cameraTransform.position);
        //transform.Rotate(0, rotationYOffset, 0);
    }
}
