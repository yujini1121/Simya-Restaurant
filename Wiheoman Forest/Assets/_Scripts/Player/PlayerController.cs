using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StatMachine
{
    private class Layer
    {
        
    }

    


    // 초기화 영역


    // 수정 영역

}

public enum EPlayerStatAction
{
    none,
    alive,
    dead,
    interaction
}

/// <summary>
///     플레이어 공격 액션(강공격/약공격/ 화살공격)을 실행할때 필요한 매개변수 목록입니다.
/// </summary>
[Serializable]
public struct AttackTupule
{
    public GameObject hitBox;
    public Vector3 attackPos;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float jumpForce = 10f;
    private float playerInput;
    private Rigidbody playerRb;
    private bool isGround = true;
    // 플레이어 상태 변수
    private EPlayerStatAction playerActionStatus;
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
    
    void Start()
    {
        if(playerRb == null)
        {
            playerRb = GetComponent<Rigidbody>();
        }

        EnemyBase.SetPlayer(gameObject);
        
        // 함수 정상 진행 조건
        Debug.Assert(playerRb != null);
    }

    void Update()
    {
        Move();
        Jump();

        DoAttackLight();
        DoAttackHeavy();
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
    
    void Move()
    {
        playerInput = Input.GetAxis("Horizontal");
        playerVec = new Vector3(playerInput, 0, 0).normalized;
        transform.position += playerVec * moveSpeed * Time.deltaTime;

        if (Math.Abs(playerInput) > 0.1) // playerInput != 0.0f보다 안전
        {
            playerLookingDirection
                = (playerInput > 0.0f) ? LOOK_RIGHT : LOOK_LEFT;
        }
    }

    /// <summary>
    /// 
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
        Debug.Log("DoAttackLight() : 실행됨");
        if (lightAttackResetCoroutine != null)
        {
            StopCoroutine(lightAttackResetCoroutine);
        }
        lightAttackCombo++;

        if (lightAttackCombo < 3)
        {
            lightAttackResetCoroutine = StartCoroutine(ResetComboAttack());
            DoAttack(HitboxAttackLight[lightAttackCombo - 1]);
        }
        else
        {
            lightAttackCombo = 0;
            StartCoroutine(LightAttackLastCombo());
        }

        
    }

    void DoAttackHeavy()
    {
        // 민혁씨 인용) 상호작용 시에는 강공격 차징 풀리게 할 것 같네욘
        if (playerActionStatus == EPlayerStatAction.interaction ||
            playerActionStatus == EPlayerStatAction.dead
            // 그 외 각종 상태는 허용
            )
        {
            return;
        }

        if (Input.GetButtonDown("AttackHeavy"))
        {
            heavyAttackCoroutine = StartCoroutine(ChargeHeavyAttack());
        }
        if (Input.GetButtonUp("AttackHeavy") == false)
        {
            return;
        }

        if (isHeavyAttackReady)
        {
            DoAttack(HitboxAttackHeavy);
        }
        else
        {
            StopCoroutine(heavyAttackCoroutine);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }

    IEnumerator ResetComboAttack()
    {
        yield return new WaitForSeconds(comboAttackResetTime);
        lightAttackCombo = 0;
    }

    IEnumerator LightAttackLastCombo()
    {
        yield return new WaitForSeconds(comboAttackFinalDelay);
        DoAttack(HitboxAttackLight[2]);
    }

    IEnumerator ChargeHeavyAttack()
    {
        yield return new WaitForSeconds(heavyAttackChargeTime);
        isHeavyAttackReady = true;
    }

    /// <summary>
    ///     공격 내용을 담은 튜플을 이용해 히트박스 객체를 생성합니다. 1초당 20회 이상 호출되는것을 권장하지 않습니다.
    /// </summary>
    /// <param name="attack"></param>
    void DoAttack(AttackTupule attack)
    {
        Debug.Log("DoAttack(AttackTupule attack) : 실행됨");
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
    }
}
