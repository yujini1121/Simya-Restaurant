using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MandalaController : EnemyBase
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
    // [SerializeField] private float mandalaMoveSpeed;
    [SerializeField] private float mandalaExplosionRange;

    private float mandalaMoveSpeed;

    private bool isPerceive;
    private bool isOnGround = false;
    private Rigidbody mandalaRb;
    private Rigidbody playerRb;
    private GameObject player;
    private Collider col;
    private Vector3 mandalaDir;

    private float mandalaJumpMax = 1.5f;
    private float mandalaDamage = 0.3f;
    private float explosionDelay = 2.0f;

    private bool isWaitAttack = false;
    private bool isSFXPlayed = false;

    protected override void DoDeathHandle()
    {
        
    }

    void Start()
    {
        MandlaInit();
    }

    void Update()
    {
        CheckPlayer();
        SetMandlaDir();
        MoveMandla();
    }

    private void MandlaInit()
    {
        if (!mandalaRb)
        {
            mandalaRb = GetComponent<Rigidbody>();
        }
        mandalaRb.useGravity = false;
        col = GetComponent<Collider>();

        player = GameObject.Find("Player");
        playerRb = player.GetComponent<Rigidbody>();
        mandalaMoveSpeed = player.GetComponent<Test_PlayerMove>().moveSpeed;

        switch (type)
        {
            case MandalaType.Herb:
                mandalaMoveSpeed *= 0.8f;
                break;
            case MandalaType.Explosion:
                mandalaMoveSpeed *= 1.2f;
                break;
        }
    }

    private void CheckPlayer()
    {
        if (!isPerceive && Physics.CheckSphere(transform.position, sensingRange, playerLayer))
        {
            isPerceive = true;
            StartCoroutine(upGround());
        }
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
    }

    private void SetMandlaDir()
    {
        if (isOnGround && isPerceive)
        {
            switch (type)
            {
                case MandalaType.Herb:
                    mandalaDir = (transform.position - playerRb.position).normalized;

                    // 나중에 더 수정 필요... 얘기 해봐야 할 듯
                    if(!isSFXPlayed)
                    {
                        AudioManager.instance.PlaySfx(AudioManager.SFX.Mandla_long);
                        isSFXPlayed = true;
                    }

                    break;
                case MandalaType.Explosion:
                    mandalaDir = (playerRb.position - transform.position).normalized;
                    break;
            }
        }
    }

    private void MoveMandla()
    {
        transform.position += mandalaDir * mandalaMoveSpeed * Time.deltaTime;
    }

    IEnumerator DamagePlayer(GameObject player)
    {
        mandalaMoveSpeed = 0;
        isWaitAttack = true;

        yield return new WaitForSeconds(explosionDelay);

        // AudioManager.instance.PlaySfx(AudioManager.SFX.TestSFX_1);

        if (Physics.CheckSphere(transform.position, mandalaExplosionRange, playerLayer))
        {
            player.GetComponent<Test_PlayerMove>().dameged = true;

            yield return new WaitForSeconds(0);

            player.GetComponent<Test_PlayerMove>().dameged = false;
        }
        isWaitAttack = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case MandalaType.Herb:
                    gameObject.SetActive(false);
                    break;
                case MandalaType.Explosion:
                    StartCoroutine(DamagePlayer(collision.gameObject));
                    break;
            }            
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        Handles.DrawWireDisc(transform.position, Vector3.forward, sensingRange);

        if(isWaitAttack)
        {
            Handles.color = new Color(1f, 0f, 0f, 0.2f);
            Handles.DrawSolidDisc(transform.position, Vector3.forward, mandalaExplosionRange);
        }
    }
#endif
}
