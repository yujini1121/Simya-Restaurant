using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 현재 상태를 저장하는 열거자입니다.
public enum EPlayerStatus
{
    none,
    idle,
    dead,
    moving,
    interaction,
}

public enum EPlayerAction
{
    empty,
    lightAttack,
    heavyAttack,
}

/// <summary>
///     플레이어 공격 액션(강공격/약공격/ 화살공격)을 실행할때 필요한 매개변수 목록입니다.
/// </summary>
[System.Serializable]
public struct AttackTupule
{
    public GameObject hitBox;
    public Vector3 attackPos;
    public float actionTime;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;
    private float playerInput;
    private Rigidbody playerRb;
    private bool isGround = true;
    // 플레이어 상태 변수
    private EPlayerStatus currentPlayerStatus;
    private EPlayerAction currentPlayerAction;
    private EPlayerAction nextPlayerAction;
    private bool isAttacking = false;
    // 플레이어 방향
    private Vector3 playerVec;
    private int playerLookingDirection = 1; // 안타깝게도 playerVec만으로 플레이어가 현재 바라보는 방향을 저장할 수 없습니다. 해봤어요.
    private const int LOOK_RIGHT = 1;
    private const int LOOK_LEFT = -1;
    // 플레이어 약공격 파트
    [SerializeField] private List<AttackTupule> HitboxAttackLight;
    [SerializeField] private float comboAttackResetTime;
    [SerializeField] private float comboAttackFinalDelay;
    private Coroutine lightAttackResetCoroutine;
    private int lightAttackCombo = 0;

    // 플레이어 강공격 파트
    [SerializeField] private AttackTupule HitboxAttackHeavy;
    private Coroutine heavyAttackCoroutine;
    [SerializeField] private float heavyAttackChargeTime;
    private bool isHeavyAttackReady = false;
    // 플레이어 애니메이션
    private Animator animatorController;
    private string parameterLightAttackCombo = "lightAttackCombo";

    // 플레이어 체력 정보
    [SerializeField] public float health;
    // 임시 변수
    private string BaseLayer = "Base Layer.";

    void Start()
    {
        if(playerRb == null)
        {
            playerRb = GetComponent<Rigidbody>();
        }

        EnemyBase.SetPlayer(gameObject);
        animatorController = transform.Find("Armature").GetComponent<Animator>();
        
        // 함수 정상 진행 조건
        Debug.Assert(playerRb != null);
        Debug.Assert(animatorController != null);
    }

    void Update()
    {
        Move();
        Jump();

        //HandleButton();

        DoAttackLight();
        DoAttackHeavy(); //여기 바로 윗줄 포함 주석해제

        TEMP_PlayAnimate();
    }

