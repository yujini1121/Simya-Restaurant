using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;
using TMPro;

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
    [SerializeField] private float attackchargingTime;        // 공격을 하기 위해 충전하는 시간
    [SerializeField] private float attackDuration;            // 근거리 꽃몬 공격 지속 시간
    [SerializeField] private float attackCooldown;            // 다음 공격까지의 쿨타임

    [Header("Value Set")]
    [SerializeField] private float moveRange;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform thorn;

    private int playerLayer;
    private GameObject player;

    private Rigidbody rb;
    private Vector3 startPos;


    void Start()
    {
        player = GameObject.Find("Player");
        playerLayer = LayerMask.GetMask("Player");

        rb = player.GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void Update()
    {
        player.GetComponent<Test_PlayerMove>().dameged = false;

        switch (curState)
        {
            case FlowerMonState.Roaming:
                StartCoroutine("Roaming");

                if (Physics.CheckSphere(transform.position, searchRadius, playerLayer))
                {
                    StartCoroutine(ChangeNextState(FlowerMonState.Charging));
                }
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
                Debug.Log($"{name} : 기본 상태");
                break;
            case FlowerMonState.Charging:
                Debug.Log($"{name} : 공격 준비 상태");
                break;
            case FlowerMonState.Attack:
                Debug.Log($"{name} : 공격 상태");
                break;
            case FlowerMonState.Cooldown:
                Debug.Log($"{name} : 쿨타임 상태");
                break;
        }
        #endregion
    }

    IEnumerator ChangeNextState(FlowerMonState nextState, float waitTime = 0f)
    {
        yield return new WaitForSeconds(waitTime);

        switch (nextState)
        {
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

    IEnumerator Roaming()
    {
        int romDir = Random.Range(-1, 2);
        float romDistance = Random.Range(1, 5);

        Vector3 targetPos = startPos + new Vector3(romDir * romDistance, 0, 0);

        while (Vector3.Distance(startPos, targetPos) > 0f)
        {
            transform.position = Vector3.MoveTowards(startPos, targetPos, moveSpeed * Time.deltaTime);
        }

        yield return null;
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