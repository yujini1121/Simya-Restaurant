using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

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
public struct AttackExecutionTupule
{
    public GameObject hitBox;
    public Vector3 attackPos;
    public float actionTime;
    string animationName;
    float attackRangeSummonTime;
}

public class PlayerController : MonoBehaviour
{
    static public PlayerController instance;

    //public int PotionCount;
    [Header("Movement")]
    //바보들을 위한 움직임 방향 결정 변수 // 잘못했어요!
    [SerializeField] private bool falseIsMoveX__trueISMoveZ;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float acceleration = 2f;   // 가속도 계수 (감속 구현하려고 만든 변수) / 값이 클수록 빠르게 변함
    [SerializeField] private float stepOffset;  // 계단, 턱 등을 무시할 수 있는 높이
    [SerializeField] private float fowardCheckDistance;
    [SerializeField] private bool isFowardBlocked;
    private bool isMoveBlocking = false;
    private float inputX;
    private float inputZ;
    private Vector3 inputDir;
    private Vector3 fowardCheckTop;
    private Vector3 fowardCheckBottom;
    private Rigidbody playerRb;

    [System.Serializable]
    public class JumpAndGravity
    {
        public float jumpForce = 10f;
        public float gravity;
        public float verticalVelocity;

        [HideInInspector] public bool jumpInput;
    }
    [Header("Jump And Gravity")]
    [SerializeField] private JumpAndGravity _jumpAndGravity = new JumpAndGravity();
    private JumpAndGravity JumpGravity => _jumpAndGravity;

    // 지면 체크 및 경사면 변수
    [System.Serializable]
    public class GroundAndSlope
    {
        public bool isGround = true;
        public bool slopeIsSteep;
        public float slopeAngle;
        public float slopeAngleLimit;
        public float castRadius;
        public float checkDistance;
        public float curGroundDistance;
        public float groundCheckThreshold;
        public LayerMask groundLayer;
        public Vector3 groundCross;

        [HideInInspector]public float capsuleRadiusDiff;
    }
    [Header("Ground And Slope")]
    [SerializeField] private GroundAndSlope _groundAndSlope = new GroundAndSlope();
    private GroundAndSlope GroundSlope => _groundAndSlope;

    // 플레이어 상태 변수
    private EPlayerStatus currentPlayerStatus;
#warning isAttacking 변수 삭제할 것
    private bool isAttacking = false;

    // 플레이어 방향
    private int playerLookingDirection = 1; // 안타깝게도 playerDirection만으로 플레이어가 현재 바라보는 방향을 저장할 수 없습니다. 해봤어요.
    private const int LOOK_RIGHT = 1;
    private const int LOOK_LEFT = -1;

    [Space (10f)]
    [Header("Light Attack")]
    // 플레이어 약공격 파트
    [SerializeField] private List<AttackExecutionTupule> HitboxAttackLight;
    [SerializeField] private float comboAttackResetTime;
    [SerializeField] private float comboAttackFinalDelay;
    private Coroutine lightAttackResetCoroutine;
    private int lightAttackCombo = 0;

    // 플레이어 강공격 파트
    [Space (5f)]
    [Header("Heavy Attack")]
    [SerializeField] private AttackExecutionTupule HitboxAttackHeavy;
    private Coroutine heavyAttackCoroutine;
    [SerializeField] private float heavyAttackChargeTime;
    private bool isHeavyAttackReady = false;

    // 플레이어 애니메이션
    [SerializeField] private Animator animatorController;

    // 플레이어 체력 정보
    //[SerializeField] public float health;
    public float Health
    {
        get => dataController.Access().health;
        set
        {
            dataController.Access().health = value;
            PlayerHealthUI.instance.UpdateHealth();
        }
    }
    [SerializeField] public float maxHealth;
    public bool IsDead { get; private set; }

    // 임시 변수
    private string BaseLayer = "Base Layer.";

    // 시간 담당 변수
    bool isInputAllowed = true;
    bool isPlayerDoing = false;
    System.Action nextAct = null;
    [SerializeField] private float prePressAllowTime = 0.5f;
    Coroutine inputBlockingCoroutine;

    // 상호 작용 변수
    List<InteractiveObjectBase> approchableInteractives;
    int index = 0;