    void Move()
    {
        playerInput = Input.GetAxis("Horizontal");
        playerVec = new Vector3(playerInput, 0, 0).normalized;
        transform.position += playerVec * moveSpeed * Time.deltaTime;

        if (Mathf.Abs(playerInput) > 0.1) // playerInput != 0.0f보다 안전
        {
            playerLookingDirection
                = (playerInput > 0.0f) ? LOOK_RIGHT : LOOK_LEFT;
        }
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround) 
        {
            playerRb.velocity = Vector3.zero;
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }
    }

    /// <summary>
    ///     플레이어가 여러 버튼을 누르는것에 대응합니다. 즉 버튼들의 신호등 역할을 합니다.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         플레이어가 현재 행동하고 있는지 여부를 판단하고, 
    ///         만약 비어있다면 즉시 실행, 진행 중이라면 예약을 하고, 
    ///         만약 예약마저 꽉찼다면 무시합니다. 현재 행동이 완료되는 즉시 예약된 행동을 실행합니다.
    ///     </para>
    ///     <para>
    ///         플레이어가 아무것도 하고 있지 않는다면 이동 버튼이 먹힙니다
    ///         이동중 공격 버튼이 눌린다면 이동 버튼은 무시됩니다.
    ///     </para>
    /// </remarks>
    void HandleButton()
    {
        if (nextPlayerAction != EPlayerAction.empty)
        {
            return;
        }

        // 가만히 있을때 할수 있는 것들
        if (currentPlayerAction == EPlayerAction.empty)
        {
            Move();
            Jump();
        }


        // 행동중 홀드 키가 눌린 경우
        // 행동이 끝난 뒤에도 홀드 키가 눌리는지 체크하기
        // 버튼 눌림 감지 -> 플레이어 현재 행동 종료 후에도 홀드 키가 눌리는지 감지 -> 행동



    }

    /// <summary>
    ///     약공격 실행
    /// </summary>
    /// <remarks>
    ///     SRP 원칙을 지켜주세요 : 이 함수는 너무 무책임해서 약공격 실행만 책임집니다.
    /// </remarks>
    void DoAttackLight()
    {
        if (Input.GetButtonDown("AttackLight") == false)
        {
            return;
        }
        if (isAttacking == true)
        {
            return;
        }
        //Debug.Log("DoAttackLight() : 실행됨");
        if (lightAttackResetCoroutine != null)
        {
            StopCoroutine(lightAttackResetCoroutine);
        }
        lightAttackCombo++;
        switch (lightAttackCombo)
        {
            case 1: animatorController.CrossFade("LightAttackCombo1", 0.15f); break;
            case 2: animatorController.CrossFade("LightAttackCombo2", 0.15f); break;
            case 3: animatorController.CrossFade("LightAttackCombo3", 0.15f); break;
            default: break;
        }

        if (lightAttackCombo < 3)
        {
            lightAttackResetCoroutine = StartCoroutine(ResetComboAttack(HitboxAttackLight[lightAttackCombo - 1].actionTime));

            Debug.LogWarning("주석 내용 확인해주세요");
            /*
                Unity C# 기능인 Func, Action 사용하여 메서드(특히 코루틴) 범용성 있게 작성
                위 코드는 아래와 같이 작성 가능

                lightAttackResetCoroutine = StartCoroutine(OverloadingCoroutine(HitboxAttackLight[lightAttackCombo - 1].actionTime + comboAttackResetTime,
                                                                    () => {
                                                                        lightAttackCombo = 0;
                                                                        animatorController.SetInteger(parameterLightAttackCombo, lightAttackCombo);
                                                                    }));
            */




            DoAttack(HitboxAttackLight[lightAttackCombo - 1]);
        }
        else
        {
            lightAttackCombo = 0;
            StartCoroutine(LightAttackLastCombo());
            Debug.LogWarning("주석 내용 확인해주세요");
            /*
                Unity C# 기능인 Func, Action 사용하여 메서드(특히 코루틴) 범용성 있게 작성
                위 코드는 아래와 같이 작성 가능

                lightAttackResetCoroutine = StartCoroutine(OverloadingCoroutine(comboAttackFinalDelay,
                                                                    () => { DoAttack(HitboxAttackLight[2]); }));
            */
        }
    }

    IEnumerator SetAttacking(float time)
    {
        isAttacking = true;
        yield return new WaitForSeconds(time);
        isAttacking = false;
    }

    IEnumerator ResetComboAttack(float time)
    {
        yield return new WaitForSeconds(comboAttackResetTime + time);
        lightAttackCombo = 0;
        animatorController.SetInteger(parameterLightAttackCombo, lightAttackCombo);
    }

    IEnumerator LightAttackLastCombo()
    {
        yield return new WaitForSeconds(comboAttackFinalDelay);
        DoAttack(HitboxAttackLight[2]);
    }

    void DoAttackHeavy()
    {
        // 민혁씨 인용) 상호작용 시에는 강공격 차징 풀리게 할 것 같네욘
        if (currentPlayerStatus == EPlayerStatus.interaction ||
            currentPlayerStatus == EPlayerStatus.dead
            // 그 외 각종 상태는 허용
            )
        {
            return;
        }
        if (isAttacking)
        {
            return;
        }

        if (Input.GetButtonDown("AttackHeavy"))
        {
            heavyAttackCoroutine = StartCoroutine(ChargeHeavyAttack());

            Debug.LogWarning("주석 내용 확인해주세요");
            /*
                Unity C# 기능인 Func, Action 사용하여 메서드(특히 코루틴) 범용성 있게 작성
                위 코드는 아래와 같이 작성 가능

                lightAttackResetCoroutine = StartCoroutine(OverloadingCoroutine(heavyAttackChargeTime,
                                                                    () => {isHeavyAttackReady = true;}));
            */
        }
        if (Input.GetButtonUp("AttackHeavy") == false)
        {
            return;
        }

        if (isHeavyAttackReady)
        {
            animatorController.CrossFade("HeavyAttack", 0.15f);
            DoAttack(HitboxAttackHeavy);
        }
        else
        {
            if (heavyAttackCoroutine != null)
            {
                StopCoroutine(heavyAttackCoroutine);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    IEnumerator ChargeHeavyAttack()
    {
        yield return new WaitForSeconds(heavyAttackChargeTime);
        isHeavyAttackReady = true;
    }

    IEnumerator OverloadingCoroutine(float t, System.Action nextAction = null)
    {
        yield return new WaitForSeconds(t);
        nextAction.Invoke();
    }

    /// <summary>
    ///     공격 내용을 담은 튜플을 이용해 히트박스 객체를 생성합니다. 1초당 20회 이상 호출되는것을 권장하지 않습니다.
    /// </summary>
    /// <param name="attack"></param>
    void DoAttack(AttackTupule attack)
    {
        //Debug.Log("DoAttack(AttackTupule attack) : 실행됨");
        Quaternion rotation = attack.hitBox.transform.rotation;
        if (playerLookingDirection == LOOK_RIGHT)
        {
            rotation.eulerAngles += new Vector3(0, 180, 0);
        }
        Instantiate(
            attack.hitBox,
            new Vector3(
                transform.position.x + attack.attackPos.x * playerLookingDirection,
                transform.position.y + attack.attackPos.y,
                transform.position.z + attack.attackPos.z),
            rotation);
        StartCoroutine(SetAttacking(attack.actionTime));
    }

    void TEMP_PlayAnimate()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animatorController.Play($"{BaseLayer}LightAttackCombo1");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            animatorController.Play($"{BaseLayer}LightAttackCombo2");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            animatorController.Play($"{BaseLayer}LightAttackCombo3");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            animatorController.Play($"{BaseLayer}HeavyAttack");
        }
    }


}