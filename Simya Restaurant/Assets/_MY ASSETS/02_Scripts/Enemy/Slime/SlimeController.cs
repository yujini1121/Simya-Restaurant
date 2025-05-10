using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class SlimeController : EnemyBase
{
    [SerializeField] private float timeForJumpInterval;
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveForce;
    [SerializeField] private float speedOfRoamingMin;
    [SerializeField] private float speedOfRoamingMax;
    [SerializeField] private float timeForRoamingIdle;
    [SerializeField] private float timeForRoamingMoving;
    [SerializeField] private float lengthForRoamingRange;
    [SerializeField] private bool isHidden;
    [SerializeField] private Vector3 spawnRaidusLeftDown;
    [SerializeField] private Vector3 spawnRaodusRightUp;
    [SerializeField] private Vector3 offsetForRoamingCenter;
    private Vector3 anchorForRoamingCenter;
    private bool isJumpReady = true;
    private bool m_isPlayerFound = true;
    private Coroutine jumpCoroutine;
    private Coroutine roamCoroutine;

    private bool isAttackMode = true;

    private void Awake()
    {
        anchorForRoamingCenter = transform.position + offsetForRoamingCenter;
        enemyRigidbody = GetComponent<Rigidbody>();
        if (enemyRigidbody == null)
        {
            Debug.LogError("SlimeController.Awake() : 리지드바디를 찾을 수 없습니다.");
        }
        if (timeForJumpInterval <= 0.0f)
        {
            Debug.LogWarning($"SlimeController.Awake() : timeForJumpInterval 의 값이 너무 작습니다({timeForJumpInterval}) -> 해당 값을 1.0f으로 강제로 수정합니다.");
            timeForJumpInterval = 1.0f;
        }
    }

    void Start()
    {
        if (IsFoundPlayer())
        {
            HandlePlayerFound();
            m_isPlayerFound = true;
        }
        else
        {
            HandlePlayerLost();
            m_isPlayerFound = false;
        }
    }

    void Update()
    {
        if ((m_isPlayerFound == true) && (IsFoundPlayer() == false))
        {
            HandlePlayerLost();
            m_isPlayerFound = false;
        }
        else if ((m_isPlayerFound == false) && (IsFoundPlayer() == true))
        {
            HandlePlayerFound();
            m_isPlayerFound = true;
        }

        
        if (m_isPlayerFound)
        {
            if (isJumpReady && jumpCoroutine == null)
            {
                //jumpCoroutine = StartCoroutine(JumpCoroutine());
            }
            // 아무것도 아니라면 가만히 있기
        }
    }

    protected override void DoDeathHandle()
    {
        // ===============================
        // 슬라임이 사망했으므로 더이상 점프를 하지 않습니다.
        // ===============================
        StopCoroutine(jumpCoroutine);
        isAttackMode = false;
        Debug.Log("슬라임이 사망했습니다.");
    }

    // ===============================
    // TODO : 해당 슬라임이 점프를 해서 튀어오르는 동안엔
    // 슬라임이 플레이어에게 닿게 된 경우 데미지가 들어가도록 해야 합니다.
    // 하지만 플레이어에게 데미지를 주는 함수가 없고 플레이어 체력에 관여하는 어떤 통일된 표준이 없어요.
    // =============================== 
    /// <summary>
    ///     슬라임이 jumpInterval 만큼의 주기로 통통 튀어오르는 함수입니다.
    /// </summary>
    /// <returns></returns>
    //private IEnumerator Jump()
    //{
    //    while (true)
    //    {
    //        // 스턴인 경우, 점프를 스킵합니다
    //        if (isStuned)
    //        {
    //            continue;
    //        }
    //        //Debug.Log($"found player : {IsFoundPlayer()}");
    //        // 점프
    //        enemyRigidbody.AddForce(
    //            new Vector3(0, jumpForce, 0) + (playerScript.IsDead ? Vector3.zero : GetPseudoDirection()) * moveForce,
    //            ForceMode.VelocityChange);
    //        isAttackMode = true;
    //        yield return new WaitForSeconds(timeForJumpInterval);
    //    }
    //}

    private void HandlePlayerFound()
    {
        Debug.Log("멈춤");
        if (roamCoroutine != null)
        {
            StopCoroutine(roamCoroutine);
        }
        jumpCoroutine = StartCoroutine(JumpCoroutine());
    }

    private void HandlePlayerLost()
    {
        Debug.Log("잃음");

        if (jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
        }
        roamCoroutine = StartCoroutine(RoamingCoroutine());
    }


    private IEnumerator JumpCoroutine()
    {
        while (true)
        {
            isJumpReady = false;

            if (isStuned == false)
            {
                enemyRigidbody.AddForce(
                    new Vector3(0, jumpForce, 0) + (playerScript.IsDead ? Vector3.zero : GetPseudoDirection()) * moveForce,
                    ForceMode.VelocityChange);
                isAttackMode = true;
            }
            StartCoroutine(
                UtilityFunctions.RunAfterDelay(
                    timeForJumpInterval,
                    () =>
                    {
                        isJumpReady = true;
                    }));
            yield return new WaitForSeconds(timeForJumpInterval);
            // N초 뒤 러차지
        }
    }

    private IEnumerator RoamingCoroutine()
    {
        float m_Time = 0.0f;
        if (timeForRoamingMoving <= 0.0f)
        {
            Debug.LogWarning($"SlimeController.RoamingCoroutine() : timeForRoamingMoving 의 값이 너무 작습니다({timeForRoamingMoving}) -> 해당 값을 1.0f으로 강제로 수정합니다.");
            timeForRoamingMoving = 1.0f;
        }
        if (timeForRoamingIdle <= 0.0f)
        {
            Debug.LogWarning($"SlimeController.RoamingCoroutine() : timeForRoamingIdle 의 값이 너무 작습니다({timeForRoamingIdle}) -> 해당 값을 1.0f으로 강제로 수정합니다.");
            timeForRoamingIdle = 1.0f;
        }
        while (true) // 특별한 일 없으면 무한반복.
        {
            // 움직이기. 한 방향으로만 지속적으로.
            // 방향을 결정
            Vector3 toCenter = anchorForRoamingCenter - transform.position;
            Vector3 translation = Vector3.zero;

            // 너무 멀다 -> 중심으로 이동
            if (toCenter.sqrMagnitude > lengthForRoamingRange * lengthForRoamingRange)
            {
                toCenter.y = 0;
                toCenter.z = 0;
                translation = new Vector3(toCenter.normalized.x, 0, 0) * Random.Range(speedOfRoamingMin, speedOfRoamingMax);
            }
            // 가깝다 -> 네 맘대로 움직여
            else
            {
                if (Random.Range(0.0f, 1.0f) < 0.5f)
                {
                    translation = new Vector3(1, 0, 0) * Random.Range(speedOfRoamingMin, speedOfRoamingMax);
                }
                else
                {
                    translation = new Vector3(-1, 0, 0) * Random.Range(speedOfRoamingMin, speedOfRoamingMax);
                }
            }

            while (m_Time < timeForRoamingMoving) // while문 내부에 로직이 적어야 함
            {
                transform.Translate(translation * Time.deltaTime); // 프레임 타임은 매 프레임마다 다를 수 있음
                m_Time += Time.deltaTime;
                yield return new WaitForFixedUpdate(); // 한 프레임동안 기다리기
            }
            m_Time = 0.0f;
            // 잠시 멈추기
            while (m_Time < timeForRoamingIdle)
            {
                m_Time += Time.deltaTime;
                yield return null; // 한 프레임동안 기다리기
            }
            m_Time = 0.0f;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isAttackMode = false;
        }
        if (isStuned)
        {
            return;
        }
        if (collision.gameObject.name == "Player" && isAttackMode)
        {
            playerGameObject.GetComponent<PlayerController>().BeAttacked(30, GetPseudoDirection(), 2.0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            UtilityFunctions.DrawGizmoCircle(anchorForRoamingCenter, lengthForRoamingRange, 30);
        }
        else
        {
            UtilityFunctions.DrawGizmoCircle(transform.position + offsetForRoamingCenter, lengthForRoamingRange, 30);
        }
    }
}