    [SerializeField] private Collider collider;

    // 외부 게임오브젝트 컴포넌트
    private DataController dataController;
    private readonly float potionHealAmount = 30.0f;
    [SerializeField] private readonly float potionCooltime = 3.0f;

    [SerializeField] private ParticleSystem qParticle;
    [SerializeField] private ParticleSystem wParticle;
    [SerializeField] private ParticleSystem eParticle;
    [SerializeField] private ParticleSystem rParticle;

    [Space(5f)]
    [Header("DEBUG VALUE")]
    [SerializeField] private bool isDebugForceMove;
    [SerializeField] private bool isDebugGroundCheck = false;

    /// <summary>
    ///     플레이어의 공격받은 것을 구현하는 함수입니다.
    /// </summary>
    /// <param name="damage"> 데미지의 양 </param>
    /// <param name="knockBackDirection"></param>
    /// <param name="force"></param>
    public void BeAttacked(float damage, Vector3 knockBackDirection, float force)
    {
        Health -= damage;

        if (Health <= 0.0f && (IsDead == false))
        {
            DoDeathHandle();
        }

        playerRb.AddForce(knockBackDirection.normalized * force, ForceMode.Impulse);
    }
    
    public void AddInteractive(InteractiveObjectBase target)
    {
        if (m_FindInteractive(target) != -1)
        {
            return;
        }
        approchableInteractives.Add(target);
    }

    public void RemoveInteractive(InteractiveObjectBase target)
    {
        int index = m_FindInteractive(target);
        if (index == -1)
        {
            return;
        }
        approchableInteractives.RemoveAt(index);
    }

    public void CallWhenSceneEnd()
    {
        dataController.SaveData();
    }

    public void CallWhenSceneStart()
    {
        dataController.LoadData();
    }

    public void PausePlayer()
    {
        isMoveBlocking = true;
    }

    public void ResumePlayer()
    {
        isMoveBlocking = false;
    }

    private void DoDeathHandle()
    {
        // 나중에 여기 구현하세요.
        IsDead = true;
        currentPlayerStatus = EPlayerStatus.dead;
        
        if (animatorController != null)
        {
            animatorController.CrossFade($"Dead", 0.15f);
        }
        else
        {
            Debug.Log("DoDeathHandle()가 존재하지 않습니다!");
        }
        dataController.Access().isDead = true;

        StartCoroutine(UtilityFunctions.RunAfterDelay(
            2.0f,
            () =>
            {
                SceneManager.LoadScene("Home");
            }));
    }

    private void Awake()
    {
        approchableInteractives = new List<InteractiveObjectBase>(32);

        if (instance == null)
        {
            instance = this;
        }

        maxHealth = 100.0f;
    }

    void Start()
    {
        qParticle.Stop();
        wParticle.Stop();
        eParticle.Stop();
        rParticle.Stop();

        if (playerRb == null)
        {
            playerRb = GetComponent<Rigidbody>();
        }

        EnemyBase.SetPlayer(gameObject);
        //animatorController = transform.Find("Armature").GetComponent<Animator>();

        // 함수 정상 진행 조건
        Debug.Assert(playerRb != null);
        //Debug.Assert(animatorController != null);

        CapsuleCollider capsule = GetComponent<CapsuleCollider>();

        GroundSlope.castRadius = capsule.radius;
        GroundSlope.capsuleRadiusDiff = capsule.radius - GroundSlope.castRadius + 0.05f;

        fowardCheckTop = new Vector3(0f, capsule.center.y + (capsule.height / 2), 0f);
        fowardCheckBottom = new Vector3(0f, capsule.center.y - (capsule.height / 2) + stepOffset, 0f);

        dataController = GameObject.Find("Data Controller").GetComponent<DataController>();
        //Time.timeScale = 0.2f;
    }

