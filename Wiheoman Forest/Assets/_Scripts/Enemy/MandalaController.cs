using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MandalaController : MonoBehaviour
{
    [SerializeField] private float sensingRange;
    [SerializeField] private float mandalaJump;
    [SerializeField] private LayerMask playerLayer;

    private bool isPerceive;
    private Rigidbody mandalaRb;
    private Vector3 mandalaPos;

    void Start()
    {
        if (mandalaRb == null)
        {
            mandalaRb = GetComponent<Rigidbody>();
        }
        mandalaRb.useGravity = false;
    }

    void Update()
    {
        // CheckSensingRange();

        if (Physics.CheckSphere(transform.position, sensingRange, playerLayer))
        {
            mandalaRb.AddForce(Vector3.up * mandalaJump, ForceMode.Impulse);
            mandalaRb.useGravity = true;
            isPerceive = false;
        }
    }

    private void CheckSensingRange()
    {
        isPerceive = Physics.CheckSphere(transform.position, sensingRange, playerLayer);
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, sensingRange);
    }
}
