using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class FlowerMon : MonoBehaviour
{
    /// <summary>
    /// FlowerMon 타입 관리 (Melee : 근거리 / Ranged : 원거리) 
    /// </summary>
    [System.Serializable]
    public enum FlowerMonType
    {
        Melee,
        Ranged
    }
    public FlowerMonType type;


    /// <summary>
    /// FlowerMon 상태 관리 
    /// </summary>
    private enum FlowerMonState 
    {
        Roaming,
        Following,
        Charging, 
        Attack, 
        Cooldown 
    }
    FlowerMonState curState = FlowerMonState.Roaming;


    [Header("Attack")]
    [SerializeField] private float searchRadius;
    [SerializeField] private float attackRadius;
    
    [SerializeField] 
    [Tooltip("공격을 하기 위해 충전하는 시간")]
    private float attackchargingTime;

    [SerializeField]
    [Tooltip("근거리 꽃몬 공격 지속 시간")] 
    private float attackDuration;

    [SerializeField]
    [Tooltip("다음 공격까지의 쿨타임")] 
    private float attackCooldown;


    [Header("Options")]
    [SerializeField] private float moveRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform thorn;

    //[Header("Ground")]
    //[SerializeField] private GameObject ground;
    //[SerializeField] private Vector3 groundCenterPos;
    //[SerializeField] private float groundOffset = 3f;

    private int playerLayer;
    private GameObject player;
    private Rigidbody playerRb;
    private Rigidbody flowermonRb;

    private Vector3 startPos;

    private Coroutine roamingCor;


    void Start()
    {
        player = GameObject.Find("Player");
        playerLayer = LayerMask.GetMask("Player");

        playerRb = player.GetComponent<Rigidbody>();
        flowermonRb = gameObject.GetComponent<Rigidbody>();

        startPos = transform.position;
    }

    void Update()
    {
        player.GetComponent<Test_PlayerMove>().dameged = false;

        switch (curState)
        {
            case FlowerMonState.Roaming:
                if (Physics.CheckSphere(transform.position, searchRadius, playerLayer))
                {
                    if (roamingCor != null)
                    {
                        StopCoroutine(roamingCor);
                        roamingCor = null;
                    }
                    StartCoroutine(ChangeNextState(FlowerMonState.Following));
                }
                else if (roamingCor == null)
                {
                    roamingCor = StartCoroutine(Roaming(Random.Range(-1, 2), Random.Range(1.0f, 3.0f)));
                }
                break;

            case FlowerMonState.Following:
                Debug.Log("오잉");
                Vector3 awayFromPlayerDir = (player.transform.position - transform.position).normalized;
                transform.Translate(awayFromPlayerDir * moveSpeed * Time.deltaTime);
                break;

            case FlowerMonState.Attack:
                if (Physics.CheckSphere(transform.position, attackRadius, playerLayer))
                {
                    player.GetComponent<Test_PlayerMove>().dameged = true;
                }
                break;
        }

        #region Debug
        switch (curState)
        {
            case FlowerMonState.Roaming:
                Debug.Log($"{name} : Roaming");
                break;
            case FlowerMonState.Following:
                Debug.Log($"{name} : Following");
                break;
            case FlowerMonState.Charging:
                Debug.Log($"{name} : Charging");
                break;
            case FlowerMonState.Attack:
                Debug.Log($"{name} : Attack");
                break;
            case FlowerMonState.Cooldown:
                Debug.Log($"{name} : Cooldown");
                break;
        }
        #endregion
    }

    IEnumerator ChangeNextState(FlowerMonState nextState, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);

        switch (nextState)
        {
            case FlowerMonState.Following:
                StartCoroutine(ChangeNextState(FlowerMonState.Charging, 3f));
                break;
            case FlowerMonState.Charging:
                StartCoroutine(ChangeNextState(FlowerMonState.Attack, attackchargingTime));
                break;
            case FlowerMonState.Attack:
                StartCoroutine(ChangeNextState(FlowerMonState.Cooldown, attackDuration));
                break;
            case FlowerMonState.Cooldown:
                StartCoroutine(ChangeNextState(FlowerMonState.Roaming, attackCooldown));
                break;
        }
        curState = nextState;
    }


    void TurnObject()
    {
        Vector3 reverse = transform.localScale;
        reverse.x = -transform.localScale.x;
        transform.localScale = reverse;
    }


    IEnumerator Roaming(int action, float moveT, float waitT = 0.0f)
    {
        float curTime = 0.0f;

        if (action == 0)
        {
            yield return new WaitForSeconds(waitT);
        }
        else
        {
            Vector3 dir = transform.right * action;

            while (curTime < moveT)
            {
                curTime += Time.deltaTime;
                transform.Translate(dir * moveSpeed * Time.deltaTime);

                yield return null;
            }
            //yield return new WaitForSeconds(waitT);
        }

        roamingCor = StartCoroutine(Roaming(Random.Range(-1, 2), Random.Range(1.0f, 3.0f), Random.Range(1.0f, 3.0f)));
    }






    #region Draw Scene View Only 
    private void OnDrawGizmos()
    {
        switch (curState)
        {
            case FlowerMonState.Roaming:
                Handles.color = Color.green;
                Handles.DrawWireDisc(transform.position, Vector3.forward, searchRadius);
                break;
            case FlowerMonState.Following:
                Handles.color = Color.blue;
                Handles.DrawWireDisc(transform.position, Vector3.forward, searchRadius);
                break;
            case FlowerMonState.Charging:
                Handles.color = Color.red;
                Handles.DrawWireDisc(transform.position, Vector3.forward, searchRadius);
                break;
            case FlowerMonState.Attack:
                Handles.color = new Color(1f, 0f, 0f, 0.2f);
                Handles.DrawSolidDisc(transform.position, Vector3.forward, attackRadius);
                break;
            case FlowerMonState.Cooldown:
                Handles.color = new Color(0.5f, 0.5f, 0.5f, 0.2f);
                Handles.DrawSolidDisc(transform.position, Vector3.forward, searchRadius);
                break;
        }
    }
    #endregion
}