    void Update()
    {
        InputObserver();

        m_AddPotion();
        m_UsePotion();
        m_ReduceHP();
        m_Save();
        M_TEMP_ItemSetting();
        M_TEMP_SetRecipe();

        m_DebuggingForceMove();
        //m_HandleStatus();
        //DoAttackLight();
        //DoAttackHeavy(); //여기 바로 윗줄 포함 주석해제
        DoInteractive();
        //
        //TEMP_PlayAnimate();
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log($"레시피가 있는가? : {dataController.HasRecepie("Chicken Pizza").isExist}");
        }
        M_TEMP_Kill();


    }

    private void FixedUpdate()
    {
        GroundCheck();
        CheckFoward();
        Gravity();

        Movement();
    }

    void InputObserver()
    {
        if (isMoveBlocking)
        {
            return;
        }

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        inputDir = new Vector3(inputX, 0f, inputZ).normalized;

        if (GroundSlope.isGround && Input.GetKeyDown(KeyCode.Space))
        {
            JumpGravity.jumpInput = true;
            animatorController.SetTrigger("isJump");
            //collider.contactOffset

            //collider.center = new Vector3(collider.center.x, jumpCenterY, playerCollider.center.z);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            animatorController.SetTrigger("Combo1");
            qParticle.Play();
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            animatorController.SetTrigger("Combo2");
            wParticle.Play();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            animatorController.SetTrigger("Combo3");
            eParticle.Play();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            animatorController.SetTrigger("HeavyAttack");
            rParticle.Play();
            
        }
        DoAttackHeavy();
    }

    void Movement()
    {
        if (IsDead) return;

        // 방향 설정 (LookRotation 방식)
        if (inputDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(inputDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
        }

        // 점프 처리
        if (JumpGravity.jumpInput && GroundSlope.isGround)
        {
            JumpGravity.verticalVelocity = JumpGravity.jumpForce;
            GroundSlope.isGround = false;
            JumpGravity.jumpInput = false;
        }

        // 이동 처리
        Vector3 horizontalVelocity = inputDir * moveSpeed;

        // 경사면 보정
        if (GroundSlope.isGround || GroundSlope.curGroundDistance < GroundSlope.checkDistance)
        {
            horizontalVelocity = Quaternion.AngleAxis(-GroundSlope.slopeAngle, GroundSlope.groundCross) * horizontalVelocity;
        }

        // 최종 적용
        playerRb.velocity = horizontalVelocity + Vector3.up * JumpGravity.verticalVelocity;

        // 애니메이션
        animatorController.SetFloat("SpeedX", inputX);
        animatorController.SetFloat("SpeedZ", inputZ);
    }

    void GroundCheck()
    {
        GroundSlope.curGroundDistance = float.MaxValue;
        Vector3 groundNormal = Vector3.up;
        GroundSlope.slopeAngle = 0f;
        GroundSlope.isGround = false;
        GroundSlope.slopeIsSteep = false;

        Vector3 groundCastPos = transform.position + new Vector3(0f, GroundSlope.castRadius + 0.05f, 0f);

        bool groundCast = Physics.SphereCast(groundCastPos, GroundSlope.castRadius, Vector3.down, out var groundHit,
                                                GroundSlope.checkDistance, GroundSlope.groundLayer);

        if (groundCast)
        {
            groundNormal = groundHit.normal;
            GroundSlope.curGroundDistance = Mathf.Max(groundHit.distance - GroundSlope.capsuleRadiusDiff - GroundSlope.groundCheckThreshold, -10f);
            GroundSlope.slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
            GroundSlope.slopeIsSteep = GroundSlope.slopeAngle >= GroundSlope.slopeAngleLimit;
            if (isDebugGroundCheck)
            {
                Debug.Log($"DEBUG_PlayerController.GroundCheck() : {groundHit.distance - GroundSlope.capsuleRadiusDiff}");
            }

            Debug.DrawLine(groundCastPos + new Vector3(-1f, 0f, 0f), groundCastPos + new Vector3(1f, 0f, 0f));
            Debug.DrawLine(groundCastPos + new Vector3(-1f, -groundHit.distance - GroundSlope.capsuleRadiusDiff, 0f), groundCastPos + new Vector3(1f, -groundHit.distance - GroundSlope.capsuleRadiusDiff, 0f));
            GroundSlope.isGround = GroundSlope.curGroundDistance <= 0.0001f && !GroundSlope.slopeIsSteep;

            //Debug.Log(groundHit.distance + " / " + SV.curGroundDistance);
        }

        GroundSlope.groundCross = Vector3.Cross(groundNormal, Vector3.up);
    }

    void m_Chat()
    {
        inputX = Input.GetAxis("Horizontal");

    }

    private void CheckFoward()
    {
        bool cast = Physics.CapsuleCast(transform.position + fowardCheckBottom, transform.position + fowardCheckTop, GroundSlope.castRadius,
                                            new Vector3(playerLookingDirection, 0, 0), out var hit, fowardCheckDistance, GroundSlope.groundLayer);

        isFowardBlocked = false;
        if (cast)
        {
            float forwardObstacleAngle = Vector3.Angle(hit.normal, Vector3.up);
            isFowardBlocked = forwardObstacleAngle >= GroundSlope.slopeAngleLimit;
        }
    }

    private void Gravity()
    {
        if (GroundSlope.isGround)
        {
            JumpGravity.verticalVelocity = 0f;
        }
        else
        {
            JumpGravity.verticalVelocity += Time.fixedDeltaTime * JumpGravity.gravity;
        }
    }

    /// <summary>
    ///     약공격 실행 함수
    /// </summary>
    /// <remarks>
    ///     SRP 원칙을 지켜주세요 : 이 함수는 너무 무책임해서 약공격 실행만 책임집니다.
    /// </remarks>
    void DoAttackLight()
    {
        ReadyAttackLight();

        // 빈 현재 행동에 nextAct를 집어넣음
        if (nextAct != null && (isPlayerDoing == false)) // 이때 현재 행동중이 아니여야 함
        {
            nextAct();
            nextAct = null;
        }
    }

    /// <summary>
    ///     조건에 맞는 상태인 경우, 약공격 액션을 준비합니다.
    /// </summary>
    void ReadyAttackLight()
    {
        // 얼리 리턴 패턴을 사용하기 위해 함수 블록을 따로 빼두었습니다.
        if (currentPlayerStatus == EPlayerStatus.interaction ||
            currentPlayerStatus == EPlayerStatus.dead
            // 그 외 각종 상태는 허용
            )
        {
            return;
        }
        if (isInputAllowed == false)
        {
            //Debug.Log("공격 입력을 받을 수 없음");
            return;
        }
        if (Input.GetButtonDown("AttackLight") == false)
        {
            //Debug.Log("공격 입력 안함");
            return;
        }
        if (nextAct != null)
        {
            //Debug.Log("현재 행동 중 + 이미 예약이 가득 참");
            return;
        }
        // nextAct에 값 쑤셔넣기
        nextAct = () =>
        {
            if (lightAttackResetCoroutine != null)
            {
                StopCoroutine(lightAttackResetCoroutine);
            }
            lightAttackCombo++;

            Debug.Assert(
                lightAttackCombo >= 1 && lightAttackCombo <= 3,
                "PlayerController.DoAttackLight : 유효하지 않은 lightAttackCombo 값.");
            animatorController.CrossFade($"LightAttackCombo{lightAttackCombo}", 0.15f);

            if (lightAttackCombo < 3)
            {
                lightAttackResetCoroutine = StartCoroutine(
                    RunAfterDelay(
                        HitboxAttackLight[lightAttackCombo - 1].actionTime + comboAttackResetTime,
                        () => { lightAttackCombo = 0; }
                        )
                    );

                MakeHitbox(HitboxAttackLight[lightAttackCombo - 1]);
                inputBlockingCoroutine = BlockInputForSeconds(HitboxAttackLight[lightAttackCombo - 1].actionTime);
            }
            else
            {
                lightAttackCombo = 0;
                lightAttackResetCoroutine = StartCoroutine(
                    RunAfterDelay(
                        comboAttackFinalDelay,
                        () =>
                        {
                            MakeHitbox(HitboxAttackLight[2]);
                            inputBlockingCoroutine = BlockInputForSeconds(HitboxAttackLight[2].actionTime);
                        }
                        )
                    );
            }
            nextAct = null;
        };
    }

    /// <summary>
    ///     일정시간동안 입력을 받지 못하게 합니다.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    Coroutine BlockInputForSeconds(float time)
    {
        isInputAllowed = false;
        isPlayerDoing = true;
        
        return StartCoroutine(
            RunAfterDelay(
                (time - prePressAllowTime, () => { isInputAllowed = true; }),
                (prePressAllowTime, () => { isPlayerDoing = false; })
                )
            );
    }

    void DoAttackHeavy()
    {
        // 민혁씨 인용) 상호작용 시에는 강공격 차징 풀리게 할 것 같네욘
        if (currentPlayerStatus == EPlayerStatus.interaction ||
            currentPlayerStatus == EPlayerStatus.dead)                      // 그 외 각종 상태는 허용
        {
            return;
        }
        if (isInputAllowed == false)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            heavyAttackCoroutine
                = StartCoroutine(
                    RunAfterDelay(
                        heavyAttackChargeTime,
                        () => {
                            isHeavyAttackReady = true;
                        }
                        )
                    );
        }
        // if (Input.GetButtonUp("AttackHeavy") == false)
        if (Input.GetKeyUp(KeyCode.R) == false)
        {
            return;
        }

        if (isHeavyAttackReady)
        {
            animatorController.CrossFade("HeavyAttack", 0.15f);
            MakeHitbox(HitboxAttackHeavy);
            inputBlockingCoroutine = BlockInputForSeconds(HitboxAttackHeavy.actionTime);
        }
        else
        {
            if (heavyAttackCoroutine != null)
            {
                StopCoroutine(heavyAttackCoroutine);
            }
        }
    }

    void DoInteractive()
    {
        if (Input.GetKeyDown(KeyCode.X) == false)
        {
            return;
        }
        if (isMoveBlocking)
        {
            return;
        }
        if (approchableInteractives.Count == 0)
        {
            Debug.Log("인터랙티브 할 수 있는 대상이 없음");
            return;
        }
        Debug.Log("인터랙티브 진행");

        InteractiveObjectBase[] sortedArray = approchableInteractives.ToArray();
        UtilityFunctions.Sort(
            ref sortedArray,
            (InteractiveObjectBase left, InteractiveObjectBase right) =>
            {
                return (left.transform.position - transform.position).sqrMagnitude >
                    (right.transform.position - transform.position).sqrMagnitude;
            }
            );

        sortedArray[0].DoInteractiveWithThis();
    }

    void m_Select()
    {
        
    }

    /// <summary>
    ///     대기 시간 후 호출할 함수의 일련을 준비합니다.
    /// </summary>
    /// <param name="actionSequences">
    ///     대기 시간과 대기시간 후 실행할 무명 메서드의 튜플입니다.
    /// </param>
    /// <returns>
    ///     코루틴을 돌려줍니다.
    /// </returns>
    IEnumerator RunAfterDelay(params (float waitingTime, System.Action nextAction)[] actionSequences)
    {
        // for문을 따로 떼어둔 것은, 호출 시점에서 오류를 미리 잡아두는 것이 가장 좋기 때문입니다.
        for (int index = 0; index < actionSequences.Length; ++index)
        {
            Debug.Assert(
                actionSequences[index].waitingTime >= 0.0f,
                $"오류_PlayerController.RunAfterDelay : {index + 1}번째 시퀀스의 waitingTime이 {actionSequences[index].waitingTime}이며 0보다 작습니다."
                );
            Debug.Assert(
                actionSequences[index].nextAction != null, 
                $"오류_PlayerController.RunAfterDelay : {index + 1}번째 시퀀스의 nextAction이 널 값입니다.");
        }

        for (int index = 0; index < actionSequences.Length; ++index)
        {
            yield return new WaitForSeconds(actionSequences[index].waitingTime);
            actionSequences[index].nextAction();
        }
    }

    /// <summary>
    ///     대기 시간 후 호출할 함수를 준비합니다.
    /// </summary>
    /// <param name="waitingTime">
    ///     대기할 시간입니다.
    /// </param>
    /// <param name="nextAction">
    ///     waitingTime 뒤에 실행할 코드입니다. 무명 메서드를 넣을 수 있고, 혹은 입력과 출력이 void인 함수명을 넣을 수 있습니다.
    /// </param>
    /// <returns>
    ///     코루틴을 돌려줍니다.
    /// </returns>
    IEnumerator RunAfterDelay(float waitingTime, System.Action nextAction)
    {
        return RunAfterDelay((waitingTime, nextAction));
    }

    /// <summary>
    ///     공격 내용을 담은 튜플을 이용해 히트박스 객체를 생성합니다. 1초당 20회 이상 호출되는것을 권장하지 않습니다.
    /// </summary>
    /// <param name="attack"></param>
    void MakeHitbox(AttackExecutionTupule attack)
    {
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

    private int m_FindInteractive(InteractiveObjectBase target)
    {
        for (int index = 0; index < approchableInteractives.Count; ++index)
        {
            if (ReferenceEquals(approchableInteractives[index], target))
            {
                return index;
            }
        }
        return -1;
    }

    private void m_UsePotion()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) == false)
        {
            return;
        }
        if (dataController == null)
        {
            Debug.LogError("Data Controller가 존재하지 않습니다! 하이어라키에 Data Controller이라는 이름의 게임오브젝트를 만들고, 해당 게임오브젝트에 DataController 컴포넌트를 어테치하세요!");
            return;
        }
        if (dataController.Access().potionsRemain < 1)
        {
            return;
        }
        if (dataController.Access().cooltimePotion > 0.0f)
        {
            Debug.Log($"m_AddPotion() : 현재 포션에 쿨타임이 돌고 있습니다. {dataController.Access().cooltimePotion}");
            return;
        }
        if (isMoveBlocking)
        {
            Debug.LogWarning("PlayerController.m_UsePotion() : 플레이어가 블로킹 되었습니다.");
            return;
        }

        dataController.Access().potionsRemain--;
        dataController.Access().cooltimePotion = potionCooltime;
        PlayerHealthUI.instance.UpdatePotionCount(dataController.Access().potionsRemain);
        Health = Mathf.Min(maxHealth, Health + potionHealAmount);

        
        Debug.Log("m_UsePotion()");
    }

    private void m_AddPotion()
    {
        if (Input.GetKey(KeyCode.Backslash))
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0) == false)
        {
            return;
        }
        if (dataController == null)
        {
            Debug.LogError("Data Controller가 존재하지 않습니다! 하이어라키에 Data Controller이라는 이름의 게임오브젝트를 만들고, 해당 게임오브젝트에 DataController 컴포넌트를 어테치하세요!");
            return;
        }

        dataController.Access().potionsRemain++;
        PlayerHealthUI.instance.UpdatePotionCount(dataController.Access().potionsRemain);
        Debug.Log($"m_AddPotion() : {dataController.Access().potionsRemain}");
    }

    private void m_ReduceHP()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon) == false)
        {
            return;
        }

        Health = Mathf.Max(0.0f, Health - 10.0f);

        Debug.Log("m_ReduceHP()");
    }

    private void m_Save()
    {
        if (!Input.GetKey(KeyCode.LeftShift) || !Input.GetKeyDown(KeyCode.I))
        {
            return;
        }
        if (isMoveBlocking)
        {
            Debug.LogWarning("PlayerController.m_Save() : 플레이어가 블로킹 되었습니다.");
            return;
        }
        dataController.SaveData();
    }

    private void m_DebuggingForceMove()
    {
        if (isDebugForceMove == false)
        {
            return;
        }

        float speed = 7.0f;
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            if (Input.GetKey(KeyCode.Comma))
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Period))
            {
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }

    private void m_HandleStatus()
    {
        dataController.Access().cooltimePotion = 
            Mathf.Max(0.0f, dataController.Access().cooltimePotion - Time.deltaTime);
    }

    private void M_TEMP_ItemSetting()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.O))
        {
            dataController.AddItem(200, 1);
            Debug.Log($"M_TEMP_ItemSetting() : 아이템이 추가되었습니다.");
        }
    }

    private void M_TEMP_SetRecipe()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.C))
        {
            dataController.AddRecepie("snail and chocolate pasta");
            Debug.Log("M_TEMP_SetRecipe() : 레시피 추가됨");
        }

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.V))
        {
            dataController.TryRemoveRecipe("snail and chocolate pasta");
            Debug.Log("M_TEMP_SetRecipe() : 레시피 제거됨");
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(dataController.HasRecepie("snail and chocolate pasta").isExist ? "has recipe" : "no recipe");
        }
    }

    private void M_TEMP_Kill()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            DoDeathHandle();
        }
    }
}