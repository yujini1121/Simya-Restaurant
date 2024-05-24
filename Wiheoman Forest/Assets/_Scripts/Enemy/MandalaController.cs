using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MandalaController : MonoBehaviour
{
    [System.Serializable]
    public enum MandalaType
    {
        Herb,
        Explosion
    }

    [SerializeField] private MandalaType type;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float sensingRange;
    [SerializeField] private float mandalaJumpSpeed;
    [SerializeField] private float mandalaMovSpeed;

    private bool isPerceive;
    private bool isOnGround = false;
    private Rigidbody mandalaRb;
    private GameObject player;
    private Rigidbody playerRb;
    private Collider col;
    private Vector3 mandalaDir;

    private float groundPos = 1.0f;
    private float mandalaJumpMax = 1.5f;

    void Start()
    {
        if (!mandalaRb)
        {
            mandalaRb = GetComponent<Rigidbody>();
        }
        mandalaRb.useGravity = false;
        col = GetComponent<Collider>();

        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isPerceive && Physics.CheckSphere(transform.position, sensingRange, playerLayer))
        {
            isPerceive = true;            
            StartCoroutine(upGround());
        }

        if (isOnGround && isPerceive)
        {
            switch (type)
            {
                case MandalaType.Herb:
                    mandalaDir = (transform.position - playerRb.position).normalized;
                    break;
                case MandalaType.Explosion:
                    mandalaDir = (playerRb.position - transform.position).normalized;
                    break;
            }
        }
        transform.position += mandalaDir * mandalaMovSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 땅에 심어져있던 만드라가 땅위로 올라오는 IEnumerator
    /// </summary>
    /// <returns></returns>
    IEnumerator upGround() 
    {
        while (transform.position.y <= mandalaJumpMax)
        {
            mandalaRb.AddForce(Vector3.up * mandalaJumpSpeed * Time.deltaTime, ForceMode.Impulse);
            yield return null;
        }
        mandalaRb.useGravity = true;

        col.isTrigger = false;
        isOnGround = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.forward, sensingRange);
    }
}
