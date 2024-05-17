using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


public class FlowerMon : MonoBehaviour
{
    /// <summary>
    /// FlowerMon 상태 관리 
    /// </summary>
    private enum FlowerMonState 
    { 
        Idle, 
        Charging, 
        Attack, 
        Cooldown 
    }
    FlowerMonState curState = FlowerMonState.Idle;

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

    [Header("Attack")]
    public float searchRadius;
    public float attackRadius;
    public float attackchargingTime;
    public float attackDuration;
    public float attackCooldown;


    int playerLayer;
    GameObject player;

    Transform thorn;

    void Start()
    {
        player = GameObject.Find("Player");
        playerLayer = LayerMask.GetMask("Player");

        thorn = transform.GetChild(0);
    }

    private void Update()
    {
        player.GetComponent<Test_PlayerMove>().dameged = false;

        switch (type)
        {
            /// <summary>
            /// 근거리 꽃몬  
            /// </summary>
            case FlowerMonType.Melee:
                switch (curState)
                {
                    case FlowerMonState.Idle:
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
                break;

            /// <summary>
            /// 원거리 꽃몬   
            /// </summary>
            case FlowerMonType.Ranged:
                switch (curState)
                {
                    case FlowerMonState.Idle:
                        if (Physics.CheckSphere(transform.position, searchRadius, playerLayer))
                        {
                            StartCoroutine(ChangeNextState(FlowerMonState.Charging));
                        }
                        break;
                    case FlowerMonState.Attack:
                        Debug.Log("Attack");
                        break;
                }
                break;
        }
        

        //switch (curState)
        //{
        //    case FlowerMonState.Idle:
        //        if (Physics.CheckSphere(transform.position, searchRadius, playerLayer))
        //        {
        //            StartCoroutine(ChangeNextState(FlowerMonState.Charging));
        //        }
        //        break;
        //    case FlowerMonState.Attack:
        //        if (Physics.CheckSphere(transform.position, attackRadius, playerLayer))
        //        {
        //            player.GetComponent<Test_PlayerMove>().dameged = true;
        //        }
        //        break;
        //}


        #region Debug
        switch (curState)
        {
            case FlowerMonState.Idle:
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
                StartCoroutine(ChangeNextState(FlowerMonState.Idle, attackCooldown));
                break;
        }
        curState = nextState;
    }

    private void OnDrawGizmos()
    {
        switch (curState)
        {
            case FlowerMonState.Idle:
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
}